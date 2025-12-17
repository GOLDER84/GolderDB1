using UniDB.Domain;
using UniDB.Storage.Interfaces;

namespace UniDB.Storage;

public class LinkedListCollection : ICollection
{
    private LinkedList<Student> _students;

    public LinkedListCollection()
    {
        _students = new LinkedList<Student>();
    }

    public void insertOne(Student student)
    {
        _students.AddLast(student);
    }

    public void deleteOne(int id)
    {
        var student = FindById(id);
        if (student != null)
        {
            _students.Remove(student);
        }
    }

    public Student? FindById(int id)
    {
        foreach (var student in _students)
        {
            if (student.Id == id) return student;
        }

        return null;
    }

    public List<Student> FindAll()
    {
        return _students.ToList();
    }

    public int Count()
    {
        return _students.Count;
    }

    public double Sum(string field)
    {
        double sum = 0;
        foreach (var student in _students)
        {
            if (field == "gpa")
            {
                sum += student.Gpa;
            }
        }

        return sum;
    }

    public double Average(string field)
    {
        if (_students.Count == 0) return 0;
        return Sum(field) / _students.Count;
    }

    public List<Student> Filter(string field, string value)
    {
        List<Student> result = new List<Student>();
        if (field == "gpa")
        {
            double gpaValue = double.Parse(value);
            foreach (var student in _students)
            {
                if (student.Gpa.Equals(gpaValue)) result.Add(student);
            }
        }
        else if (field == "name")
        {
            foreach (var student in _students)
            {
                if (student.Name.Equals(value)) result.Add(student);
            }
        }

        return result;
    }
}