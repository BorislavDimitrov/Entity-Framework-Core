using Microsoft.Data.SqlClient;
using System;

namespace IncreaseAgeStoredProcedure
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection("Server=.;Integrated Security=true;TrustServerCertificate=true;Database=DBMinions"))
            {
                connection.Open();
                var commandToCreateStoredProcedure = new SqlCommand(@"
                                                    CREATE PROCEDURE usp_GetOlder (@id INT)
                                                     AS
                                                     UPDATE Minions
                                                     SET Age += 1
                                                     WHERE Id = @id", connection);
                commandToCreateStoredProcedure.ExecuteNonQuery();
            }
        }
    }
}
