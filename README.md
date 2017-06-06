# EasyMig

> Migration Tool and Services

Support:
* **Sql Server** 
* **MySQL** (require [MySQL Connector](https://dev.mysql.com/downloads/connector/net/) For .NET)

Commands:
* **Drop Database**
* **Create Database**
* **Create Table**
    * _Add primary key_
    * _Add column_ (type, nullable, default, unique)
    * _Add foreign key_
    * _add timestamps_
    * _Insert data_
* **Alter Table**
    * _Add column_
    * _Modify column_
    * _Drop column_
    * _Add primary key constraint_
    * _Add foreign key constraint_
* **Drop Table**

* **SeedTable**
    * Insert dictionary of key/value

Execution:
* Do **Migrations All**: update database from Assembly path or Types or in Memory
* Do **Migration One** (only one migration file/Type)
* Do **Seed All**: from Assembly path or Types or in Memory
* Do **Seed One** (only one seeder)
* Get Migrations | Seeders **Query string**
* Create **Migration Script** (create table, etc.)
* Create **Seed Script**
* **Execute** a sql **Query**

Database information:
* Check if **Database exists**
* Check if **Table exists**
* Check if **Column exists**
* **Get Table** Schema with columns, primary key and foreign key definitions
* **Get** Table **rows**

**Version**:
Migration and Seeder types are **grouped** by name **and sorted** by version and name. Example:
"_001_CREATE_POSTS_TABLE" is before "_002_CREATE_POSTS_TABLE". Only the last is executed ("_002_CREATE_POSTS_TABLE" here)

"_001_CREATE_POSTS_TABLE" : the version is "_001_", the name is "CREATE_POSTS_TABLE", the full name is "_001_CREATE_POSTS_TABLE"

## Migrations and Seeders

Create a **Migration file**

```cs
public class CREATE_POSTS_TABLE : Migration
{
    public override void Up()
    {
        EasyMig.CreateTable("posts")
              .AddPrimaryKey("id") // key auto incremented
              .AddColumn("title")
              .AddColumn("content", ColumnType.Text())
              .AddTimestamps() // add created_at and updated_at columns
              .AddForeignKey("user_id", "users", "id")
              // we could initliaze table with data (identity off)
              .Insert(SeedData.New.Set("id", 1).Set("title", "Post 1").Set("content", "Content 1").Set("user_id", 1))
              .Insert(SeedData.New.Set("id", 2).Set("title", "Post 2").Set("content", "Content 2").Set("user_id", 1))
              .Insert(SeedData.New.Set("id", 3).Set("title", "Post 3").Set("content", "Content 3").Set("user_id", 2));

    }

    public override void Down()
    {
        EasyMig.DropTable("posts");
    }

}
```

Create a **Seeder file** 
Allow to seed an existing table

```cs
public class Posts_Seeder : Seeder
{
    public override void Run()
    {
        EasyMig.SeedTable("posts")
            // dictionary (string, object) or use SeeData helper
            .Insert(SeedData.New.Set("title", "Post 4").Set("content", "Content 4").Set("user_id", 3))
            .Insert(SeedData.New.Set("title", "Post 5").Set("content", "Content 5").Set("user_id", 3))
    }
}
```

Then use the EasyMig Tool is the easy way to select types and do migrations.

<img src="http://res.cloudinary.com/romagny13/image/upload/v1496624500/easymigapp_tool_sc0yol.png">

## In Memory

But its not a requirement. We could define migrations and seeders where we want and execute from Memory.

Example:
```cs
// define commands
EasyMig
    .AlterTable("posts")
    .AddForeignKeyConstraint("category_id", "categories", "id");

    // other commands ...

// execute
EasyMig.DoMigrationsFromMemory(@"Server=localhost\SQLEXPRESS;Database=db1;Trusted_Connection=True;", "System.Data.SqlClient");
```

## Commands

### Drop Database

```cs
EasyMig.DropDatabase("db1"); 
```
Sql generated with SQL Server for example

```sql
DROP DATABASE IF EXISTS [db1];
```

### Create Database

```cs
EasyMig.CreateDatabase("db1");
```

```sql
CREATE DATABASE [db1];
```

### Create Table

### Add a primary key

By default, the primary key is "INT", "UNSIGNED" and auto incremented ("AUTO_INCREAMENT" for MySQL, "IDENTITY(1,1)" for Sql Server)

```cs
 EasyMig.CreateTable("users")
        .AddPrimaryKey("id");
```

We could define the primary key. Example with the type "varchar"

```cs
EasyMig.CreateTable("users")
       .AddPrimaryKey("id",ColumnType.VarChar());
```

### Add columns

By default, the column ("username" in this example) is "VARCHAR" ("VARCHAR(255)" for MySQL, "NVARCHAR(255)" for Sql Server), "NOT NULL"

```cs
EasyMig.CreateTable("users")
       .AddPrimaryKey("id")
       .AddColumn("username");
```

Change the type and set as "NULL". Example with the column "age" ("TINYINT UNSIGNED" and "NULL")

```cs
  EasyMig.CreateTable("users")
         .AddPrimaryKey("id",ColumnType.VarChar())
         .AddColumn("username")
         .AddColumn("age",ColumnType.TinyInt(true),true);
```

#### ColumnTypes

Numbers (all are "unsignables"):
* TinyInt
* SmallInt
* Int
* BigInt

Type | MySQL | SQL Server
-------- |  --------
Float | FLOAT or FLOAT(10,d) with digits | FLOAT or DECIMAL(18,d) with digits

* Bit for "Boolean" (accepts 0,1 or NULL)

String Types:
Type | MySQL | SQL Server
-------- |  --------
Char | CHAR(n) by default n is 10 | NCHAR(n) by default n is 10
VarChar | VARCHAR(n) by default n is 255 | NVARCHAR(n) by default n is 255
Text | TEXT | NVARCHAR(MAX)
LongText | LONGTEXT | NTEXT

Datetimes:
* DateTime (format:"YYYY-MM-DD HH:MI:SS")
* Date (format:"YYYY-MM-DD")
* Time (format:"HH:MI:SS")
* Timestamp

Type | MySQL | SQL Server
-------- |  --------
Blob | BLOB | VARBINARY(MAX)

### Add a foreign key

Example with the column "user_id" in the table "posts" that references the primary key "id" of the table "users"

```cs
 EasyMig.CreateTable("posts")
                .AddPrimaryKey("id")
                .AddColumn("title")
                .AddColumn("content", ColumnType.Text())
                .AddForeignKey("user_id", "users", "id");
```

### Constraints: default value and unique

Default value, example with the column "user_id" (default value 1):

```cs
EasyMig.CreateTable("posts")
                .AddPrimaryKey("id")
                .AddColumn("title")
                .AddColumn("content", ColumnType.Text())
                .AddForeignKey("user_id", ColumnType.Int(true), "users", "id", false, 1);
```

Unique, example with the column email (unique is the last parameter | boolean):

```cs
EasyMig.CreateTable("users")
       .AddColumn("email",ColumnType.VarChar(),false,null,true);
```

### Insert data on table creation

We could define the primary key ("_IDENTITY_INSERT_" is "off" for **Sql Server**).

Example with **Dictionaries**

```cs
EasyMig.CreateTable("users")
       .AddPrimaryKey("id",ColumnType.VarChar())
       .AddColumn("username")
       .Insert(new Dictionary<string, object> { { "id",1 }, { "username", "user1" } })
       .Insert(new Dictionary<string, object> { { "id", 2 }, { "username", "user2" } });
```
... Or with **SeedData** helper

```cs
 EasyMig.CreateTable("users")
        .AddPrimaryKey("id",ColumnType.VarChar())
        .AddColumn("username")
        .Insert(SeedData.New.Set("id",1).Set("username","user1"))
        .Insert(SeedData.New.Set("id", 2).Set("username", "user2"));
```

## Alter Table command

Allow to modify an existing Table.

### Add column

```cs
 EasyMig.AlterTable("users")
                .AddColumn("firstname"); // + column type, nullable, default value ...
```
### Modify Column

```cs
  EasyMig.AlterTable("users")
         .ModifyColumn("firstname", true); // example set as nullable
```

Note: Sql Server do not suppoort column modification with a default value or unqiue constraint, so these constraints are disabled with Sql Server.

### Drop column

```cs
 EasyMig.AlterTable("users")
        .DropColumn("firstname");
```

### Add Primary Key Constraint

```cs
 EasyMig.AlterTable("posts")
        .AddPrimaryKeyConstraint("id");
```

With primary keys

```cs
 EasyMig.AlterTable("posts")
        .AddPrimaryKeyConstraint("id1","id2");
```

### Add Foreign Key Constraint

```cs
EasyMig.AlterTable("posts")
       .AddForeignKeyConstraint("user_id","users","id"); // could be set as 
```
Set as nullable for relation 0.1

```cs
 EasyMig.AlterTable("posts")
        .AddForeignKeyConstraint("user_id","users","id", true);
```

## Seed Table

Seed an existing table

Example
```cs
// here we do not define the primary key (auto increment)
 EasyMig.SeedTable("users")
        .Insert(SeedData.New.Set("username", "user3")) 
        .Insert(SeedData.New.Set("username", "user4"));
```

## Execution

### Migrations 

#### From Memory

Example with MySQL
```cs
 EasyMig.DoMigrationsFromMemory("server=localhost;database=db1;uid=root", "MySql.Data.MySqlClient");
```
* **Default engine** is "**InnoDB**" (databases are created with relations and schema). But its possible to change to "**MyISAM**"

<img src="http://res.cloudinary.com/romagny13/image/upload/v1496622672/mysql_schema_bevqqq.png">

Change the engine. Example:
```cs
 EasyMig.DoMigrationsFromMemory("server=localhost;database=db1;uid=root", "MySql.Data.MySqlClient", "MyISAM");
 ```

 Example with Sql Server
 ```cs
  EasyMig.DoMigrationsFromMemory(@"Server=localhost\SQLEXPRESS;Database=db1;Trusted_Connection=True;", "System.Data.SqlClient");
  ```

### With Assembly

Example:

Assembly path (exe or dll), MigrationDirection (Down or Up by default, the method to execute in Migrations)
```cs
 EasyMig.DoMigrationsForAssembly("c:/path/to/assembly.dll", connectionString, providerName, null, MigrationDirection.Up);
```

Or With Types
```cs
 EasyMig.DoMigrationsForTypes(new Type[] { typeof(MyMigration1), typeof(MyMigration2) }, connectionString, providerName, null, MigrationDirection.Up);
```

Its possible to execute only for one type with fileName or file type. Example:

```cs
EasyMig.DoMigrationOnlyFor("c:/path/to/assembly.dll","MyMigration1", connectionString, providerName, null, MigrationDirection.Up);
```

### Seed

```cs
EasyMig.DoSeedForAssembly("c:/path/to/assembly.dll", connectionString, providerName);
```
...Or with types

... Only for one file
```cs
 EasyMig.DoSeedOnlyFor("MySeeder","c:/path/to/assembly.dll", connectionString, providerName);
 ```

### Get SQL Queries

Migrations:

```cs
var sql = EasyMig.GetMigrationQuery(providerName);
```

Seeders:

```cs
var sql = EasyMig.GetSeedQuery(providerName);
```
### Create Scripts

```cs
EasyMig.CreateMigrationScript("c:/path/to/assembly.dll",providerName);
```

```cs
EasyMig.CreateSeedScript("c:/path/to/assembly.dll",providerName);
```
 
## Fake Data

Example with [Faker.Net](https://github.com/slashdotdash/faker-cs)

```
PM> Install-Package Faker.Net
```

```cs
var table = EasyMig.CreateTable("users")
               .AddPrimaryKey("id")
               .AddColumn("firstname")
               .AddColumn("lastname")
               .AddColumn("age", ColumnType.Int())
               .AddColumn("email")
               .AddColumn("phone")
               .AddColumn("address")
               .AddColumn("zip")
               .AddColumn("city")
               .AddColumn("note", ColumnType.Text());

for (int i = 1; i< 100; i++)
{
    table.Insert(SeedData.New
    .Set("id", i)
    .Set("firstName", Faker.Name.First())
    .Set("lastname", Faker.Name.Last())
    .Set("age", Faker.RandomNumber.Next(20, 50))
    .Set("email", Faker.Internet.Email())
    .Set("phone", Faker.Phone.Number())
    .Set("address", Faker.Address.StreetAddress())
    .Set("zip", Faker.Address.ZipCode())
    .Set("city", Faker.Address.City())
    .Set("note", Faker.Lorem.Paragraph())
    );
}

//  var query = EasyMig.GetMigrationQuery("MySql.Data.MySqlClient");
// or
EasyMig.DoMigrationsFromMemory("server=localhost;database=dbtest;uid=root", "MySql.Data.MySqlClient");

// or with Sql Server
// EasyMig.DoMigrationsFromMemory(@"Server=localhost\SQLEXPRESS;Database=db1;Trusted_Connection=True;", "System.Data.SqlClient");
```


## Database Information

Check if **Database exists**

```cs
var result = EasyMig.DatabaseInformation.DatabaseExists("mydb", connectionString, providerName)
```

Check if **Table exists**

```cs
var result = EasyMig.DatabaseInformation.TableExists("mydb", "users", connectionString, providerName)
```
Check if **Column exists**

```cs
var result = EasyMig.DatabaseInformation.ColumnExists("mydb", "users", "username" , connectionString, providerName))
```

**Get Table** Schema with columns, primary key and foreign key definitions

```cs
 var table = EasyMig.DatabaseInformation.GetTable("mydb", "users", connectionString, providerName);
```

**Get** Table **rows**

```cs
var tableRows = EasyMig.DatabaseInformation.GetTableRows("users", connectionString, providerName);
Assert.AreEqual((uint)1,((uint) tableRows[0]["id"])); // caution to type with MySQL unsigned numbers
Assert.AreEqual("user1", (string) tableRows[0]["username"]);
Assert.AreEqual(20, ((int)tableRows[0]["age"]));
```
