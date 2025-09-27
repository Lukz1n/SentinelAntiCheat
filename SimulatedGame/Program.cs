using System;
using System.Threading;

namespace SimulatedGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Simulated Game: Jogo em execução...");
            while (true)
            {
                Thread.Sleep(1000); // Simula o jogo rodando
            }
        }
    }
}
