using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseConnection.Databases;
using BackupEngine;
using DatabaseConnection;
using DatabaseClient.Routines;

namespace DatabaseClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Database database = new Database("198.24.160.66", "8959_convoy", "8959_convoy", "convoyqwe123");
            //Database database = new Database("az4.volt-host.com", "mick88_website", "mick88_mick88", "c0s1k1q2w3e4r5t6y7u8i9o");

            DatabaseManager manager = new DatabaseManager().LoadDatabases();
            if (args.Length > 0)
            {
                new ArgProcess(manager, args).Run();
            }
            else
            {
                new Main(manager).Run();
            }
        }
    }
}
