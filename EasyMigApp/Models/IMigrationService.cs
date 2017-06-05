﻿using System;
using System.Collections.Generic;
using System.Reflection;
using EasyMigLib.Services;

namespace EasyMigApp.Models
{
    public interface IMigrationService
    {
        void CreateMigrationScript(string assemblyPath, string providerName, string fileName, string engine);
        void CreateSeedScript(string assemblyPath, string providerName, string fileName, string engine);
        void DownAll(string assemblyPath, string connectionString, string providerName, string engine);
        void DownOne(string fileName, string assemblyPath, string connectionString, string providerName, string engine);
        string GetAssemblyDirectory(string assemblyPath);
        List<Type> GetMigrations(Assembly assembly);
        List<Type> GetSeeders(Assembly assembly);
        Assembly LoadFromFile(string path);
        List<RecognizedMigrationFile> ResolveMigrationTypes(string assemblyPath);
        List<RecognizedMigrationFile> ResolveSeederTypes(string assemblyPath);
        void SeedAll(string assemblyPath, string connectionString, string providerName, string engine);
        void SeedOne(string fileName, string assemblyPath, string connectionString, string providerName, string engine);
        void UpAll(string assemblyPath, string connectionString, string providerName, string engine);
        void UpOne(string fileName, string assemblyPath, string connectionString, string providerName, string engine);
    }
}