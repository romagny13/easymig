﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyMigLib.Commands;
using EasyMigLib;
using EasyMigLib.Services;

namespace EasyMigLibTest.Commands
{
    [TestClass]
    public class DropDatabaseCommandTest
    {
        [TestMethod]
        public void TestGetQuery_WithSql()
        {
            var command = new DropDatabaseCommand("db1");
            var result = command.GetQuery(new SqlQueryService());

            Assert.AreEqual("DROP DATABASE IF EXISTS [db1];\r", result);
        }

        [TestMethod]
        public void TestGetQuery_WithMySQL()
        {
            var command = new DropDatabaseCommand("db1");
            var result = command.GetQuery(new MySQLQueryService());

            Assert.AreEqual("DROP DATABASE IF EXISTS `db1`;\r", result);
        }
    }
}
