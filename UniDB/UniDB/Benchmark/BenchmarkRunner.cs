using System.Diagnostics;

namespace UniDB.Benchmark;

public class BenchmarkRunner
{
    private readonly Parser.QueryParser _queryParser;
    private readonly Dictionary<string, long> _results;

    public BenchmarkRunner(Parser.QueryParser queryParser)
    {
        _queryParser = queryParser;
        _results = new Dictionary<string, long>();
    }

    public void RunFullBenchmark()
    {
        Console.WriteLine("\n========== BENCHMARK START ==========\n");
        
        string path = @"E:\University\Term3\Data Structure\AllProject\UniDB\project01-unidb-GOLDER84\UniDB\UniDB\data_50k.csv";
        string csvPath = "data_50k.csv";
        DataGenerator.GenerateCSV(path, 50000);
        
        var sw = Stopwatch.StartNew();
        string query = "db.s.import(\"" + csvPath + "\")";
        _queryParser.Parse(query);
        sw.Stop();
        _results["Import 50,000 records"] = sw.ElapsedMilliseconds;
        // Console.WriteLine($"Import: {sw.ElapsedMilliseconds} ms\n");
        
        sw.Restart();
        for (int i = 1; i <= 500; i++)
        {
            _queryParser.Parse($"db.students.deleteOne({{_id: {i}}})");
        }
        sw.Stop();
        _results["Delete 500 from start"] = sw.ElapsedMilliseconds;
        // Console.WriteLine($"Delete from start: {sw.ElapsedMilliseconds} ms\n");
        
        sw.Restart();
        for (int i = 49501; i <= 50000; i++)
        {
            _queryParser.Parse($"db.students.deleteOne({{_id: {i}}})");
        }
        sw.Stop();
        _results["Delete 500 from end"] = sw.ElapsedMilliseconds;
        // Console.WriteLine($"Delete from end: {sw.ElapsedMilliseconds} ms\n");
        
        var random = new Random();
        var randomIds = Enumerable.Range(501, 49000)
            .OrderBy(_ => random.Next())
            .Take(500)
            .ToList();
        
        sw.Restart();
        foreach (int id in randomIds)
        {
            _queryParser.Parse($"db.students.findById({{_id: {id}}})");
        }
        sw.Stop();
        _results["Search 500 random"] = sw.ElapsedMilliseconds;
        // Console.WriteLine($"Random search: {sw.ElapsedMilliseconds} ms\n");
        
        PrintResult();
    }

    private void PrintResult()
    {
        Console.WriteLine("\n========== BENCHMARK RESULTS (ms) ==========\n");

        foreach (var result in _results)
        {
            Console.WriteLine($"{result.Key} | {result.Value} ms");
        }

        Console.WriteLine("\n=============================================\n");
    }
}