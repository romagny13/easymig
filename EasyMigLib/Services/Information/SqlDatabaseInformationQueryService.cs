﻿using System;

namespace EasyMigLib.Services
{
    public class SqlDatabaseInformationQueryService : DatabaseInformationQueryService
    {
        public override string GetDatabaseExists(string databaseName)
        {
            return "select count(*) from sys.databases where name='" + databaseName + "'";
        }

        public override string GetTableExists(string databaseName, string tableName)
        {
            return "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='" + tableName + "'";
        }

        public override string GetColumnExists(string databaseName, string tableName, string columnName)
        {
            return "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + tableName + "' AND COLUMN_NAME='" + columnName + "'";
        }

        public override string GetTable(string databaseName, string tableName)
        {
            return "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='" + tableName + "'";
        }

        public override string GetColumns(string databaseName, string tableName)
        {
            return "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + tableName + "'";
        }

        public override string GetPrimaryKeys(string databaseName, string tableName)
        {
            return "SELECT c.* FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS as tc, INFORMATION_SCHEMA.KEY_COLUMN_USAGE as kcu,INFORMATION_SCHEMA.COLUMNS as c WHERE tc.CONSTRAINT_TYPE='PRIMARY KEY' and tc.CONSTRAINT_NAME=kcu.CONSTRAINT_NAME AND kcu.COLUMN_NAME=c.COLUMN_NAME AND kcu.TABLE_NAME=tc.TABLE_NAME AND tc.TABLE_NAME=c.TABLE_NAME AND c.TABLE_NAME='" + tableName + "'";
        }

        public override string GetForeignKeys(string databaseName, string tableName)
        {
            return "select o.name as TABLE_NAME, fc.name as COLUMN_NAME, ro.name as TABLE_REFERENCED, c.name as PRIMARY_KEY from sysobjects o join sysforeignkeys fk on fk.fkeyid=o.id join sysobjects ro on ro.id=fk.rkeyid join syscolumns c on c.id=ro.id and c.colid=fk.rkey join syscolumns fc on fc.id=o.id and fc.colid=fk.fkey where o.name='" + tableName + "'";
        }

        public override string GetTableRows(string tableName, int? limit)
        {
            if (limit.HasValue)
            {
                return "SELECT TOP " + limit + " * FROM [" + tableName + "];";
            }
            else
            {
                return "SELECT * FROM [" + tableName + "];";
            }
        }
    }

}