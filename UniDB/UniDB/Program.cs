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
            Console.WriteLine("Enter an option (or 'benchmark' & 'benchmark2' to run benchmark):");
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
            else if (option == "benchmark2")
            {
                var benchmark2 = new Phase2Benchmark();
                benchmark2.RunAll();
            }
            else
            {
                queryParser.Parse(option);
            }
        }
        Console.WriteLine("Have a nice day!");
    }
}