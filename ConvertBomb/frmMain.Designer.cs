namespace ConvertBomb
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnConvert = new System.Windows.Forms.Button();
            this.txtResource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.listError = new System.Windows.Forms.ListView();
            this.lblAbout = new System.Windows.Forms.Label();
            this.rBtnBomb = new System.Windows.Forms.RadioButton();
            this.rBtnMap = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnConvert
            // 
            this.btnConvert.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.btnConvert.Location = new System.Drawing.Point(12, 113);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(236, 64);
            this.btnConvert.TabIndex = 0;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // txtResource
            // 
            this.txtResource.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtResource.Location = new System.Drawing.Point(12, 51);
            this.txtResource.Name = "txtResource";
            this.txtResource.Size = new System.Drawing.Size(236, 24);
            this.txtResource.TabIndex = 1;
            this.txtResource.Text = "http://res2.gn.zing.vn/";
            this.txtResource.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Link Resource";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(24, 97);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(65, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Trạng Thái: ";
            // 
            // listError
            // 
            this.listError.Dock = System.Windows.Forms.DockStyle.Right;
            this.listError.FullRowSelect = true;
            this.listError.HideSelection = false;
            this.listError.Location = new System.Drawing.Point(355, 0);
            this.listError.Name = "listError";
            this.listError.Size = new System.Drawing.Size(231, 220);
            this.listError.TabIndex = 5;
            this.listError.UseCompatibleStateImageBehavior = false;
            // 
            // lblAbout
            // 
            this.lblAbout.AutoSize = true;
            this.lblAbout.Location = new System.Drawing.Point(144, 191);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(120, 13);
            this.lblAbout.TabIndex = 6;
            this.lblAbout.Text = "Phạm Ngọc Khánh Lâm";
            // 
            // rBtnBomb
            // 
            this.rBtnBomb.AutoSize = true;
            this.rBtnBomb.Location = new System.Drawing.Point(254, 113);
            this.rBtnBomb.Name = "rBtnBomb";
            this.rBtnBomb.Size = new System.Drawing.Size(92, 17);
            this.rBtnBomb.TabIndex = 7;
            this.rBtnBomb.TabStop = true;
            this.rBtnBomb.Text = "Convert Bomb";
            this.rBtnBomb.UseVisualStyleBackColor = true;
            // 
            // rBtnMap
            // 
            this.rBtnMap.AutoSize = true;
            this.rBtnMap.Location = new System.Drawing.Point(254, 138);
            this.rBtnMap.Name = "rBtnMap";
            this.rBtnMap.Size = new System.Drawing.Size(86, 17);
            this.rBtnMap.TabIndex = 8;
            this.rBtnMap.TabStop = true;
            this.rBtnMap.Text = "Convert Map";
            this.rBtnMap.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 220);
            this.Controls.Add(this.rBtnMap);
            this.Controls.Add(this.rBtnBomb);
            this.Controls.Add(this.lblAbout);
            this.Controls.Add(this.listError);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtResource);
            this.Controls.Add(this.btnConvert);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(602, 259);
            this.MinimumSize = new System.Drawing.Size(602, 259);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tool Convert Bomb Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox txtResource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListView listError;
        private System.Windows.Forms.Label lblAbout;
        private System.Windows.Forms.RadioButton rBtnBomb;
        private System.Windows.Forms.RadioButton rBtnMap;
    }
}