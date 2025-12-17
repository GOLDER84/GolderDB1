using UniDB.Domain;
using UniDB.Engine;

namespace UniDB.Storage;

public class StorageManagement
{
    private static StorageManagement? _instance;
    private static readonly object _lock = new object();
    
    private ArrayCollection _arrayCollection = new ArrayCollection();
    private LinkedListCollection _linkedListCollection = new LinkedListCollection();
    private StorageManagement() { }  // Constructor خصوصی

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
    public bool InsertOne(Student student)
    {
        string first = _arrayCollection.insertOne(student);
        if (first.Equals("Student inserted successfully"))
        {
            string last = _linkedListCollection.insertOne(student);
            if (last.Equals("Student inserted successfully"))
            {
                return true;
            }
        }

        return false;
    }
    public bool DeleteOne(int id)
    {
        string first = _arrayCollection.deleteOne(id);
        if (first.Equals("Student deleted successfully"))
        {
            string last = _linkedListCollection.deleteOne(id);
            if (last.Equals("Student deleted successfully"))
            {
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
    public List<Student> Filter(string field , string value)
    {
        // return _arrayCollection.Filter(field , value);
        return _linkedListCollection.Filter(field , value);
    }
    
}