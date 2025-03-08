# AutoBT_MT5 - Automação de Backtest para o MetaTrader 5

## Descrição
AutoBT_MT5 é uma ferramenta desenvolvida para automatizar e facilitar a execução de backtests no MetaTrader 5. O software permite a execução de testes em massa, otimizando o processo de avaliação de estratégias de trading.

## Atualizações
✅ 03/03/2025 - Primeira versão. </br>
✅ 06/03/2025 - **Bug corrigido** onde, se o usuário tivesse mais de um MT5 instalado na máquina, o programa não realizava o backtest corretamente.

## Funcionalidades
- **Seleção de EAs:** Permite escolher a pasta contendo os Expert Advisors (EAs) que serão testados.
- **Seleção de pasta para resultados:** Define o local onde os resultados .html dos backtests serão armazenados.
- **Configuração de ativos e timeframes:** Permite definir o ativo e o período de tempo para os testes.
- **Configuração de período de testes:** Define a data de início e fim dos backtests.
- **Definição de depósito inicial e moeda:** Ajusta o capital inicial e a moeda utilizada nos testes.
- **Caminho do MT5:** Permite definir o caminho da pasta onde está o **.exe do MT5**, normalmente em: **C:\Program Files\MetaTrader 5 Terminal**, arquivo: **terminal64.exe**.
- **Opções adicionais:** Possui opções para minimizar o MetaTrader 5 durante a execução dos backtests.
- **Execução de backtest em massa:** Testa automaticamente todos os EAs presentes na pasta selecionada.
- **Gerenciamento de logs:** Registra informações sobre a execução dos backtests e permite limpar os logs facilmente.
- **Janela "Sobre":** Exibe informações sobre o software, incluindo links para documentação e licença.

## Como Usar
1. Escolha a pasta contendo os **Expert Advisors**. Essa pasta deve ser **obrigatoriamente sua pasta de perfil do MT5**, normalmente no caminho: **C:\Users\seu usuário\AppData\Roaming\MetaQuotes\Terminal\seu ID\MQL5\Experts**
   - Exemplo: **C:\Users\Cleber\AppData\Roaming\MetaQuotes\Terminal\FB9A56D617EDDDFE29EE54EBEFFE96C1\MQL5\Experts**
   - **Atenção: O MT5 não consegue executar backtests com EAs fora da pasta do perfil. Caso seus EAs estejam em outra pasta, o backtest não será realizado.**
2. Defina a pasta de destino para os resultados.
3. Configure o ativo, timeframe e período do backtest.
4. Ajuste o valor do depósito inicial e a moeda.
5. Caminho da pasta onde está o **.exe do MT5**, normalmente em: **C:\Program Files\MetaTrader 5 Terminal**, arquivo: **terminal64.exe.**
   - **Atenção: O backtest só inicia se o arquivo terminal64.exe for encontrado.**
6. Opcionalmente, ative a opção de minimizar o MT5.
7. Clique em "Iniciar Backtest" para executar os testes.
8. Acompanhe os logs e utilize "Limpar Log" se necessário.
9. Acesse "Sobre" para informações adicionais sobre a ferramenta.

**"Os arquivos .ini para cada backtest serão criados na pasta de resultados. Após finalizar o processo, você pode deletá-los."**

## Licença
Este projeto está licenciado sob a **MIT License**.

**Copyright (c) 2025 Cleberton Francisco**