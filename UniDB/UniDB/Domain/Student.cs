namespace UniDB.Domain;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Gpa { get; set; }

    public Student(int id, string name, double gpa)
    {
        Id = id;
        Name = name;
        Gpa = gpa;
    }
}