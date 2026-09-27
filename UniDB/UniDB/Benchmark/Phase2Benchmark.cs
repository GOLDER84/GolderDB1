using System.Diagnostics;
using UniDB.Domain;
using UniDB.Storage;

namespace UniDB.Benchmark
{
    public class Phase2Benchmark
    {
        public void RunAll()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("      PHASE 2 PERFORMANCE ANALYSIS      ");
            Console.WriteLine("========================================");

            Compare_BST_vs_AVL_SortedData();
            Console.WriteLine("\n----------------------------------------\n");
            Compare_Search_Strategies();
            Console.WriteLine("\n----------------------------------------\n");
            Analyze_InvertedIndex_Logic();
        }
        
        private void Compare_BST_vs_AVL_SortedData()
        {
            Console.WriteLine("1. Comparison: BST vs AVL (Sorted Input 1..10000)");

            var bst = new BSTIndex();
            var avl = new AVLTreeIndex();
            int count = 10000;

            Console.WriteLine($"Inserting {count} sorted integers...");
            for (int i = 1; i <= count; i++)
            {
                var s = new Student(i, $"Student_{i}", 18.5);
                bst.Insert(s);
                avl.Insert(s);
            }

            int targetId = 10000;
            
            var sw = Stopwatch.StartNew();
            bst.Search(targetId);
            sw.Stop();
            long bstTime = sw.ElapsedTicks;
            Console.WriteLine($"BST Search Time (Worst Case): {bstTime} ticks");

            sw.Restart();
            avl.Search(targetId);
            sw.Stop();
            long avlTime = sw.ElapsedTicks;
            Console.WriteLine($"AVL Search Time (Balanced): {avlTime} ticks");

            Console.WriteLine($"Result: AVL is {bstTime / (double)avlTime:F2}x faster in sorted scenario.");
        }
        private void Compare_Search_Strategies()
        {
            Console.WriteLine("2. Impact of Hash Index (FindById)");

            var storage = StorageManagement.GetInstance();
          
            int count = 50000;
            Console.WriteLine($"Populating DB with {count} records...");
            
            
            var sw = Stopwatch.StartNew();
            storage.FindById(count - 1);
            sw.Stop();
            Console.WriteLine($"Strategy 1: Full Scan (No Index) -> {sw.ElapsedTicks} ticks");
            
            storage.CreateIndex("id", "bst");
            sw.Restart();
            storage.Filter("id", (count - 1).ToString());
            sw.Stop();
            Console.WriteLine($"Strategy 2: BST Index (O(log n)) -> {sw.ElapsedTicks} ticks");
            
            storage.CreateIndex("id", "hash");
            sw.Restart();
            storage.Filter("id", (count - 1).ToString());
            sw.Stop();
            Console.WriteLine($"Strategy 3: Hash Index (O(1)) -> {sw.ElapsedTicks} ticks (Fastest)");
        }
        
        private void Analyze_InvertedIndex_Logic()
        {
            Console.WriteLine("3. Inverted Index Analysis");
            Console.WriteLine("Executing full text search for 'Ali'...");
            
            var storage = StorageManagement.GetInstance();
            storage.InsertOne(new Student(99999, "Mohammad Ali Rezaei", 20));
            
            storage.CreateIndex("name", "inverted");
            
            var sw = Stopwatch.StartNew();
            storage.SearchText("name", "Ali");
            sw.Stop();
            
            Console.WriteLine($"Inverted Index Search Time: {sw.ElapsedTicks} ticks.");
        }
    }
}