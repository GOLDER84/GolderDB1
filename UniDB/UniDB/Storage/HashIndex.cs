using UniDB.Storage.Interfaces;

namespace UniDB.Storage;

using UniDB.Domain;
using UniDB.Storage;

public class HashIndex : IIndex
{
    private const int Size = 100;
    private LinkedListCollection[] buckets;

    public HashIndex()
    {
        buckets = new LinkedListCollection[Size];
        for (int i = 0; i < Size; i++)
        {
            buckets[i] = new LinkedListCollection();
        }
    }

    private int GetHash(int key)
    {
        return key % Size;
    }

    public string Insert(Student student)
    {
        int index = GetHash(student.Id);
        return buckets[index].insertOne(student);
    }

    public Student Search(int id)
    {
        int index = GetHash(id);
        return buckets[index].FindById(id);
    }

    public string Delete(Student student)
    {
        return Delete(student.Id);
    }
    public string Delete(int id)
    {
        int index = GetHash(id);
        return buckets[index].deleteOne(id);
    }
}