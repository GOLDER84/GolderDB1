# UniDB

> A lightweight educational NoSQL database engine built from scratch to explore database internals, data structures, indexing, query execution, and clean software architecture.

![Language](https://img.shields.io/badge/Language-C%23-blue)
![Database](https://img.shields.io/badge/Database-NoSQL-orange)
![Architecture](https://img.shields.io/badge/Architecture-Layered-green)
![Status](https://img.shields.io/badge/Status-Academic%20Project-purple)

## 📌 Overview

**UniDB** is a custom NoSQL database engine developed as an academic project for the Data Structures course.

The project focuses on implementing core database concepts from scratch rather than relying on existing database engines. It uses fundamental data structures and algorithmic techniques to provide document storage, querying, transactions, batch processing, indexing, and query optimization.

The project was developed in two phases:

* **Phase 1:** Storage engine, query parser, execution engine, transactions, batch processing, and data loading.
* **Phase 2:** Database indexing and query optimization using BST, AVL Tree, Hash Table, and Inverted Index.

---

## 🎯 Project Goals

The main goals of UniDB are:

* Understanding how a database engine manages data internally
* Implementing storage structures without using a built-in database
* Applying fundamental data structures to real-world database problems
* Designing a modular and maintainable architecture
* Understanding query execution and optimization
* Comparing the performance of different data structures
* Exploring how database indexes improve search performance

---

## 🏗️ Architecture

UniGolderDB follows a layered architecture that separates parsing, execution, storage, and indexing responsibilities.

```text
                    ┌─────────────────────┐
                    │      User Query     │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │   Query Parser      │
                    │                     │
                    │ • Tokenization      │
                    │ • Command Parsing   │
                    │ • Parameter Extract │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │  Execution Engine   │
                    │                     │
                    │ • Command Queue     │
                    │ • Transactions      │
                    │ • Batch Processing  │
                    └──────────┬──────────┘
                               │
                               ▼
              ┌─────────────────────────────────┐
              │         Storage Engine          │
              │                                 │
              │  Array Collection               │
              │  Linked List Collection         │
              └────────────────┬────────────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │      Index Layer    │
                    │                     │
                    │ • BST               │
                    │ • AVL Tree          │
                    │ • Hash Index        │
                    │ • Inverted Index    │
                    └─────────────────────┘
```

The architecture keeps the parser independent from the storage layer, while the execution engine coordinates commands, transactions, and database operations.

---

# 🚀 Phase 1 — Database Core

The first phase focuses on building the core of the database engine.

The database stores information in a **document-oriented format**, inspired by systems such as MongoDB.

### Storage Engine

Two collection implementations were designed:

#### Array Collection

A dynamic-array-based storage structure supporting operations such as:

* `insertOne`
* `deleteOne`
* `findByID`
* `findAll`
* `filter`
* `count`
* `sum`
* `average`

#### Linked List Collection

A doubly linked-list implementation providing similar database operations while demonstrating the trade-offs between arrays and linked lists.

---

## 🔍 Query Parser

The query parser converts raw textual commands into structured command objects.

For example:

```text
db.students.insertOne({_id: 101, name: "Ali", gpa: 18.5})
```

The parser is responsible for:

* Tokenization
* Command identification
* Parameter extraction
* Creating command objects

This keeps the database engine independent from the syntax used by the user.

---

## ⚙️ Execution Engine

The execution engine is responsible for executing parsed commands.

It includes:

### Transaction Management

Transactions are implemented using a stack-based approach.

Supported operations include:

```text
db.beginTransaction()
db.commit()
db.rollback()
```

### Batch Processing

Commands can also be executed in batches:

```text
db.batch.start()

db.students.insertOne(...)
db.students.deleteOne(...)

db.batch.execute()
```

A queue is used to maintain the order of batched commands.

---

## 📥 Bulk Data Import

UniDB supports importing student records from CSV files.

Example:

```text
db.students.import("data_50k.csv")
```

The importer reads the CSV file line by line, converts records into documents, and inserts them into the collection.

---

# ⚡ Phase 2 — Indexing & Query Optimization

The second phase extends the database with indexing capabilities.

Without indexes, searching for a record requires scanning the collection. With large datasets, this can become inefficient.

UniDB therefore introduces multiple index structures.

---

## 🌳 BST Index

A Binary Search Tree is used for indexing numeric and string fields.

Supported operations:

* Insert
* Search
* Delete

BST indexing demonstrates how ordered data can improve search operations compared with a linear scan.

---

## 🌲 AVL Index

An AVL Tree extends the BST approach by maintaining tree balance.

The implementation uses rotations to preserve the tree's balance after modifications.

This provides predictable logarithmic search complexity.

---

## #️⃣ Hash Index

A Hash Table is provided for exact-match searches such as:

```text
id = 101
```

and:

```text
national_code = "1234567890"
```

Collision handling is implemented using **chaining with linked lists**.

---

## 🔎 Inverted Index

The Inverted Index is designed for text-based searches.

Instead of searching every document individually, words are mapped to the IDs of documents containing them.

For example:

```text
"Ali"     → [101, 204, 315]
"Rezaei"  → [101, 422]
```

This structure is particularly useful for full-text style searches.

---

# 🧠 Query Optimization

The execution engine can choose between indexed search and a full collection scan.

The basic strategy is:

```text
                 Query Filter
                      │
                      ▼
              Is an index available?
                 /             \
               Yes              No
                │                │
                ▼                ▼
         Use the index       Full Scan
```

When a suitable index exists, the query can use the corresponding BST, AVL, Hash, or Inverted Index instead of scanning the entire collection.

---

# 📊 Performance Analysis

The project also includes performance analysis of different data structures.

The analysis focuses on:

### BST vs AVL

Sorted data is inserted into both structures and their search performance is compared.

This demonstrates the effect of tree balancing on search complexity.

### Hash vs Tree vs Full Scan

`findByID` performance is compared using:

1. Full collection scan
2. Tree-based index
3. Hash-based index

The goal is to demonstrate how indexing can significantly reduce the amount of data that must be inspected during a query.

---

# 🧩 Data Structures Used

| Data Structure     | Purpose                            |
| ------------------ | ---------------------------------- |
| Dynamic Array      | Document storage                   |
| Doubly Linked List | Collection storage & hash chaining |
| Stack              | Transaction management             |
| Queue              | Batch command processing           |
| BST                | Ordered indexing                   |
| AVL Tree           | Balanced ordered indexing          |
| Hash Table         | Exact-match indexing               |
| Inverted Index     | Text search                        |

---
📁 Project Structure

The project is organized into separate layers and components to keep the database engine modular, maintainable, and easy to extend.

```text
UniDB/
│
├── Benchmark/
│   ├── BenchmarkRunner.cs
│   └── DataGenerator.cs
│
├── Domain/
│   └── Student.cs
│
├── Engine/
│   ├── Command.cs
│   ├── CommandType.cs
│   ├── ExecutionEngine.cs
│   └── TransactionStack.cs
│
├── Parser/
│   └── QueryParser.cs
│
├── Storage/
│   ├── Interfaces/
│   │   ├── ICollection.cs
│   │   └── IIndex.cs
│   │
│   ├── AVLTreeIndex.cs
│   ├── ArrayCollection.cs
│   ├── BSTIndex.cs
│   ├── HashIndex.cs
│   ├── InvertedIndex.cs
│   ├── LinkedListCollection.cs
│   └── StorageManagement.cs
│
├── Data.csv
├── data_50k.csv
└── Program.cs
```

🔄 Request Flow

A typical database request follows the following flow:

```text
User Query
    │
    ▼
QueryParser
    │
    ▼
Command
    │
    ▼
ExecutionEngine
    │
    ├───────────────┐
    │               │
    ▼               ▼
Storage         Indexes
    │               │
    ├── Array       ├── BST
    │               ├── AVL
    └── LinkedList  ├── Hash
                    └── Inverted
```
For indexed queries, the execution engine can use an appropriate index instead of performing a full scan over the collection.


---

# 💻 Example Commands

### Insert

```text
db.students.insertOne({
    _id: 101,
    name: "Ali",
    gpa: 18.5
})
```

### Find by ID

```text
db.students.findByID(101)
```

### Find All

```text
db.students.findAll()
```

### Filter

```text
db.students.filter("name", "Ali")
```

### Delete

```text
db.students.deleteOne({_id: 101})
```

### Aggregation

```text
db.students.count()
db.students.sum("gpa")
db.students.average("gpa")
```

### Import CSV

```text
db.students.import("data_50k.csv")
```

---

🧪 Benchmarking

UniDB includes a dedicated benchmarking component.

The benchmark system can be used to evaluate the performance differences between:

Array-based storage
Linked-list-based storage
BST indexing
AVL indexing
Hash indexing
Full collection scans

Large datasets such as data_50k.csv can be used to make these comparisons more meaningful.

---

# 🧱 Clean Architecture & Code Quality

A major requirement of the project is maintaining a modular and maintainable codebase.

The implementation separates:

```text
Parser
   ↓
Execution Engine
   ↓
Storage
   ↓
Indexes
```

The project follows principles such as:

* Separation of responsibilities
* Interface-based abstraction
* Meaningful naming
* Small and focused methods
* Avoiding duplicated code
* Independent data structure implementations
* Separation of storage, indexing, and query logic

Each major data structure is implemented as a separate component rather than placing the entire database engine inside a single class or source file.

---

# 📚 Concepts Demonstrated

This project combines several fundamental Computer Science concepts:

* Data Structures
* Algorithms & Complexity Analysis
* NoSQL Database Design
* Query Parsing
* Query Execution
* Database Indexing
* Query Optimization
* Transactions
* Batch Processing
* Clean Architecture
* Object-Oriented Design
* Performance Benchmarking

---

# 📁 Project Documentation

The repository also contains the original project documentation for both phases:

* `UniDB.pdf` — Phase 1 specification
* `UniDB2.pdf` — Phase 2 specification

These documents describe the academic requirements, architecture, implementation requirements, performance analysis, and evaluation criteria.

---

# 🎓 Academic Context

**Course:** Data Structures
**Project:** UniDB — NoSQL Database Engine
**Institution:** University of Isfahan
**Development:** Academic Project
**Phases:** 2

---

# 👨‍💻 Author

**Aref Zargar**

Computer Engineering Student
University of Isfahan

GitHub: [GOLDER84](https://github.com/GOLDER84)

---

## ⭐ Highlights

UniDB was built to go beyond implementing isolated data structures and demonstrate how they can work together inside a practical software system.

The project connects fundamental concepts such as **arrays, linked lists, stacks, queues, trees, hashing, and indexing** to real database-engineering problems such as **storage management, transactions, query execution, and search optimization**.
