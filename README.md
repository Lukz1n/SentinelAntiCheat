# 🛡️ Protótipo Anti-Cheat - Sentinel

![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-Windows-blue?style=for-the-badge&logo=windows)
![Status](https://img.shields.io/badge/Status-Educational-orange?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

> Projeto educacional que simula o funcionamento de um sistema **anti-cheat em nível kernel**, inspirado no **Riot Vanguard**, mas implementado inteiramente em **C# / .NET** e rodando em **modo usuário**.  
O objetivo não é criar um anti-cheat funcional para jogos reais, mas explorar os **conceitos, design e desafios técnicos** por trás dessas soluções.

---

## 📖 Índice
1. [Introdução](#-introdução)  
2. [Design Conceitual](#-design-conceitual)  
3. [Implementação](#-implementação)  
   - [SentinelUnified](#sentinelunified)  
   - [SimulatedGame](#simulatedgame)  
4. [Limitações](#-limitações)  
5. [Conclusão](#-conclusão)  
6. [Referências](#-referências)  

---

## 📌 Introdução
O **Sentinel** é um protótipo de anti-cheat simulado.  

Ele foi inspirado no **Riot Vanguard**, conhecido por rodar em nível **kernel**, mas aqui o funcionamento é apenas uma **simulação em ambiente controlado** e seguro, sem acesso a privilégios de kernel.  

Funcionalidades principais simuladas:
- 🔍 Monitoramento de processos  
- 📂 Verificação de integridade de arquivos  
- 🛠️ Detecção de *hooking* (simulada)  

---

## 🏗️ Design Conceitual
A arquitetura do Sentinel é dividida em dois principais componentes:

### 🔹 **SentinelUnified**
Simula o papel do **serviço VGC (vgc.exe)** e do **driver VGK.SYS (vgk.sys)**:
- Atua como o "serviço" anti-cheat em **modo usuário**  
- Contém internamente a lógica que simula operações de **driver kernel**  

### 🔹 **SimulatedGame**
Um programa simples em **console C#** que representa o "jogo" protegido pelo Sentinel.  
É alvo das verificações e pode simular modificações para testar o sistema.

### 🔄 Fluxo de Interação
1. `SentinelUnified` é iniciado e instancia o `SimulatedKernel`  
2. Inicia o `SimulatedGame` como processo separado  
3. Registra o jogo para monitoramento e integridade  
4. Executa verificações contínuas:
   - Status do processo  
   - Integridade do processo  
   - Integridade do arquivo  
   - Detecção de hooking  
5. Caso seja detectada anomalia → **encerra o jogo simulado**  

---

## 💻 Implementação

### 🛡️ SentinelUnified
- **Inicialização** → instancia `SimulatedKernel`, define jogo alvo  
- **Registro de Monitoramento** → monitora o processo do jogo  
- **Registro de Integridade de Arquivo** → calcula e registra hash SHA256  
- **Loop Contínuo** → executa verificações periódicas  
- **Reação a Ameaças** → encerra o jogo caso haja falha de integridade ou hooking  
- **Simulação de Ameaças** → pode modificar `game_data.txt` aleatoriamente para teste  

### 🎮 SimulatedGame
- Aplicação console minimalista  
- Simula um jogo em execução  
- Exibe mensagens no console  
- Pode ser encerrado pelo `SentinelUnified`  

---

## ⚠️ Limitações
É importante reforçar que este projeto é **puramente educacional** e possui várias limitações:

- 🚫 **Não roda em nível kernel real**  
- 🔐 **Verificações de hooking e integridade são extremamente simplificadas**  
- 🧑‍💻 **Não é seguro contra bypass**  
- 🐢 **Sem otimização de performance**  
- 💻 **Compatibilidade apenas com C# e .NET**  

---

## ✅ Conclusão
O **Sentinel** é uma **ferramenta de estudo** para compreender conceitos por trás de anti-cheats de nível kernel.  

Ele mostra como:
- Monitorar processos  
- Validar integridade de arquivos  
- Simular verificações de hooking  

Apesar de **não ser funcional em cenários reais**, este protótipo ajuda a visualizar a **complexidade e os desafios** enfrentados por desenvolvedores de sistemas anti-cheat.

---

## 📚 Referências
- [Riot Games - Vanguard](https://playvalorant.com/pt-br/news/dev/valorant-anti-cheat-the-vanguard/)  
- [Microsoft Docs - Process Class](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process?view=net-8.0)  
- [Microsoft Docs - SHA256 Class](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.sha256?view=net-8.0)  

---

🔧 Desenvolvido em **C# .NET** | 📘 Uso **educacional apenas**
