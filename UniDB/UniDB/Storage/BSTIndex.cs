using UniDB.Storage.Interfaces;

namespace UniDB.Storage;

using UniDB.Domain;

public class BSTNode
{
    public Student Data;
    public BSTNode Left;
    public BSTNode Right;

    public BSTNode(Student data)
    {
        Data = data;
        Left = Right = null;
    }
}

public class BSTIndex : IIndex
{
    public BSTNode Root;
    
    public string Insert(Student student)
    {
        bool isInserted = false;
        Root = InsertRec(Root, student , ref isInserted);
        if (isInserted)
        {
            return "Student inserted successfully";
        }
        else
        {
            return "Student insertion failed (Duplicate ID)";
        }
    }

    private BSTNode InsertRec(BSTNode root, Student student , ref bool isInserted)
    {
        if (root == null)
        {
            isInserted = true;
            return new BSTNode(student);
        }

        if (student.Id < root.Data.Id)
        {
            root.Left = InsertRec(root.Left, student , ref isInserted);
        }
        else if (student.Id > root.Data.Id)
        {
            root.Right = InsertRec(root.Right, student  , ref isInserted);
        }
        else
        {
            isInserted = false;
        }
        return root;
    }
    
    public Student Search(int id)
    {
        return SearchRec(Root, id);
    }

    private Student SearchRec(BSTNode root, int id)
    {
        if (root == null)
        {
            return null;
        }

        if (root.Data.Id == id)
        {
            return root.Data;
        }

        if (id < root.Data.Id)
        {
            return SearchRec(root.Left, id);
        }
        else
        {
            return SearchRec(root.Right, id);
        }
    }

    public string Delete(Student student)
    {
        return Delete(student.Id);
    }

    public string Delete(int id)
    {
        bool isDeleted = false;
        Root = DeleteRec(Root, id , ref isDeleted);
        if (isDeleted)
        {
            return "Student deleted successfully";
        }
        else
        {
            return "Student not found";
        }
    }

    private BSTNode DeleteRec(BSTNode root, int id ,  ref bool isDeleted)
    {
        if (root == null)
        {
            return root;
        }

        if (id < root.Data.Id)
        {
            root.Left = DeleteRec(root.Left, id ,  ref isDeleted);
        }
        else if (id > root.Data.Id)
        {
            root.Right = DeleteRec(root.Right, id  ,  ref isDeleted);
        }
        else
        {
            isDeleted = true;
            if (root.Left == null)
            {
                return root.Right;
            }
            else if (root.Right == null)
            {
                return root.Left;
            }
            
            root.Data = MinValue(root.Right);
            
            bool dummy = false;
            root.Right = DeleteRec(root.Right, root.Data.Id , ref dummy);
        }

        return root;
    }

    private Student MinValue(BSTNode root)
    {
        Student minv = root.Data;
        while (root.Left != null)
        {
            minv = root.Left.Data;
            root = root.Left;
        }
        return minv;
    }
    
}