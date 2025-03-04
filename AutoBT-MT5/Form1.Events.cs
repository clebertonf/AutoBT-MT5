using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace AutoBT_MT5
{
    public partial class Form1
    {
        public void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void BtnSelectOutputFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtOutputFolder.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void BtnStartBacktest_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolderPath.Text) || !Directory.Exists(txtFolderPath.Text))
                {
                    LogMessage("Por favor, selecione uma pasta contendo os EAs (.ex5).");
                    LogSeparator();
                    return;
                }

                if (string.IsNullOrEmpty(txtOutputFolder.Text) || !Directory.Exists(txtOutputFolder.Text))
                {
                    LogMessage("Por favor, selecione uma pasta para salvar os resultados.");
                    LogSeparator();
                    return;
                }

                string[] eaFiles = Directory.GetFiles(txtFolderPath.Text, "*.ex5");
                if (eaFiles.Length == 0)
                {
                    LogMessage("Nenhum EA encontrado na pasta selecionada.");
                    LogSeparator();
                    return;
                }

                int totalFiles = eaFiles.Length;
                progressBar.Maximum = totalFiles;
                progressBar.Value = 0;

                foreach (string eaFile in eaFiles)
                {
                    LogMessage($"Criando arquivo .ini para: {Path.GetFileName(eaFile)}");
                    string iniFilePath = Path.Combine(txtOutputFolder.Text,
                        Path.GetFileNameWithoutExtension(eaFile) + ".ini");
                    CreateIniFile(iniFilePath, eaFile);

                    LogMessage($"Iniciando backtest para: {Path.GetFileName(eaFile)}");
                    StartBacktest(eaFile, iniFilePath);
                    UpdateProgress(progressBar.Value + 1);
                    Thread.Sleep(3000);
                    LogSeparator();
                }

                UpdateProgress(totalFiles);
                LogMessage("Todos os backtests foram Finalizados.");
                MoveReportsFolder();
            }
            catch (Exception ex)
            {
                LogMessage($"Erro: {ex.Message}");
            }
        }

        private void StartBacktest(string eaFilePath, string iniFilePath)
        {
            try
            {
                string mt5TerminalPath = txtMt5Path.Text.Trim();

                if (string.IsNullOrEmpty(mt5TerminalPath))
                {
                    LogMessage("Erro: O caminho do terminal MT5 não foi fornecido.");
                    LogSeparator();
                    return;
                }

                string mt5ExecutablePath = Path.Combine(mt5TerminalPath, "terminal64.exe");

                if (!File.Exists(mt5ExecutablePath))
                {
                    LogMessage("Erro: O arquivo 'terminal64.exe' não foi encontrado.");
                    LogSeparator();
                    return;
                }

                string arguments = $"/config:{iniFilePath}";

                LogMessage($"Backtest iniciado para: {eaFilePath}");
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = mt5ExecutablePath,
                    WorkingDirectory = mt5TerminalPath,
                    Arguments = arguments,
                    UseShellExecute = true
                };

                Process mt5Process = Process.Start(processStartInfo);

                if (mt5Process != null)
                {
                    mt5Process.WaitForExit();
                }

                LogSeparator("----");
                LogMessage($"Backtest finalizado para: {eaFilePath}");
                LogSeparator();

                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                LogMessage($"Erro ao executar o backtest do EA '{eaFilePath}': {ex.Message}");
                LogSeparator();
            }
        }

        private void CreateIniFile(string iniFilePath, string eaFilePath)
        {
            try
            {
                string eaFileName = Path.GetFileName(eaFilePath);
                string userPath = txtFolderPath.Text;

                string metaQuotesPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MetaQuotes",
                        "Terminal");

                if (!Directory.Exists(metaQuotesPath))
                {
                    LogMessage("Erro: O diretório MetaQuotes\\Terminal não foi encontrado.");
                    return;
                }

                string? terminalFolder = Directory.GetDirectories(metaQuotesPath)
                    .FirstOrDefault(folder => Regex.IsMatch(Path.GetFileName(folder), @"^[A-Z0-9]+$"));

                if (string.IsNullOrEmpty(terminalFolder))
                {
                    LogMessage("Erro: Nenhuma pasta com ID dinâmico foi encontrada.");
                    return;
                }

                string mql5ExpertsFolder = Path.Combine(terminalFolder, "MQL5", "Experts");

                if (!Directory.Exists(mql5ExpertsFolder))
                {
                    LogMessage("Erro: O diretório MQL5\\Experts não foi encontrado.");
                    return;
                }

                string eaFolderPath = Path.GetDirectoryName(eaFilePath);
                string relativeEaPath =
                    eaFolderPath.Replace(mql5ExpertsFolder, "").TrimStart(Path.DirectorySeparatorChar);

                string basePath = userPath.Substring(0, userPath.IndexOf("MQL5"));
                string reportsFolder = Path.Combine(basePath, "Reports");

                if (!Directory.Exists(reportsFolder))
                {
                    Directory.CreateDirectory(reportsFolder);
                }

                StringBuilder iniContent = new StringBuilder();

                iniContent.AppendLine("[Tester]");
                iniContent.AppendLine($"Expert={Path.Combine(relativeEaPath, eaFileName)}");
                iniContent.AppendLine($"Symbol={txtSymbol.Text}");
                iniContent.AppendLine($"Period={cmbTimeFrame.SelectedItem}");
                iniContent.AppendLine($"FromDate={dtpStartDate.Value:yyyy.MM.dd}");
                iniContent.AppendLine($"ToDate={dtpEndDate.Value:yyyy.MM.dd}");
                iniContent.AppendLine($"Deposit={txtDeposit.Text}");
                iniContent.AppendLine($"Currency={cmbCurrency.SelectedItem}");
                iniContent.AppendLine("Optimization=0");
                iniContent.AppendLine("Model=1");
                iniContent.AppendLine($"Report=Reports/{eaFileName}.html");
                iniContent.AppendLine("ShutdownTerminal=1");

                File.WriteAllText(iniFilePath, iniContent.ToString());

                LogMessage($"Arquivo .ini criado: {iniFilePath}");
                LogSeparator("----");
            }
            catch (Exception ex)
            {
                LogMessage($"Erro ao criar o arquivo .ini: {ex.Message}");
                LogSeparator();
            }
        }

        private void BtnSelectMt5Path_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Selecione o diretório do MetaTrader 5";

                folderDialog.SelectedPath = @"C:\Program Files\MetaTrader 5 Terminal";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFolderPath = folderDialog.SelectedPath;
                    txtMt5Path.Text = selectedFolderPath;
                    string mt5ExecutablePath = Path.Combine(selectedFolderPath, "terminal64.exe");

                    if (File.Exists(mt5ExecutablePath))
                    {
                        LogSeparator();
                        LogMessage($"Caminho do MT5 scaneado: {mt5ExecutablePath}");
                        LogSeparator();
                    }
                    else
                    {
                        MessageBox.Show(
                            $"O arquivo 'terminal64.exe' não foi encontrado no diretório selecionado.\nCaminho: {mt5ExecutablePath}",
                            "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void LogMessage(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => { txtLog.AppendText(message + Environment.NewLine); }));
            }
            else
            {
                txtLog.AppendText(message + Environment.NewLine);
            }
        }

        public void LogSeparator(
            string separator =
                "------------------------------------------------------------------------------------------------")
        {
            LogMessage(separator);
        }

        public void UpdateProgress(int value)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action(() => { progressBar.Value = value; }));
            }
            else
            {
                progressBar.Value = value;
            }
        }

        private void MoveReportsFolder()
        {
            try
            {
                string metaQuotesPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MetaQuotes",
                        "Terminal");

                if (!Directory.Exists(metaQuotesPath))
                {
                    LogMessage("Erro: O diretório MetaQuotes\\Terminal não foi encontrado.");
                    return;
                }

                string? reportsFolder = Directory.GetDirectories(metaQuotesPath)
                    .FirstOrDefault(folder => Regex.IsMatch(Path.GetFileName(folder), @"^[A-Z0-9]+$"));

                if (string.IsNullOrEmpty(reportsFolder))
                {
                    LogMessage("Erro: Nenhuma pasta com ID dinâmico foi encontrada.");
                    LogSeparator();
                    return;
                }

                string reportsPath = Path.Combine(reportsFolder, "Reports");

                if (!Directory.Exists(reportsPath))
                {
                    LogMessage("Erro: A pasta Reports não foi encontrada dentro do diretório identificado.");
                    LogSeparator();
                    return;
                }

                string destinationFolder = txtOutputFolder.Text;
                if (string.IsNullOrEmpty(destinationFolder) || !Directory.Exists(destinationFolder))
                {
                    LogMessage("Erro: A pasta de destino não está definida ou não existe.");
                    LogSeparator();
                    return;
                }

                string destinationPath = Path.Combine(destinationFolder, "Reports");

                if (Directory.Exists(destinationPath))
                {
                    Directory.Delete(destinationPath, true);
                }

                Directory.Move(reportsPath, destinationPath);
                LogMessage($"Pasta Reports movida para: {destinationPath}");
                LogSeparator();
            }
            catch (Exception ex)
            {
                LogMessage($"Erro ao mover a pasta Reports: {ex.Message}");
                LogSeparator();
            }
        }
    }
}