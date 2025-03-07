# AutoBT_MT5 - Automacao de Backtest para o MetaTrader 5

## Descricão
AutoBT_MT5 e uma ferramenta desenvolvida para automatizar e facilitar a execucao de backtests no MetaTrader 5. O software permite a execucão de testes em massa, otimizando o processo de avaliacao de estrategias de trading.

## Atualizações
✅ 03/03/2025 - Primeira versão.
✅ 06/03/2025 - Bug corrigido onde se usuario tiver mais de um mt5 instalado na maquina o programa não realizava o back teste corretamente.

## Funcionalidades
- **Selecao de EAs:** Permite escolher a pasta contendo os Expert Advisors (EAs) que serão testados.
- **Selecao de pasta para resultados:** Define o local onde os resultados .html dos backtests serão armazenados.
- **Configuracao de ativos e timeframes:** Permite definir o ativo e o periodo de tempo para os testes.
- **Configuracao de periodo de testes:** Define a data de inicio e fim dos backtests.
- **Definicao de deposito inicial e moeda:** Ajusta o capital inicial e a moeda utilizada nos testes.
- **Caminho MT5** Passar o caminho da pasta onde está o **.exe do MT5**, normalmente em: **C:\Program Files\MetaTrader 5 Terminal** aquivo: **terminal64.exe**.
- **Opcoes adicionais:**: Possui opcoes para minimizar o MetaTrader 5 durante a execucao dos backtests.
- **Execucao de backtest em massa:** Testa automaticamente todos os EAs presentes na pasta selecionada.
- **Gerenciamento de logs:** Registra informacoes sobre a execucao dos backtests e permite limpar os logs facilmente.
- **Janela "Sobre":** Exibe informacoes sobre o software, incluindo links para documentação e licença.

## Como Usar
1. Escolha a pasta contendo os **Expert Advisors**, essa pasta deve ser **obrigatoriamente sua pasta de perfil do MT5**, normalmente no caminho: **C:\Users\seu usuário\AppData\Roaming\MetaQuotes\Terminal\seu ID\MQL5\Experts**
   - Exemplo: **C:\Users\Cleber\AppData\Roaming\MetaQuotes\Terminal\FB9A56D617EDDDFE29EE54EBEFFE96C1\MQL5\Experts**
   - **Atenção: O MT5 não consegue executar back testes com Eas fora da pasta do perfil. Caso seus Eas estejam em outra pasta o backtest não será realizado.**
3. Defina a pasta de destino para os resultados.
4. Configure o ativo, timeframe e periodo do backtest.
5. Ajuste o valor do deposito inicial e a moeda.
6. Caminho da pasta onde está o **.exe do MT5**, normalmente em: **C:\Program Files\MetaTrader 5 Terminal** arquivo: **terminal64.exe.**
   - **Atenção: O backtest só incia se o arquivo terminal64.exe for encontrado.**
8. Opcionalmente, ative a opcao de minimizar o MT5.
9. Clique em "Iniciar Backtest" para executar os testes.
10. Acompanhe os logs e utilize "Limpar Log" se necessario.
11. Acesse "Sobre" para informacoes adicionais sobre a ferramenta.

**"Os arquivos .ini para cada backtest serão criados na pasta de resultados. Após finalizar o processo, você pode deletá-los."**

## Licença
Este projeto esta licenciado sob a **MIT License**.

**Copyright (c) 2025 Cleberton Francisco**

