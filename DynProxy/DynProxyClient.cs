using System;
using System.Threading.Tasks;

namespace DynProxy
{
    class DynProxyClient
    {
        static void Main(string[] args)
        {
            var decoratedCalculator = LoggingDecorator<ICalculator>.Create(new Calculator());
            decoratedCalculator.Add(3, 5);
            decoratedCalculator.Add(2, 4);
            decoratedCalculator.Add(1, 3);

            decoratedCalculator.AddSpecial(1, 2, 3);
            decoratedCalculator.AddSpecial(4, 5, 6);
            decoratedCalculator.AddSpecial(7, 8, 9);
            Console.ReadKey();

            Console.WriteLine("\n\n");

            var decorated = LoggingAdvice<IMyClass>.Create(
                            new MyClass(),
                            s => Console.WriteLine("Info:\n" + s),
                            s => Console.WriteLine("Error:\n" + s),
                            o => o?.ToString(),
                            TaskScheduler.Default);

            var msg = System.Text.Json.JsonSerializer.Serialize("Hello World!", typeof(string));
            var length = decorated.MyMethod(msg);
            Console.ReadKey();
        }   //  Main()
    }   //  class DynProxyClient
}   //  namespace DynProxy
