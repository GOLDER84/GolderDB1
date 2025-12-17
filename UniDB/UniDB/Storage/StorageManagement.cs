using UniDB.Domain;
using UniDB.Engine;

namespace UniDB.Storage;

public class StorageManagement
{
    
    private ArrayCollection _arrayCollection = new ArrayCollection();
    private LinkedListCollection _linkedListCollection = new LinkedListCollection();
    public static StorageManagement GetInstance()
    {
        return new StorageManagement();
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
}