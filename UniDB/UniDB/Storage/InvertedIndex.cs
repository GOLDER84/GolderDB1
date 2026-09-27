using UniDB.Storage.Interfaces;

namespace UniDB.Storage;

using UniDB.Domain;

public class InvertedIndex : IIndex
{
    private Dictionary<string, List<int>> index;

    public InvertedIndex()
    {
        index = new Dictionary<string, List<int>>();
    }

    public string Insert(Student student)
    {
        if (string.IsNullOrEmpty(student.Name))
        {
            return "Insertion failed: Student name is empty";
        }

        int tokensIndexed = 0;
        var tokens = student.Name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var token in tokens)
        {
            string term = token;
            if (!index.ContainsKey(term))
            {
                index[term] = new List<int>();
            }

            if (!index[term].Contains(student.Id))
            {
                index[term].Add(student.Id);
                tokensIndexed++;
            }
        }

        if (tokensIndexed > 0)
        {
            return "Student indexed successfully";
        }
        else
        {
            return "Student already indexed or no valid tokens";
        }
    }

    public List<int> Search(string token)
    {
        if (index.ContainsKey(token))
        {
            return index[token];
        }

        return new List<int>();
    }

    public string Delete(Student student)
    {
        if (string.IsNullOrEmpty(student.Name))
        {
            return "Deletion failed: Student name is empty";
        }

        var tokens = student.Name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        bool anyRemoved = false;

        foreach (var token in tokens)
        {
            if (index.ContainsKey(token))
            {
                if (index[token].Remove(student.Id))
                {
                    anyRemoved = true;
                }

                if (index[token].Count == 0)
                {
                    index.Remove(token);
                }
            }
        }

        if (anyRemoved)
        {
            return "Student removed from index successfully";
        }
        else
        {
            return "Student not found in index";
        }
    }
}