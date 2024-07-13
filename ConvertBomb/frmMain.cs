using Game.Logic.Phy.Maps;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertBomb
{
    public partial class frmMain : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ConvertBomb.Properties.Settings.DDT_Game34ConnectionString"].ConnectionString;
        private List<string> craterDownloadInfoList = new List<string>();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public frmMain()
        {
            InitializeComponent();
            SetupListView();
            rBtnBomb.Checked = true;
        }

        private async void btnConvert_Click(object sender, EventArgs e)
        {
            btnConvert.Enabled = false;
            if (rBtnBomb.Checked)
            {
                await ConvertBomb();
            }
            else if (rBtnMap.Checked)
            {
                await ConvertMaps();
            }
            btnConvert.Enabled = true;
        }

        private void SetupListView()
        {
            listError.View = View.Details;
            listError.Columns.Add("Date Time", 150);
            listError.Columns.Add("File Name", 150);
        }

        private async Task ConvertBomb()
        {
            craterDownloadInfoList.Clear();
            try
            {
                await LoadCraterDownloadInfo();
                Directory.CreateDirectory("Crater");
                Directory.CreateDirectory("Bomb");

                using (WebClient webClient = new WebClient())
                {
                    foreach (string item in craterDownloadInfoList)
                    {
                        await ProcessCraterItem(item, webClient);
                    }
                }
                MessageBox.Show("Convert finish!");
            }
            catch (Exception ex)
            {
                log.Error("ConvertBomb Error: " + ex.Message);
            }
        }

        private async Task LoadCraterDownloadInfo()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string selectCommandText = "SELECT * FROM [dbo].[Ball]";
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommandText, connection))
                    {
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet, "Groups");
                        DataTable dataTable = dataSet.Tables["Groups"];

                        if (dataTable.Rows.Count > 0)
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {
                                if (int.TryParse(row["Crater"].ToString(), out int result) && result > 0)
                                {
                                    craterDownloadInfoList.Add($"{row["Crater"]}|{row["ID"]}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("SQL Connect Error : " + ex.Message);
                throw;
            }
        }

        private async Task ProcessCraterItem(string item, WebClient webClient)
        {
            string[] parts = item.Split('|');
            string craterDirectory = Path.Combine("Crater", parts[0]);

            Directory.CreateDirectory(craterDirectory);

            try
            {
                string url = Path.Combine(txtResource.Text, "image", "bomb", "crater", parts[0], "crater.png");
                string destinationPath = Path.Combine(craterDirectory, "crater.png");

                await webClient.DownloadFileTaskAsync(url, destinationPath);
                string bombFilePath = Path.Combine("Bomb", $"{parts[1]}.bomb");

                CreateBombFile(destinationPath, bombFilePath, parts[1]);
                lblStatus.Text = $"Convert {parts[1]}.bomb (success!)";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Convert {parts[1]}.bomb (fail!)";
                log.Error("Path Error: " + ex.Message);
            }
        }

        private void CreateBombFile(string imagePath, string bombFilePath, string filename)
        {
            try
            {
                using (Bitmap bitmap = new Bitmap(imagePath))
                {
                    Tile tile = new Tile(bitmap, true);

                    using (FileStream fileStream = File.Create(bombFilePath))
                    using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                    {
                        binaryWriter.Write(tile.Width);
                        binaryWriter.Write(tile.Height);
                        binaryWriter.Write(tile.Data, 0, tile.Data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                listError.Items.Add(new ListViewItem(new[] { DateTime.Now.ToString(), $"Convert {filename}.bomb (fail!)" }));
                lblStatus.Text = $"Convert {filename}.bomb (fail!)";
                log.Error("CreateBombFile Error: " + ex.Message);
            }
        }

        private async Task ConvertMaps()
        {
            string pathMap = @"SrcMap//";
            string pathCreateMap = @"Map//";

            if (!Directory.Exists(pathMap) || string.IsNullOrEmpty(pathCreateMap))
            {
                lblStatus.Text = "Source map directory does not exist or output path is empty.";
                return;
            }

            try
            {
                lblStatus.Text = "Starting map conversion...";
                Directory.CreateDirectory(pathCreateMap);

                string[] directories = Directory.GetDirectories(pathMap);
                if (directories.Length == 0)
                {
                    lblStatus.Text = "No directories found in source map path.";
                    return;
                }

                foreach (string dir in directories)
                {
                    await Task.Run(() => ProcessMapFiles(dir, pathCreateMap));
                }

                lblStatus.Text = "Map conversion finished!";
                MessageBox.Show("Map conversion finished!");
            }
            catch (Exception ex)
            {
                log.Error("ConvertMaps Error: " + ex.Message);
                lblStatus.Text = "Map conversion failed!";
            }
        }

        private void ProcessMapFiles(string directory, string outputPath)
        {
            lblStatus.Invoke((MethodInvoker)delegate { lblStatus.Text = $"Processing directory: {directory}"; });
            ProcessFile(directory, "fore.png", outputPath, "fore.map");
            ProcessFile(directory, "dead.png", outputPath, "dead.map");
        }

        private void ProcessFile(string directory, string inputFileName, string outputPath, string outputFileName)
        {
            string inputFilePath = Path.Combine(directory, inputFileName);
            if (File.Exists(inputFilePath))
            {
                try
                {
                    using (Bitmap bitmap = new Bitmap(inputFilePath))
                    {
                        Tile tile = new Tile(bitmap, digable: true);
                        string outputDirectory = Path.Combine(outputPath, new DirectoryInfo(directory).Name);
                        string outputFile = Path.Combine(outputDirectory, outputFileName);

                        Directory.CreateDirectory(outputDirectory);

                        using (FileStream fileStream = File.Create(outputFile))
                        using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                        {
                            binaryWriter.Write(tile.Width);
                            binaryWriter.Write(tile.Height);
                            binaryWriter.Write(tile.Data, 0, tile.Data.Length);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //listError.Items.Add(new ListViewItem(new[] { DateTime.Now.ToString(), $"Processing {inputFileName} in {directory} failed!" }));
                    //lblStatus.Invoke((MethodInvoker)delegate { lblStatus.Text = $"Processing {inputFileName} in {directory} failed!"; });
                    //log.Error("ProcessFile Error: " + ex.Message);
                    Invoke((MethodInvoker)delegate
                    {
                        listError.Items.Add(new ListViewItem(new[] { DateTime.Now.ToString(), $"Processing {inputFileName} in {directory} failed!" }));
                        lblStatus.Text = $"Processing {inputFileName} in {directory} failed!";
                    });
                    log.Error("ProcessFile Error: " + ex.Message);
                }
            }
            else
            {
                //listError.Items.Add(new ListViewItem(new[] { DateTime.Now.ToString(), $"{inputFileName} not found in {directory}!" }));
                //lblStatus.Invoke((MethodInvoker)delegate { lblStatus.Text = $"{inputFileName} not found in {directory}!"; });
                Invoke((MethodInvoker)delegate
                {
                    listError.Items.Add(new ListViewItem(new[] { DateTime.Now.ToString(), $"Processing {inputFileName} in {directory} failed!" }));
                    lblStatus.Text = $"Processing {inputFileName} in {directory} failed!";
                });
            }
        }
    }
}