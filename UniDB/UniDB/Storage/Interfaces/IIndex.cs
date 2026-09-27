using UniDB.Domain;

namespace UniDB.Storage.Interfaces;

public interface IIndex
{
    public string Insert(Student student);
    public string Delete(Student student);
}