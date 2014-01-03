using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseConnection.Databases;

namespace DatabaseClient.Routines
{
    /// <summary>
    /// Base class for all client functionalities
    /// </summary>
    abstract class RoutineBase
    {
        protected DatabaseManager databaseManager;
        protected Database selectedDatabase;

        public RoutineBase(DatabaseManager databaseManager, Database database)
        {
            this.databaseManager = databaseManager;
            this.selectedDatabase = database;

            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnTerminate);
        }

        public RoutineBase(Database database)
            : this(null, database)
        {
        }

        public RoutineBase(DatabaseManager databaseManager)
            : this(databaseManager, null)
        {
        }

        /// <summary>
        /// Base implementation of Run() writes empty line, 
        /// prompts user ro press button and exits after clearing screen
        /// </summary>
        /// <returns>Itself</returns>
        public virtual RoutineBase Run()
        {
            Console.WriteLine();
            Console.Write("Press any key to return");
            Console.ReadKey();
            Console.Clear();
            return this;
        }

        /// <summary>
        /// Called when user presses CTRL+C to terminate
        /// </summary>
        protected virtual void OnTerminate(object sender, ConsoleCancelEventArgs args)
        {
        }
    }
}
