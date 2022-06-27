using Microsoft.Data.SqlClient;
using System;

namespace VilliansNames
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection("Server=.;Integrated Security=true;TrustServerCertificate=true;Database=DBMinions"))
            {
                connection.Open();

                var command = new SqlCommand("SELECT v.Name, COUNT(*) AS [MinionsCount]FROM MinionsVillians AS mv JOIN Villians AS v ON v.Id = mv.VillianId " +
                    "GROUP BY mv.VillianId, v.Name HAVING COUNT(*) > 1", connection);
                var reader = command.ExecuteReader();

                while (reader.Read() == true)
                {
                    Console.Write(reader["Name"] + " ");
                    Console.WriteLine(reader["MinionsCount"]);
                }
            }
        }
    }
}
