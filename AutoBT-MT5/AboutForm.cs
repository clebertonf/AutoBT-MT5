using System.Diagnostics;

namespace AutoBT_MT5
{
    public class AboutForm : Form
    {
        public AboutForm()
        {
            this.Text = "Sobre";
            this.Size = new System.Drawing.Size(450, 245);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var mainPanel = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15),
                AutoSize = true,
                ColumnCount = 1
            };

            Label lblAbout = new Label()
            {
                Text = "AutoBT_MT5 - Automação de Backtest para o MetaTrader 5.\n" +
                       "Desenvolvido para facilitar a execução de backtests de forma automatizada.\n" +
                       "Executa backtestes em massa, facilitando o processo.\n" +
                        "Salva os relatorios em .HTML.\n\n",
                AutoSize = true
            };

            var linkDoc = CriarLinkLabel("Mais informações e documentação.", "https://github.com/clebertonf/AutoBT-MT5");
            var linkDev = CriarLinkLabel("Desenvolvido por Cleberton.", "https://github.com/clebertonf");
            
            var footerPanel = new TableLayoutPanel()
            {
                Dock = DockStyle.Bottom,
                ColumnCount = 2,
                AutoSize = true
            };

            FlowLayoutPanel licensePanel = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true
            };

            Label lblLicense = new Label()
            {
                Text = "MIT License",
                AutoSize = true
            };

            Label lblCopyright = new Label()
            {
                Text = "Copyright (c) 2025 ",
                AutoSize = true
            };

            LinkLabel linkAuthor = CriarLinkLabel("Cleberton Carvalho", "https://www.instagram.com/cleberton_dev/");
            
            licensePanel.Controls.Add(lblLicense);
            licensePanel.Controls.Add(lblCopyright);
            licensePanel.Controls.Add(linkAuthor);
            
            Button btnClose = new Button()
            {
                Text = "Fechar",
                AutoSize = true
            };
            btnClose.Click += (s, e) => this.Close();
            
            footerPanel.Controls.Add(licensePanel, 0, 0);
            footerPanel.Controls.Add(btnClose, 1, 0);
            footerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            footerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            
            mainPanel.Controls.Add(lblAbout);
            mainPanel.Controls.Add(linkDoc);
            mainPanel.Controls.Add(linkDev);
            mainPanel.Controls.Add(footerPanel);

            this.Controls.Add(mainPanel);
        }

        private LinkLabel CriarLinkLabel(string texto, string url)
        {
            var link = new LinkLabel()
            {
                Text = texto,
                AutoSize = true
            };
            link.Links.Add(0, texto.Length, url);
            link.LinkClicked += LinkLabel_LinkClicked;
            return link;
        }

        private void LinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = (e.Link?.LinkData as string)!;
            if (!string.IsNullOrEmpty(target))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = target,
                    UseShellExecute = true
                });
            }
        }
    }
}