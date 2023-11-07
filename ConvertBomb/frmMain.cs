using Game.Logic.Phy.Maps;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ConvertBomb
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            SetupListView();
        }
        private string connectionString = ConfigurationManager.ConnectionStrings["ConvertBomb.Properties.Settings.DDT_Game34ConnectionString"].ConnectionString;
        private List<string> craterDownloadInfoList = new List<string>();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private async void btnConvert_Click(object sender, EventArgs e)
        {
            craterDownloadInfoList.Clear();

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
                                int result = 0;
                                int.TryParse(row["Crater"].ToString(), out result);
                                if (result > 0)
                                {
                                    craterDownloadInfoList.Add(row["Crater"].ToString() + "|" + row["ID"].ToString());
                                }
                            }
                        }
                    }
                }

                Directory.CreateDirectory("Crater");
                Directory.CreateDirectory("Bomb");
                WebClient webClient = new WebClient();

                foreach (string item in craterDownloadInfoList)
                {
                    string[] parts = item.Split('|');
                    string craterDirectory = Path.Combine("Crater", parts[0]);

                    if (!Directory.Exists(craterDirectory))
                    {
                        Directory.CreateDirectory(craterDirectory);
                    }

                    try
                    {
                        string url = Path.Combine(txtResource.Text, "image", "bomb", "crater", parts[0], "crater.png");
                        string destinationPath = Path.Combine(craterDirectory, "crater.png");

                        if (File.Exists(destinationPath))
                        {
                            File.Delete(destinationPath);
                        }

                        await webClient.DownloadFileTaskAsync(url, destinationPath);
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Convert " + parts[1] + ".bomb (fail!)";
                        log.Error("Path Error: " + ex.Message);
                    }

                    string bombFilePath = Path.Combine("Bomb", parts[1] + ".bomb");
                    CreateBombFile(Path.Combine(craterDirectory, "crater.png"), bombFilePath, parts[1]);

                    Invoke((MethodInvoker)delegate
                    {
                        lblStatus.Text = "Convert " + parts[1] + ".bomb (success!)";
                    });
                }

                MessageBox.Show("Convert finish!");
            }
            catch (Exception ex)
            {
                log.Error("SQL Connect Error : " + ex.Message);
            }
            finally
            {
                Invoke((MethodInvoker)delegate
                {
                    btnConvert.Enabled = true;
                });
            }
        }
        private void SetupListView()
        {
            listError.View = View.Details;
            listError.Columns.Add("Date Time");
            listError.Columns.Add("File Name");

            listError.Columns[0].Width = 150;
            listError.Columns[1].Width = 150;
        }


        private void CreateBombFile(string imagePath, string bombFilePath, string filename)
        {
            try
            {
                Bitmap bitmap = new Bitmap(imagePath);
                Tile tile = new Tile(bitmap, true);

                using (FileStream fileStream = File.Create(bombFilePath))
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(tile.Width);
                    binaryWriter.Write(tile.Height);
                    binaryWriter.Flush();
                    fileStream.Write(tile.Data, 0, tile.Data.Length);
                }
            }
            catch (Exception ex)
            {
                ListViewItem item = new ListViewItem(DateTime.Now.ToString());
                item.SubItems.Add($"Convert {filename}.bomb (fail!)");
                listError.Items.Add(item);
                lblStatus.Text = "Convert " + filename + ".bomb (fail!)";
            }
        }
    }
}
