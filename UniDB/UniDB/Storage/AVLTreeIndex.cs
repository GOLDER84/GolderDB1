using UniDB.Storage.Interfaces;

namespace UniDB.Storage;

using UniDB.Domain;

public class AVLNode
{
    public Student Data;
    public AVLNode Left;
    public AVLNode Right;
    public int Height;

    public AVLNode(Student data)
    {
        Data = data;
        Height = 1;
    }
}

public class AVLTreeIndex : IIndex
{
    public AVLNode Root;

    private int Height(AVLNode N)
    {
        if (N == null) return 0;
        return N.Height;
    }

    private int Max(int a, int b)
    {
        return (a > b) ? a : b;
    }

    private AVLNode RightRotate(AVLNode y)
    {
        AVLNode x = y.Left;
        AVLNode T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        y.Height = Max(Height(y.Left), Height(y.Right)) + 1;
        x.Height = Max(Height(x.Left), Height(x.Right)) + 1;

        return x;
    }

    private AVLNode LeftRotate(AVLNode x)
    {
        AVLNode y = x.Right;
        AVLNode T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        x.Height = Max(Height(x.Left), Height(x.Right)) + 1;
        y.Height = Max(Height(y.Left), Height(y.Right)) + 1;

        return y;
    }

    private int GetBalance(AVLNode N)
    {
        if (N == null) return 0;
        return Height(N.Left) - Height(N.Right);
    }

    public string Insert(Student student)
    {
        bool isInserted = false;
        Root = InsertRec(Root, student, ref isInserted);

        if (isInserted)
        {
            return "Student inserted successfully";
        }
        else
        {
            return "Student insertion failed (Duplicate ID)";
        }
    }

    private AVLNode InsertRec(AVLNode node, Student student, ref bool isInserted)
    {
        if (node == null)
        {
            isInserted = true;
            return new AVLNode(student);
        }

        if (student.Id < node.Data.Id)
        {
            node.Left = InsertRec(node.Left, student, ref isInserted);
        }
        else if (student.Id > node.Data.Id)
        {
            node.Right = InsertRec(node.Right, student, ref isInserted);
        }
        else
        {
            isInserted = false;
            return node;
        }

        node.Height = 1 + Max(Height(node.Left), Height(node.Right));
        int balance = GetBalance(node);

        if (balance > 1 && student.Id < node.Left.Data.Id)
        {
            return RightRotate(node);
        }

        if (balance < -1 && student.Id > node.Right.Data.Id)
        {
            return LeftRotate(node);
        }

        if (balance > 1 && student.Id > node.Left.Data.Id)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        if (balance < -1 && student.Id < node.Right.Data.Id)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }

    public string Delete(Student student)
    {
        return Delete(student.Id);
    }
    public string Delete(int id)
    {
        bool isDeleted = false;
        Root = DeleteRec(Root, id, ref isDeleted);

        if (isDeleted)
        {
            return "Student deleted successfully";
        }
        else
        {
            return "Student not found";
        }
    }

    private AVLNode DeleteRec(AVLNode root, int id, ref bool isDeleted)
    {
        if (root == null)
        {
            return root;
        }

        if (id < root.Data.Id)
        {
            root.Left = DeleteRec(root.Left, id, ref isDeleted);
        }
        else if (id > root.Data.Id)
        {
            root.Right = DeleteRec(root.Right, id, ref isDeleted);
        }
        else
        {
            isDeleted = true;

            if ((root.Left == null) || (root.Right == null))
            {
                AVLNode temp = null;
                if (temp == root.Left) temp = root.Right;
                else temp = root.Left;

                if (temp == null)
                {
                    temp = root;
                    root = null;
                }
                else
                {
                    root = temp;
                }
            }
            else
            {
                AVLNode temp = MinValueNode(root.Right);
                root.Data = temp.Data;
                bool dummy = false;
                root.Right = DeleteRec(root.Right, temp.Data.Id, ref dummy);
            }
        }

        if (root == null) return root;

        root.Height = Max(Height(root.Left), Height(root.Right)) + 1;
        int balance = GetBalance(root);

        if (balance > 1 && GetBalance(root.Left) >= 0)
        {
            return RightRotate(root);
        }

        if (balance > 1 && GetBalance(root.Left) < 0)
        {
            root.Left = LeftRotate(root.Left);
            return RightRotate(root);
        }

        if (balance < -1 && GetBalance(root.Right) <= 0)
        {
            return LeftRotate(root);
        }

        if (balance < -1 && GetBalance(root.Right) > 0)
        {
            root.Right = RightRotate(root.Right);
            return LeftRotate(root);
        }

        return root;
    }

    private AVLNode MinValueNode(AVLNode node)
    {
        AVLNode current = node;
        while (current.Left != null)
        {
            current = current.Left;
        }

        return current;
    }

    public Student Search(int id)
    {
        return SearchRec(Root, id);
    }
    private Student SearchRec(AVLNode root, int id)
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
}