using System;

namespace Evento.Cli
{
    interface IGreeter
    {
        void Greet(string name) => Console.WriteLine($"Hello, {name ?? "anonymous"}");
    }
}
