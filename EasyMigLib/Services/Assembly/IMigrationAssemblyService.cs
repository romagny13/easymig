﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyMigLib.Services
{
    public interface IMigrationAssemblyService
    {
        T CreateInstance<T>(Type type);
        RecognizedMigrationFile FindType<T>(string assemblyPath, string matchName);
        RecognizedMigrationFile FindType<T>(Type[] assemblyTypes, string matchName);
        List<RecognizedMigrationFile> FindTypes<T>(string assemblyPath);
        List<RecognizedMigrationFile> FindTypes<T>(Type[] assemblyTypes);
        RecognizedMigrationFile GetRecognizedMigrationFile(Type type);
        Assembly LoadAssembly(string path);
        void RunMigration(RecognizedMigrationFile recognizedType, MigrationDirection direction = MigrationDirection.Up);
        void RunSeeder(RecognizedMigrationFile recognizedType);
        void SortTypes(List<RecognizedMigrationFile> types);
        Dictionary<string, List<RecognizedMigrationFile>> Group(List<RecognizedMigrationFile> orderedList);
    }
}