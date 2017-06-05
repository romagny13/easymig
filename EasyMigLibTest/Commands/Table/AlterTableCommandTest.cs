﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyMigLib.Commands;
using EasyMigLib;
using EasyMigLib.Services;

namespace EasyMigLibTest.Commands.Table
{
    [TestClass]
    public class AlterTableCommandTest
    {
        // add column

        [TestMethod]
        public void TestAddColumnCommand()
        {
            var tableName = "table1";
            var columnName = "column1";

            var container = new AlterTableCommand(tableName);

            container.AddColumn(columnName);

            Assert.IsTrue(container.HasAddColumnCommand(columnName));

            var result = container.GetAddColumnCommand(columnName);

            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(columnName, result.Column.ColumnName);
            // Assert.AreEqual(typeof(StringColumnType), result.Column.ColumnType);
            Assert.AreEqual(null, result.Column.DefaultValue);
            Assert.AreEqual(false, result.Column.Nullable);
            Assert.AreEqual(false, result.Column.Unique);
        }

        [TestMethod]
        public void TestAddColumnCommand_WithConstraints()
        {
            var tableName = "table1";
            var columnName = "column1";

            var container = new AlterTableCommand(tableName);

            container.AddColumn(columnName, ColumnType.Int(), true, 10, true);

            Assert.IsTrue(container.HasAddColumnCommand(columnName));

            var result = container.GetAddColumnCommand(columnName);

            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(columnName, result.Column.ColumnName);
            //   Assert.AreEqual(typeof(IntColumnType), result.Column.ColumnType);
            Assert.AreEqual(10, result.Column.DefaultValue);
            Assert.AreEqual(true, result.Column.Nullable);
            Assert.AreEqual(true, result.Column.Unique);
        }

        // modify column

        [TestMethod]
        public void TestModifyColumnCommand_WithConstraints()
        {
            var tableName = "table1";
            var columnName = "column1";

            var container = new AlterTableCommand(tableName);

            container.ModifyColumn(columnName, ColumnType.Int(), true, 10, true);

            Assert.IsTrue(container.HasModifyColumnCommand(columnName));

            var result = container.GetModifyColumnCommand(columnName);

            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(columnName, result.Column.ColumnName);
            //   Assert.AreEqual(typeof(IntColumnType), result.Column.ColumnType);
            Assert.AreEqual(10, result.Column.DefaultValue);
            Assert.AreEqual(true, result.Column.Nullable);
            Assert.AreEqual(true, result.Column.Unique);
        }

        // drop column

        [TestMethod]
        public void TestDropColumnCommand()
        {
            var tableName = "table1";
            var columnName = "column1";

            var container = new AlterTableCommand(tableName);

            container.DropColumn(columnName);

            Assert.IsTrue(container.HasDropColumnCommand(columnName));

            var result = container.GetDropColumnCommand(columnName);

            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(columnName, result.ColumnName);
        }

        // primary key constraint

        [TestMethod]
        public void TestAddPrimaryKeyConstraint_WithConstraints()
        {
            var tableName = "table1";
            var columnName = "column1";

            var container = new AlterTableCommand(tableName);

            container.AddPrimaryKeyConstraint("table1", new string[] { columnName });

            Assert.IsTrue(container.HasPrimaryKeyConstraintCommand());

            var result = container.GetPrimaryKeyConstraintCommand(columnName);

            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(columnName, result.PrimaryKeys[0]);
        }

        // foreign key constraint

        [TestMethod]
        public void TestAddForeignKeyConstraint_WithConstraints()
        {
            var tableName = "table1";
            var columnName = "column1";

            var container = new AlterTableCommand(tableName);

            container.AddForeignKeyConstraint(columnName, ColumnType.String(), "users", "id", true);

            Assert.IsTrue(container.HasForeignKeyConstraintCommand(columnName));

            var result = container.GetForeignKeyConstraintCommand(columnName);

            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(columnName, result.ForeignKey.ColumnName);
            // Assert.AreEqual(typeof(IntColumnType), result.ForeignKey.ColumnType);
            Assert.AreEqual("id", result.ForeignKey.PrimaryKeyReferenced);
            Assert.AreEqual("users", result.ForeignKey.TableReferenced);
            Assert.AreEqual(true, result.ForeignKey.Nullable);
        }

        [TestMethod]
        public void TestGetQuery_WithAddColumns()
        {
            var tableName = "table1";

            var container = new AlterTableCommand(tableName);
            container
                .AddColumn("column1")
                .AddColumn("column2");

            var result = container.GetQuery(new SqlQueryService());

            Assert.AreEqual("ALTER TABLE [dbo].[table1] ADD [column1] NVARCHAR(255) NOT NULL;\r\rALTER TABLE [dbo].[table1] ADD [column2] NVARCHAR(255) NOT NULL;\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithModify()
        {
            var tableName = "table1";

            var container = new AlterTableCommand(tableName);
            container
                .ModifyColumn("column1")
                .ModifyColumn("column2");

            var result = container.GetQuery(new SqlQueryService());

            Assert.AreEqual("ALTER TABLE [dbo].[table1] ALTER COLUMN [column1] NVARCHAR(255) NOT NULL;\r\rALTER TABLE [dbo].[table1] ALTER COLUMN [column2] NVARCHAR(255) NOT NULL;\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithDrop()
        {
            var tableName = "table1";

            var container = new AlterTableCommand(tableName);
            container
                .DropColumn("column1")
                .DropColumn("column2");

            var result = container.GetQuery(new SqlQueryService());

            Assert.AreEqual("ALTER TABLE [dbo].[table1] DROP COLUMN [column1];\r\rALTER TABLE [dbo].[table1] DROP COLUMN [column2];\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithPrimary()
        {
            var tableName = "table1";

            var container = new AlterTableCommand(tableName);
            container
                .AddPrimaryKeyConstraint("column1",new string[] { "column1","column2" });

            var result = container.GetQuery(new SqlQueryService());

            Assert.AreEqual("ALTER TABLE [dbo].[column1] ADD PRIMARY KEY ([column1],[column2]);\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithForeignKeys()
        {
            var tableName = "posts";

            var container = new AlterTableCommand(tableName);
            container
                .AddForeignKeyConstraint("user_id","users","id")
                .AddForeignKeyConstraint("category_id", "categories", "id");

            var result = container.GetQuery(new SqlQueryService());

            Assert.AreEqual("ALTER TABLE [dbo].[posts] ADD FOREIGN KEY ([user_id]) REFERENCES [dbo].[users]([id]);\r\rALTER TABLE [dbo].[posts] ADD FOREIGN KEY ([category_id]) REFERENCES [dbo].[categories]([id]);\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithConstraints()
        {
            var tableName = "posts";

            var container = new AlterTableCommand(tableName);
            container
                .AddColumn("column1")
                .AddColumn("column2")
                .ModifyColumn("column3");

            var result = container.GetQuery(new SqlQueryService());

            Assert.AreEqual("ALTER TABLE [dbo].[posts] ADD [column1] NVARCHAR(255) NOT NULL;\r\rALTER TABLE [dbo].[posts] ADD [column2] NVARCHAR(255) NOT NULL;\r\rALTER TABLE [dbo].[posts] ALTER COLUMN [column3] NVARCHAR(255) NOT NULL;\r", result);
        }


        [TestMethod]
        public void TestGetQuery_WithAddColumnsAndMySQL()
        {
            var tableName = "table1";

            var container = new AlterTableCommand(tableName);
            container
                .AddColumn("column1")
                .AddColumn("column2");

            var result = container.GetQuery(new MySQLQueryService());

            Assert.AreEqual("ALTER TABLE `table1` ADD `column1` VARCHAR(255) COLLATE utf8mb4_unicode_ci NOT NULL;\r\rALTER TABLE `table1` ADD `column2` VARCHAR(255) COLLATE utf8mb4_unicode_ci NOT NULL;\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithModifyAndMySQL()
        {
            var tableName = "table1";

            var container = new AlterTableCommand(tableName);
            container
                .ModifyColumn("column1")
                .ModifyColumn("column2");

            var result = container.GetQuery(new MySQLQueryService());

            Assert.AreEqual("ALTER TABLE `table1` MODIFY COLUMN `column1` VARCHAR(255) COLLATE utf8mb4_unicode_ci NOT NULL;\r\rALTER TABLE `table1` MODIFY COLUMN `column2` VARCHAR(255) COLLATE utf8mb4_unicode_ci NOT NULL;\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithDropAndMySQL()
        {
            var tableName = "table1";

            var container = new AlterTableCommand(tableName);
            container
                .DropColumn("column1")
                .DropColumn("column2");

            var result = container.GetQuery(new MySQLQueryService());

            Assert.AreEqual("ALTER TABLE `table1` DROP COLUMN `column1`;\r\rALTER TABLE `table1` DROP COLUMN `column2`;\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithPrimaryAndMySQL()
        {
            var tableName = "table1";

            var container = new AlterTableCommand(tableName);
            container
                .AddPrimaryKeyConstraint("column1", new string[] { "column1", "column2" });

            var result = container.GetQuery(new MySQLQueryService());

            Assert.AreEqual("ALTER TABLE `column1` ADD PRIMARY KEY (`column1`,`column2`);\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithForeignKeysAndMySQL()
        {
            var tableName = "posts";

            var container = new AlterTableCommand(tableName);
            container
                .AddForeignKeyConstraint("user_id", "users", "id")
                .AddForeignKeyConstraint("category_id", "categories", "id");

            var result = container.GetQuery(new MySQLQueryService());

            Assert.AreEqual("ALTER TABLE `posts` ADD FOREIGN KEY (`user_id`) REFERENCES `users`(`id`);\r\rALTER TABLE `posts` ADD FOREIGN KEY (`category_id`) REFERENCES `categories`(`id`);\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithConstraintsAndMySQL()
        {
            var tableName = "posts";

            var container = new AlterTableCommand(tableName);
            container
                .AddColumn("column1")
                .AddColumn("column2")
                .ModifyColumn("column3");

            var result = container.GetQuery(new MySQLQueryService());

            Assert.AreEqual("ALTER TABLE `posts` ADD `column1` VARCHAR(255) COLLATE utf8mb4_unicode_ci NOT NULL;\r\rALTER TABLE `posts` ADD `column2` VARCHAR(255) COLLATE utf8mb4_unicode_ci NOT NULL;\r\rALTER TABLE `posts` MODIFY COLUMN `column3` VARCHAR(255) COLLATE utf8mb4_unicode_ci NOT NULL;\r", result);
        }
    }
}