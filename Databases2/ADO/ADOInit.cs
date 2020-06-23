using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Databases2.ADO
{
    class ADOInit
    {
        private readonly string serverConnString = @"Server=DESKTOP-JI4MNRR\SQLEXPRESS;
            Integrated Security=true";

        private readonly string dbConnString = @"Server=DESKTOP-JI4MNRR\SQLEXPRESS;
            Initial Catalog=Netflox;
            Integrated Security=true";

        public void Initialize()
        {
            CreateDB();
            CreateSchemaAndTables();
        }

        public string GetDbConnString()
        {
            return dbConnString;
        }

        private void CreateDB()
        {
            DeleteDB();
            using (var conn = new SqlConnection(serverConnString))
            {
                conn.Open();
                string query = "CREATE DATABASE [Netflox]";
                SqlCommand command = new SqlCommand(query, conn);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void DeleteDB()
        {
            using (var conn = new SqlConnection(serverConnString))
            {
                conn.Open();
                string query = "DROP DATABASE [Netflox]";
                SqlCommand command = new SqlCommand(query, conn);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void CreateSchemaAndTables()
        {
            using (var conn = new SqlConnection(dbConnString))
            {
                conn.Open();
                string queryString;
                // Read the sql file with the create tables querys
                using (FileStream fileStream = File.OpenRead(@"Resources\SQL\CreateTables.sql"))
                {
                    StreamReader reader = new StreamReader(fileStream);
                    queryString = reader.ReadToEnd();
                }

                SqlCommand createSchemaCommand = new SqlCommand("CREATE SCHEMA [user_info]", conn);
                SqlCommand createTablesCommand = new SqlCommand(queryString, conn);

                try
                {
                    Console.WriteLine("Creating schema's");
                    createSchemaCommand.ExecuteScalar();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                try
                {
                    Console.WriteLine("Create Tables");
                    createTablesCommand.ExecuteScalar();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Tables already exists");
                }
            }
        }
    }
}
