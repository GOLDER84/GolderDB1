namespace UniDB.Engine;

public enum CommandType
{
    InsertOne,
    DeleteOne,
    FindById,
    FindAll,
    Filter,
    Count,
    Sum,
    Average,
    Import,
    BeginTransaction,
    Rollback,
    Commit,
    Batch,
    CreateIndex,
    Search
}