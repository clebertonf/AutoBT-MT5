﻿namespace AutoBT_MT5
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.TextBox txtSymbol;
        private System.Windows.Forms.TextBox txtTimeFrame;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.TextBox txtDeposit;
        private System.Windows.Forms.ComboBox cmbCurrency;
        private System.Windows.Forms.Label lblFolderPath;
        private System.Windows.Forms.Label lblSymbol;
        private System.Windows.Forms.Label lblTimeFrame;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.Label lblDeposit;
        private System.Windows.Forms.Label lblCurrency;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button btnSelectOutputFolder;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.Button btnStartBacktest;
        private System.Windows.Forms.Button btnStopBacktest;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtMt5Path;
        private System.Windows.Forms.Button btnSelectMt5Path;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Label lblMt5Path;
        private System.Windows.Forms.CheckBox chkMinimizeMt5;
        private System.Windows.Forms.CheckBox chkMt5PortableMode;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 750);
            this.Text = "Automação de Backtest - MT5";
            
            lblFolderPath = new Label() { Text = "Pasta dos EAs:", Location = new System.Drawing.Point(20, 20), AutoSize = true };
            txtFolderPath = new TextBox() { Location = new System.Drawing.Point(120, 20), Width = 350 };
            btnSelectFolder = new Button() { Text = "Selecionar", Location = new System.Drawing.Point(480, 18) };
            btnSelectFolder.Click += BtnSelectFolder_Click;
            
            lblOutputFolder = new Label() { Text = "Resultados:", Location = new System.Drawing.Point(20, 60), AutoSize = true };
            txtOutputFolder = new TextBox() { Location = new System.Drawing.Point(120, 60), Width = 350 };
            btnSelectOutputFolder = new Button() { Text = "Selecionar", Location = new System.Drawing.Point(480, 58) };
            btnSelectOutputFolder.Click += BtnSelectOutputFolder_Click;
            
            lblSymbol = new Label() { Text = "Ativo:", Location = new System.Drawing.Point(20, 100), AutoSize = true };
            txtSymbol = new TextBox() { Location = new System.Drawing.Point(120, 100), Width = 100 };
            
            lblTimeFrame = new Label() { Text = "Time Frame:", Location = new System.Drawing.Point(20, 140), AutoSize = true };
            txtTimeFrame = new TextBox() { Location = new System.Drawing.Point(120, 140), Width = 100 };
            
            lblStartDate = new Label() { Text = "Data Início:", Location = new System.Drawing.Point(20, 180), AutoSize = true };
            dtpStartDate = new DateTimePicker() { Location = new System.Drawing.Point(120, 180), Format = DateTimePickerFormat.Short };
            dtpStartDate.Value = new DateTime(2018, 1, 1);

            lblEndDate = new Label() { Text = "Data Fim:", Location = new System.Drawing.Point(20, 220), AutoSize = true };
            dtpEndDate = new DateTimePicker() { Location = new System.Drawing.Point(120, 220), Format = DateTimePickerFormat.Short };
            
            lblDeposit = new Label() { Text = "Depósito:", Location = new System.Drawing.Point(20, 260), AutoSize = true };
            txtDeposit = new TextBox() { Location = new System.Drawing.Point(120, 260), Width = 100 };
            txtDeposit.Text = "10000";
            
            lblCurrency = new Label() { Text = "Moeda:", Location = new System.Drawing.Point(20, 300), AutoSize = true };
            cmbCurrency = new ComboBox() { Location = new System.Drawing.Point(120, 300), Width = 100 };
            cmbCurrency.Items.AddRange(new string[] { "BRL", "USD", "EUR", "GBP", "JPY", "AUD", "CAD" });
            cmbCurrency.SelectedItem = "BRL";
            
            lblMt5Path = new Label() { Text = "Caminho MT5:", Location = new System.Drawing.Point(20, 340), AutoSize = true };
            txtMt5Path = new TextBox() { Location = new System.Drawing.Point(120, 340), Width = 350 };
            btnSelectMt5Path = new Button() { Text = "Selecionar", Location = new System.Drawing.Point(480, 338) };
            btnSelectMt5Path.Click += BtnSelectMt5Path_Click;
            
            btnStartBacktest = new Button() { Text = "Iniciar Backtest", Location = new System.Drawing.Point(20, 380), Size = new System.Drawing.Size(150, 30) };
            btnStartBacktest.Click += BtnStartBacktest_Click;
            
            btnStopBacktest = new Button() { Text = "Parar", Location = new System.Drawing.Point(180, 380), Size = new System.Drawing.Size(150, 30) };
            btnStopBacktest.Click += BtnStopBacktest_Click;
            btnStopBacktest.Enabled = false;
            
            btnClearLog = new Button() { Text = "Limpar Log", Location = new System.Drawing.Point(20, 710), Size = new System.Drawing.Size(150, 30) };
            btnClearLog.Click += BtnClearLog_Click;

           
            GroupBox rightPanel = new GroupBox()
            {
                Location = new System.Drawing.Point(380, 150),
                Size = new System.Drawing.Size(150, 150),
                Text = "Opções",
                Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Italic),
                ForeColor = System.Drawing.Color.Black,
                BackColor = System.Drawing.Color.White
            };
            
            chkMinimizeMt5 = new CheckBox()
            {
                Text = "Minimizar MT5",
                Location = new System.Drawing.Point(388, 175),
                AutoSize = true,
                Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular)
            };
            
            chkMt5PortableMode = new CheckBox()
            {
                Text = "MT5-Portable",
                Location = new System.Drawing.Point(388, 198),
                AutoSize = true,
                Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular)
            };
            
            rightPanel.Controls.Add(chkMinimizeMt5);
            rightPanel.Controls.Add(chkMt5PortableMode);

            
            progressBar = new ProgressBar()
            {
                Location = new System.Drawing.Point(20, 420),
                Size = new System.Drawing.Size(550, 25),
                Style = ProgressBarStyle.Continuous
            };
            
            txtLog = new TextBox()
            {
                Location = new System.Drawing.Point(20, 460),
                Size = new System.Drawing.Size(550, 245),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                WordWrap = true
            };
            
            Button btnAbout = new Button() { 
                Text = "Sobre", 
                Location = new System.Drawing.Point(480, 710), 
                Size = new System.Drawing.Size(90, 30) 
            };
            btnAbout.Click += BtnAbout_Click;
            
            this.Controls.Add(lblFolderPath);
            this.Controls.Add(txtFolderPath);
            this.Controls.Add(btnSelectFolder);
            this.Controls.Add(lblOutputFolder);
            this.Controls.Add(txtOutputFolder);
            this.Controls.Add(btnSelectOutputFolder);
            this.Controls.Add(lblSymbol);
            this.Controls.Add(txtSymbol);
            this.Controls.Add(lblTimeFrame);
            this.Controls.Add(txtTimeFrame);
            this.Controls.Add(lblStartDate);
            this.Controls.Add(dtpStartDate);
            this.Controls.Add(lblEndDate);
            this.Controls.Add(dtpEndDate);
            this.Controls.Add(lblDeposit);
            this.Controls.Add(txtDeposit);
            this.Controls.Add(lblCurrency);
            this.Controls.Add(cmbCurrency);
            this.Controls.Add(lblMt5Path);
            this.Controls.Add(txtMt5Path);
            this.Controls.Add(btnSelectMt5Path);
            this.Controls.Add(btnStartBacktest);
            this.Controls.Add(btnStopBacktest);
            this.Controls.Add(progressBar);
            this.Controls.Add(txtLog);
            this.Controls.Add(btnClearLog);
            this.Controls.Add(chkMinimizeMt5);
            this.Controls.Add(chkMt5PortableMode);
            this.Controls.Add(rightPanel);
            this.Controls.Add(btnAbout);
        }
    }
}
