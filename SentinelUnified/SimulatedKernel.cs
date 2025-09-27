using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SentinelUnified
{
    public enum IoControlCode : uint
    {
        RegisterProcessForMonitoring = 0x80002000,
        CheckProcessIntegrity = 0x80002004,
        RegisterFileIntegrity = 0x80002008,
        CheckFileIntegrity = 0x8000200C,
        DetectHooking = 0x80002010,
        TerminateProcess = 0x80002014,
        GetMonitoredProcessStatus = 0x80002018
    }

    public class SimulatedKernel
    {
        private readonly List<string> _monitoredProcesses = new List<string>();
        private readonly Dictionary<string, string> _fileHashes = new Dictionary<string, string>();

        public SimulatedKernel()
        {
            Console.WriteLine("SimulatedKernel: Inicializado.");
        }

        public object HandleIoControl(IoControlCode controlCode, params object[] args)
        {
            switch (controlCode)
            {
                case IoControlCode.RegisterProcessForMonitoring:
                    {
                        string processName = (string)args[0];
                        if (!_monitoredProcesses.Contains(processName))
                        {
                            _monitoredProcesses.Add(processName);
                            Console.WriteLine($"SimulatedKernel: Processo \'{processName}\' adicionado para monitoramento.");
                        }
                        return true;
                    }
                case IoControlCode.RegisterFileIntegrity:
                    {
                        string filePath = (string)args[0];
                        string expectedHash = (string)args[1];
                        _fileHashes[filePath] = expectedHash;
                        Console.WriteLine($"SimulatedKernel: Integridade de arquivo \'{filePath}\' registrada com hash \'{expectedHash}\'.");
                        return true;
                    }
                case IoControlCode.CheckProcessIntegrity:
                    {
                        string processName = (string)args[0];
                        Console.WriteLine($"SimulatedKernel: Verificando integridade do processo \'{processName}\' (simulado)...");
                        // Simulação: 10% de chance de falha na integridade do processo
                        if (new Random().Next(0, 10) == 0)
                        {
                            Console.WriteLine($"SimulatedKernel: ALERTA! Integridade do processo \'{processName}\' comprometida (simulado).");
                            return false;
                        }
                        return true;
                    }
                case IoControlCode.CheckFileIntegrity:
                    {
                        string filePath = (string)args[0];
                        if (!_fileHashes.ContainsKey(filePath))
                        {
                            Console.WriteLine($"SimulatedKernel: Arquivo \'{filePath}\' não registrado para verificação de integridade.");
                            return true; // Não registrado, assume-se OK
                        }

                        try
                        {
                            using (var sha256 = SHA256.Create())
                            {
                                using (var stream = System.IO.File.OpenRead(filePath))
                                {
                                    var hash = sha256.ComputeHash(stream);
                                    var currentHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                                    bool isIntegrityOk = currentHash == _fileHashes[filePath];
                                    Console.WriteLine($"SimulatedKernel: Integridade de arquivo \'{filePath}\' verificada. Hash atual: \'{currentHash}\', Esperado: \'{_fileHashes[filePath]}\'. OK: {isIntegrityOk}");
                                    return isIntegrityOk;
                                }
                            }
                        }
                        catch (System.IO.FileNotFoundException)
                        {
                            Console.WriteLine($"SimulatedKernel: Arquivo \'{filePath}\' não encontrado para verificação de integridade.");
                            return false; // Arquivo ausente é uma falha de integridade
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"SimulatedKernel: Erro ao verificar integridade do arquivo \'{filePath}\': {ex.Message}");
                            return false;
                        }
                    }
                case IoControlCode.DetectHooking:
                    {
                        string processName = (string)args[0];
                        Console.WriteLine($"SimulatedKernel: Detectando hooking no processo \'{processName}\' (simulado)...");
                        // Simulação: 5% de chance de detectar hooking
                        if (new Random().Next(0, 20) == 0)
                        {
                            Console.WriteLine($"SimulatedKernel: ALERTA! Hooking detectado no processo \'{processName}\' (simulado).");
                            return true;
                        }
                        return false;
                    }
                case IoControlCode.TerminateProcess:
                    {
                        string processName = (string)args[0];
                        var processes = Process.GetProcessesByName(processName);
                        foreach (var p in processes)
                        {
                            try
                            {
                                p.Kill();
                                Console.WriteLine($"SimulatedKernel: Processo \'{processName}\' (PID: {p.Id}) terminado.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"SimulatedKernel: Erro ao terminar processo \'{processName}\' (PID: {p.Id}): {ex.Message}");
                            }
                        }
                        return true;
                    }
                case IoControlCode.GetMonitoredProcessStatus:
                    {
                        string processName = (string)args[0];
                        return Process.GetProcessesByName(processName).Any();
                    }
                default:
                    Console.WriteLine($"SimulatedKernel: IOCTL não reconhecido: {controlCode}");
                    return null;
            }
        }
    }
}