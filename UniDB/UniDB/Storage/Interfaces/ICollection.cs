using UniDB.Domain;

namespace UniDB.Storage.Interfaces;

public interface ICollection
{
    public void insertOne(Student student);
    public void deleteOne(int id);
    public Student? FindById(int id);
    public List<Student> FindAll();

    public int Count();
    public double Sum(string field);
    public double Average(string field);
    
    public List<Student> Filter(string field , double value);
}