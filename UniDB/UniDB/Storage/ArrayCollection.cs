using System.Collections;
using UniDB.Domain;
using UniDB.Storage.Interfaces;
using ICollection = UniDB.Storage.Interfaces.ICollection;

namespace UniDB.Storage;

public class ArrayCollection : ICollection
{
    private Student[] _students;
    private int _count;

    public ArrayCollection(int size = 10000)
    {
        _students = new Student[size];
        _count = 0;
    }

    public string insertOne(Student student)
    {
        if (_count >= _students.Length)
        {
            return "Collection is full";
        }
        else
        {
            _students[_count] = student;
            _count++;
            return "Student inserted successfully";
        }
    }

    public string deleteOne(int id)
    {
        for (int i = 0; i < _students.Length; i++)
        {
            if (_students[i] != null && _students[i].Id == id)
            {
                for (int j = i; j < _count - 1; j++)
                {
                    _students[j] = _students[j + 1];
                }

                _students[_count - 1] = null;
                _count--;
                return "Student deleted successfully";
            }
        }
        return "Student not found";
    }

    public Student? FindById(int id)
    {
        for (int i = 0; i < _students.Length; i++)
        {
            if (_students[i] != null && _students[i].Id == id)
            {
                return _students[i];
            }
        }

        return null;
    }

    public List<Student> FindAll()
    {
        List<Student> result = new List<Student>();
        for (int i = 0; i < _count; i++)
        {
            if (_students[i] != null)
            {
                result.Add(_students[i]);
            }
        }

        return result;
    }

    public int Count()
    {
        return _count;
    }

    public double Sum(string field)
    {
        double sum = 0;
        if (field == "gpa")
        {
            for (int i = 0; i < _count; i++)
            {
                if (_students[i] != null)
                {
                    sum += _students[i].Gpa;
                }
            }
        }
        else if (field == "id")
        {
            for (int i = 0; i < _count; i++)
            {
                if (_students[i] != null)
                {
                    sum += _students[i].Id;
                }
            }
        }
        return sum;
    }

    public double Average(string field)
    {
        if (_count == 0) return 0;
        if (field == "gpa")
        {
            return Sum(field) / _count;
        }
        else if (field == "id")
        {
            return Sum(field) / _count;
        }
        return 0;
    }

    public List<Student> Filter(string field, string value)
    {
        List<Student> result = new List<Student>();
        if (field == "gpa")
        {
            double gpa = double.Parse(value);
            for (int i = 0; i < _count; i++)
            {
                if (_students[i] != null && _students[i].Gpa == gpa)
                {
                    result.Add(_students[i]);
                }
            }
        }
        else if (field == "name")
        {
            for (int i = 0; i < _count; i++)
            {
                if (_students[i] != null && _students[i].Name == value)
                {
                    result.Add(_students[i]);
                }
            }
        }

        return result;
    }
}