using UniDB.Benchmark;
using UniDB.Parser;

public class Program
{
    public static void Main(string[] args)
    {
        QueryParser queryParser = new QueryParser();
        Console.WriteLine("Welcome to UniDB!");
        while (true)
        {
            Console.WriteLine("Enter an option (or 'benchmark' to run benchmark):");
            string option = Console.ReadLine();
            if (option == "exit")
            {
                break;
            }
            else if (option == "benchmark")
            {
                var benchmark = new BenchmarkRunner(queryParser);
                benchmark.RunFullBenchmark();
            }
            else
            {
                queryParser.Parse(option);
            }
        }
        Console.WriteLine("Have a nice day!");
    }
}