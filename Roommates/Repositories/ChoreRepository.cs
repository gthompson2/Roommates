using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Roommates.Models;


namespace Roommates.Repositories
{
    /// <summary>
    /// This class interacts with chore data.
    /// It inherits from the BaseRepository class so that it can use the BasRepository's connection property
    /// </summary>
    class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Get a list of all Chores in the database
        /// </summary>
        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                // We have to open the connection manually
                conn.Open();

                // We also have to manually 'activate' commands
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    // Set up the command with the SQL we want to execute
                    cmd.CommandText = "SELECT Id, Name FROM Chore ";

                    // Execute the SQL and get a 'reader' **still working on what the reader does**
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Set up a list to hold the chores we retrieve from the DB
                    List<Chore> chores = new List<Chore>();

                    // If there's still data to read, the statement will return true
                    while (reader.Read())
                    {
                        // GetOrdinal gets the numeric position of the Id column in the query results
                        // We can then use the ordinal value (0 for Id) to get the Id value
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        // Create a new Chore object using the data we just grabbed
                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };

                        // Add the chore object to the list
                        chores.Add(chore);
                    }

                    // Have to close the reader we opened. Don't leave the front door open when you leave the house!
                    reader.Close();

                    // Return the list of chores to whatever called the method
                    return chores;
                }
            }
        }

        // Drop a method for adding a new chore to the database here at some point
    }
}
