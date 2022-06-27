using Microsoft.Data.SqlClient;
using System;

namespace UsingStoredProcedureIncreaseAge
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            using (var connection = new SqlConnection("Server=.;Integrated Security=true;TrustServerCertificate=true;Database=DBMinions"))
            {
                connection.Open();

                var commandToUseStoredProcedure = new SqlCommand("EXEC dbo.usp_GetOlder @id", connection);
                commandToUseStoredProcedure.Parameters.AddWithValue("@id", input);
                commandToUseStoredProcedure.ExecuteNonQuery();

                var commandToCollectTheCertainMininon = new SqlCommand("SELECT Name, Age FROM Minions WHERE Id = @Id",connection);
                commandToCollectTheCertainMininon.Parameters.AddWithValue("@Id", input);
                var reader = commandToCollectTheCertainMininon.ExecuteReader();
                using (reader)
                {
                    reader.Read();
                    Console.WriteLine($"{reader["Name"]} {reader["Age"]} years old.");
                }                          
            }
        }
    }
}
