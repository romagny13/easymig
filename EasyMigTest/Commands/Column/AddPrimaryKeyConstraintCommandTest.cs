﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyMigLib.Commands;
using EasyMigLib.Query.SqlClient;
using EasyMigLib.Query.MySqlClient;

namespace EasyMigLibTest.Commands
{
    [TestClass]
    public class AddPrimaryKeyConstraintCommandTest
    {
        [TestMethod]
        public void TestGetQuery_WithSql()
        {
            var keys = new string[] { "column1" };
            var command = new AddPrimaryKeyConstraintCommand("table1", keys);
            var result = command.GetQuery(new SqlQueryService());

            Assert.AreEqual("ALTER TABLE [dbo].[table1] ADD PRIMARY KEY ([column1])\rGO\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithColumnsAndSql()
        {
            var keys = new string[] { "column1", "column2" };
            var command = new AddPrimaryKeyConstraintCommand("table1", keys);
            var result = command.GetQuery(new SqlQueryService());

            Assert.AreEqual("ALTER TABLE [dbo].[table1] ADD PRIMARY KEY ([column1],[column2])\rGO\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithMySQL()
        {
            var keys = new string[] { "column1" };
            var command = new AddPrimaryKeyConstraintCommand("table1", keys);
            var result = command.GetQuery(new MySqlQueryService());

            Assert.AreEqual("ALTER TABLE `table1` ADD PRIMARY KEY (`column1`);\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithColumnsAndMySQL()
        {
            var keys = new string[] { "column1", "column2" };
            var command = new AddPrimaryKeyConstraintCommand("table1", keys);
            var result = command.GetQuery(new MySqlQueryService());

            Assert.AreEqual("ALTER TABLE `table1` ADD PRIMARY KEY (`column1`,`column2`);\r", result);
        }
    }
}
