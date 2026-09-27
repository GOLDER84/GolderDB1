using UniDB.Domain;
using UniDB.Engine;
using UniDB.Storage.Interfaces;

namespace UniDB.Storage;

public class StorageManagement
{
    private static StorageManagement? _instance;
    private static readonly object _lock = new object();

    private ArrayCollection _arrayCollection = new ArrayCollection();
    private LinkedListCollection _linkedListCollection = new LinkedListCollection();
    private Dictionary<string, IIndex> _activeIndexes = new Dictionary<string, IIndex>();

    private StorageManagement()
    {
    }

    public static StorageManagement GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new StorageManagement();
                }
            }
        }

        return _instance;
    }

    public string CreateIndex(string field, string type)
    {
        string key = $"{field.ToLower()}_{type.ToLower()}";

        if (_activeIndexes.ContainsKey(key))
        {
            return $"Index '{type}' on field '{field}' already exists.";
        }

        IIndex newIndex;

        switch (type.ToLower())
        {
            case "bst":
                newIndex = new BSTIndex();
                break;
            case "avl":
                newIndex = new AVLTreeIndex();
                break;
            case "hash":
                newIndex = new HashIndex();
                break;
            case "inverted":
                newIndex = new InvertedIndex();
                break;
            default:
                return "Unknown index type.";
        }

        foreach (var student in _linkedListCollection.FindAll())
        {
            newIndex.Insert(student);
        }

        _activeIndexes[key] = newIndex;
        return $"Index '{type}' created on '{field}' successfully.";
    }

    public bool InsertOne(Student student)
    {
        string first = _arrayCollection.insertOne(student);
        if (first.Equals("Student inserted successfully"))
        {
            string last = _linkedListCollection.insertOne(student);
            if (last.Equals("Student inserted successfully"))
            {
                foreach (var index in _activeIndexes.Values)
                {
                    index.Insert(student);
                }

                return true;
            }
        }

        return false;
    }

    public bool DeleteOne(int id)
    {
        var student = FindById(id);
        if (student == null)
        {
            return false;
        }

        string first = _arrayCollection.deleteOne(id);
        if (first.Equals("Student deleted successfully"))
        {
            string last = _linkedListCollection.deleteOne(id);
            if (last.Equals("Student deleted successfully"))
            {
                foreach (var index in _activeIndexes.Values)
                {
                    index.Delete(student);
                }

                return true;
            }
        }

        return false;
    }

    public Student? FindById(int id)
    {
        // return _arrayCollection.FindById(id);
        return _linkedListCollection.FindById(id);
    }

    public List<Student> FindAll()
    {
        // return _arrayCollection.FindAll();
        return _linkedListCollection.FindAll();
    }

    public int Count()
    {
        // return _arrayCollection.Count();
        return _linkedListCollection.Count();
    }

    public double Sum(string field)
    {
        // return _arrayCollection.Sum(field);
        return _linkedListCollection.Sum(field);
    }

    public double Average(string field)
    {
        // return _arrayCollection.Average(field);
        return _linkedListCollection.Average(field);
    }

    public List<Student> Filter(string field, string value)
    {
        field = field.ToLower();

        if (field == "id" && _activeIndexes.TryGetValue("id_hash", out IIndex hashIndex))
        {
            Console.WriteLine("-> Query Plan: Using Hash Index");
            var result = ((HashIndex)hashIndex).Search(int.Parse(value));
            return result != null ? new List<Student> { result } : new List<Student>();
        }

        if (field == "gpa" && _activeIndexes.TryGetValue($"{field}_avl", out IIndex avlIndex))
        {
            Console.WriteLine("-> Query Plan: Using AVL Tree Index");
            var result = ((AVLTreeIndex)avlIndex).Search(int.Parse(value));
            return result != null ? new List<Student> { result } : new List<Student>();
        }

        if (field == "name" && _activeIndexes.TryGetValue($"{field}_bst", out IIndex bstIndex))
        {
            Console.WriteLine("-> Query Plan: Using BST Index");
            var result = ((BSTIndex)bstIndex).Search(int.Parse(value));
            return result != null ? new List<Student> { result } : new List<Student>();
        }

        Console.WriteLine("-> Query Plan: Full Scan");
        return _linkedListCollection.Filter(field, value);
    }

    public List<Student> SearchText(string field, string token)
    {
        if (field.ToLower() == "name" && _activeIndexes.TryGetValue("name_inverted", out IIndex index))
        {
            Console.WriteLine("-> Query Plan: Using Inverted Index");
            var ids = ((InvertedIndex)index).Search(token);

            return ids.Select(id => FindById(id)).Where(s => s != null).ToList();
        }

        Console.WriteLine("-> Query Plan: Full Scan (Contains)");
        return _linkedListCollection.FindAll().Where(s => s.Name.Contains(token)).ToList();
    }
}