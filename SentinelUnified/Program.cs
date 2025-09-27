using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Security.Cryptography;

namespace SentinelUnified
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"SentinelUnified: BaseDirectory: {AppDomain.CurrentDomain.BaseDirectory}");
            Console.WriteLine("SentinelUnified: Iniciando sistema anti-cheat simulado...");
            var simulatedKernel = new SimulatedKernel();

            // Simular um processo de jogo
            string gameProcessName = "SimulatedGame";
            string gameExecutablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SimulatedGameOutput", gameProcessName);



            // Adicionar o processo do jogo para monitoramento
            simulatedKernel.HandleIoControl(IoControlCode.RegisterProcessForMonitoring, gameProcessName);

            // Obter o caminho do executável do jogo simulado (assumindo que já foi construído)
            string actualGameExecutablePath = GetSimulatedGameExecutablePath(gameProcessName);
            gameExecutablePath = actualGameExecutablePath;

            // Iniciar o processo do jogo simulado
            Process gameProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = gameExecutablePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            try
            {
                gameProcess.Start();
                Console.WriteLine($"SentinelUnified: Processo simulado \'{gameProcessName}\' iniciado com PID: {gameProcess.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SentinelUnified: Erro ao iniciar o jogo simulado: {ex.Message}. Encerrando.");
                return;
            }

            // Registrar a integridade do arquivo do jogo (hash simulado) com o caminho real
            string gameFileHash = CalculateFileHash(gameExecutablePath);
            simulatedKernel.HandleIoControl(IoControlCode.RegisterFileIntegrity, gameExecutablePath, gameFileHash);

            Console.WriteLine($"SentinelUnified: Monitorando processo: {gameProcessName}");
            Console.WriteLine($"SentinelUnified: Verificando integridade do arquivo: {gameExecutablePath}");

            // Loop de monitoramento
            while (true)
            {
                Console.WriteLine("\nSentinelUnified: Executando ciclo de monitoramento...");

                // 1. Verificar se o processo do jogo está rodando
                if (!(bool)simulatedKernel.HandleIoControl(IoControlCode.GetMonitoredProcessStatus, gameProcessName))
                {
                    Console.WriteLine($"SentinelUnified: ALERTA! Processo \'{gameProcessName}\' não está rodando. Possível encerramento inesperado ou manipulação.");
                    break;
                }

                // 2. Verificar integridade do processo (simulado)
                if (!(bool)simulatedKernel.HandleIoControl(IoControlCode.CheckProcessIntegrity, gameProcessName))
                {
                    Console.WriteLine($"SentinelUnified: ALERTA! Integridade do processo \'{gameProcessName}\' comprometida. Encerrando jogo.");
                    simulatedKernel.HandleIoControl(IoControlCode.TerminateProcess, gameProcessName);
                    break;
                }

                // 3. Verificar integridade do arquivo do jogo
                if (!(bool)simulatedKernel.HandleIoControl(IoControlCode.CheckFileIntegrity, gameExecutablePath))
                {
                    Console.WriteLine($"SentinelUnified: ALERTA! Integridade do arquivo \'{gameExecutablePath}\' comprometida. Encerrando jogo.");
                    simulatedKernel.HandleIoControl(IoControlCode.TerminateProcess, gameProcessName);
                    break;
                }

                // 4. Simular detecção de hooking
                if ((bool)simulatedKernel.HandleIoControl(IoControlCode.DetectHooking, gameProcessName))
                {
                    Console.WriteLine($"SentinelUnified: ALERTA! Hooking detectado no processo \'{gameProcessName}\'! Encerrando jogo.");
                    simulatedKernel.HandleIoControl(IoControlCode.TerminateProcess, gameProcessName);
                    break;
                }

                // Simular uma ameaça (ex: modificar o arquivo do jogo)
                if (new Random().Next(0, 10) == 0) // 10% de chance de modificar o arquivo
                {
                    Console.WriteLine("SentinelUnified: SIMULANDO AMEAÇA: Modificando arquivo do jogo...");
                    string gameDataPath = Path.Combine(Path.GetDirectoryName(gameExecutablePath) ?? "", "game_data.txt");
                    // Certifique-se de que o arquivo game_data.txt existe no diretório de publicação
                    if (!File.Exists(gameDataPath))
                    {
                        File.WriteAllText(gameDataPath, "Initial game data.");
                    }
                    File.AppendAllText(gameDataPath, "\nMalicious data injected!");
                }

                Thread.Sleep(5000); // Espera 5 segundos antes do próximo ciclo
            }

            Console.WriteLine("SentinelUnified: Sistema anti-cheat simulado finalizado.");
        }

        static string GetSimulatedGameExecutablePath(string gameProcessName)
        {
            string simulatedGameProjectDir = Path.Combine("/home/ubuntu/Sentinel", "SimulatedGame");
            string outputDir = Path.Combine(simulatedGameProjectDir, "publish");
            string actualGameExecutablePath = Path.Combine(outputDir, gameProcessName);
            return actualGameExecutablePath;
        }

        static string CalculateFileHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
