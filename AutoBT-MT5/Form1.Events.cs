﻿using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace AutoBT_MT5
{
    public partial class Form1
    {
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isValisPath;

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

        private async void BtnStartBacktest_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolderPath.Text) || !Directory.Exists(txtFolderPath.Text))
                {
                    LogMessage("Por favor, selecione uma pasta contendo os EAs (.ex5).");
                    MessageBox.Show("Por favor, selecione uma pasta contendo os EAs (.ex5).", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogSeparator();
                    return;
                }

                if (string.IsNullOrEmpty(txtOutputFolder.Text) || !Directory.Exists(txtOutputFolder.Text))
                {
                    LogMessage("Por favor, selecione uma pasta para salvar os resultados.");
                    MessageBox.Show("Por favor, selecione uma pasta para salvar os resultados.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogSeparator();
                    return;
                }

                if (string.IsNullOrEmpty(txtSymbol.Text))
                {
                    LogMessage("Por favor, insira o simbolo do ativo.");
                    LogSeparator();
                    MessageBox.Show("Por favor, insira o simbolo do ativo.", "Erro", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(txtTimeFrame.Text))
                {
                    LogMessage("Por favor, insira o timeframe");
                    LogSeparator();
                    MessageBox.Show("Por favor, insira o timeframe", "Erro", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(txtMt5Path.Text) || !_isValisPath)
                {
                    LogMessage("Por favor, selecione o diretório do MetaTrader 5 antes de iniciar o backtest.");
                    LogSeparator();
                    MessageBox.Show("Por favor, selecione o diretório do MetaTrader 5 antes de iniciar o backtest.",
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string[] eaFiles = Directory.GetFiles(txtFolderPath.Text, "*.ex5");
                if (eaFiles.Length == 0)
                {
                    LogMessage("Nenhum EA encontrado na pasta selecionada.");
                    LogSeparator();
                    return;
                }

                LogSeparator();
                LogMessage("Parametros carregados com sucesso.");
                LogMessage($"Total de Experts encontrados na pasta: {eaFiles.Length}");
                string log = chkMinimizeMt5.Checked ? "MT5 minimizado" : "MT5 não minimizado";
                LogMessage($"{log}");
                LogMessage("Iniciando processo...");
                DateTime startTime = DateTime.Now;
                LogMessage($"Processo iniciado em: {startTime:yyyy-MM-dd HH:mm:ss}");
                LogSeparator();

                int totalFiles = eaFiles.Length;
                progressBar.Maximum = totalFiles;
                progressBar.Value = 0;

                btnStartBacktest.Enabled = false;
                btnStopBacktest.Enabled = true;

                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;

                await Task.Run(() =>
                {
                    int completedBacktests = 0;
                    foreach (string eaFile in eaFiles)
                    {
                        if (token.IsCancellationRequested)
                        {
                            DateTime time = DateTime.Now;
                            LogMessage($"Processo interrompido. {time:yyyy-MM-dd HH:mm:ss}");
                            break;
                        }

                        LogMessage($"Criando arquivo .ini para ==> {Path.GetFileName(eaFile)}");
                        string iniFilePath = Path.Combine(txtOutputFolder.Text,
                            Path.GetFileNameWithoutExtension(eaFile) + ".ini");
                        CreateIniFile(iniFilePath, eaFile);

                        LogMessage($"Iniciando backtest para ==> {Path.GetFileName(eaFile)}");
                        StartBacktest(eaFile, iniFilePath);

                        completedBacktests++;
                        UpdateProgress(completedBacktests);
                        Thread.Sleep(1000);
                        LogSeparator();
                    }

                    DateTime endTime = DateTime.Now;
                    TimeSpan duration = endTime - startTime;

                    LogMessage($"Todos os backtests foram finalizados.");
                    LogMessage($"Total de Experts encontrados: {eaFiles.Length}");
                    LogMessage($"Total de backtests executados: {completedBacktests}");
                    LogMessage($"Processo finalizado em: {endTime:yyyy-MM-dd HH:mm:ss}");
                    LogMessage($"Tempo total decorrido: {duration.Hours}h {duration.Minutes}m {duration.Seconds}s");

                    LogMessage("Movendo resultados...");
                    MoveReportsFolder();
                    UpdateProgress(0);

                    btnStartBacktest.Enabled = true;
                    btnStopBacktest.Enabled = false;
                    btnClearLog.Enabled = true;
                }, token);
            }
            catch (Exception ex)
            {
                LogMessage($"Erro: {ex.Message}");
                btnStartBacktest.Enabled = true;
                btnStopBacktest.Enabled = false;
                btnClearLog.Enabled = true;
            }
        }

        private void StartBacktest(string eaFilePath, string iniFilePath)
        {
            try
            {
                btnClearLog.Enabled = false;
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

                LogMessage($"Backtest iniciado ==> {Path.GetFileName(eaFilePath)}");
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = mt5ExecutablePath,
                    WorkingDirectory = mt5TerminalPath,
                    Arguments = arguments,
                    UseShellExecute = true,
                    WindowStyle = chkMinimizeMt5.Checked ? ProcessWindowStyle.Minimized : ProcessWindowStyle.Normal
                };

                Process? mt5Process = Process.Start(processStartInfo);

                if (mt5Process != null)
                {
                    mt5Process.WaitForExit();
                }

                LogMessage($"Backtest finalizado ==> {Path.GetFileName(eaFilePath)}");
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
                string userPath = txtFolderPath.Text.Trim();

                if (string.IsNullOrEmpty(userPath) || !Directory.Exists(userPath))
                {
                    LogMessage("Erro: O caminho da pasta do terminal não é válido.");
                    return;
                }
                
                Match match = Regex.Match(userPath, @"MetaQuotes\\Terminal\\([A-Z0-9]+)", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    LogMessage("Erro: ID do terminal não encontrado no caminho.");
                    return;
                }

                string terminalId = match.Groups[1].Value;
                string terminalFolder =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MetaQuotes",
                        "Terminal", terminalId);

                if (!Directory.Exists(terminalFolder))
                {
                    LogMessage("Erro: Pasta do terminal não encontrada.");
                    return;
                }
                
                string mql5ExpertsFolder = Path.Combine(terminalFolder, "MQL5", "Experts");

                if (!Directory.Exists(mql5ExpertsFolder))
                {
                    LogMessage("Erro: O diretório MQL5\\Experts não foi encontrado.");
                    return;
                }
                
                string? eaFolderPath = Path.GetDirectoryName(eaFilePath);
                string relativeEaPath =
                    eaFolderPath!.Replace(mql5ExpertsFolder, "").TrimStart(Path.DirectorySeparatorChar);
                
                string reportsFolder = Path.Combine(terminalFolder, "Reports");

                if (!Directory.Exists(reportsFolder))
                {
                    Directory.CreateDirectory(reportsFolder);
                }

                // Monta o conteúdo do arquivo .ini
                StringBuilder iniContent = new StringBuilder();
                iniContent.AppendLine("[Tester]");
                iniContent.AppendLine($"Expert={Path.Combine(relativeEaPath, eaFileName)}");
                iniContent.AppendLine($"Symbol={txtSymbol.Text}");
                iniContent.AppendLine($"Period={txtTimeFrame.Text}");
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
            }
            catch (Exception ex)
            {
                LogMessage($"Erro ao criar o arquivo .ini: {ex.Message}");
                LogSeparator();
            }
        }


        private void BtnStopBacktest_Click(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now;
            _cancellationTokenSource?.Cancel();
            LogSeparator("##################################################################");
            LogMessage($"Processo interrompido {startTime:yyyy-MM-dd HH:mm:ss} , AGUARDE ...");
            LogSeparator("##################################################################");

            btnStartBacktest.Enabled = true;
            btnStopBacktest.Enabled = false;
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
                        _isValisPath = true;
                    }
                    else
                    {
                        _isValisPath = false;
                        MessageBox.Show(
                            $"O arquivo 'terminal64.exe' não foi encontrado no diretório selecionado.\nCaminho: {mt5ExecutablePath}\n Por favor selecione o diretório corretamente!",
                            "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void LogMessage(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(() => { txtLog.AppendText(message + Environment.NewLine); });
            }
            else
            {
                txtLog.AppendText(message + Environment.NewLine);
            }
        }

        private void BtnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
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
                progressBar.Invoke(() => { progressBar.Value = value; });
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
                string selectedPath = txtFolderPath.Text.Trim();

                if (string.IsNullOrEmpty(selectedPath) || !Directory.Exists(selectedPath))
                {
                    LogMessage("Erro: O caminho da pasta do terminal não é válido.");
                    return;
                }

                Match match = Regex.Match(selectedPath, @"MetaQuotes\\Terminal\\([A-Z0-9]+)", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    LogMessage("Erro: ID do terminal não encontrado no caminho.");
                    return;
                }

                string terminalId = match.Groups[1].Value;

                string terminalFolder =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MetaQuotes",
                        "Terminal", terminalId);

                if (!Directory.Exists(terminalFolder))
                {
                    LogMessage("Erro: Pasta do terminal não encontrada.");
                    return;
                }

                string reportsFolder = Path.Combine(terminalFolder, "Reports");
                string destinationFolder = Path.Combine(txtOutputFolder.Text, "Reports");

                if (!Directory.Exists(reportsFolder))
                {
                    LogMessage("Erro: Pasta de relatórios não encontrada.");
                    return;
                }

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                // Mover arquivos
                foreach (string file in Directory.GetFiles(reportsFolder))
                {
                    string destinationFile = Path.Combine(destinationFolder, Path.GetFileName(file));
                    File.Move(file, destinationFile, true);
                }

                LogMessage($"Resultados movidos para: {destinationFolder}");
                LogSeparator();
            }
            catch (Exception ex)
            {
                LogMessage($"Erro ao mover resultados: {ex.Message}");
            }
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }
    }
}