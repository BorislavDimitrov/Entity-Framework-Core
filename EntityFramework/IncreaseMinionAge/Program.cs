using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IncreaseMinionAge
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> input = Console.ReadLine().Split().Select(int.Parse).ToList();

            using (var connection = new SqlConnection("Server=.;Integrated Security=true;TrustServerCertificate=true;Database=DBMinions"))
            {
                connection.Open();

                for (int i = 0; i < input.Count; i++)
                {
                    var commandToUpdateAges = new SqlCommand(@" UPDATE Minions
                                            SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                            WHERE Id = @Id", connection);
                    commandToUpdateAges.Parameters.AddWithValue("@Id",input[i]);
                    commandToUpdateAges.ExecuteNonQuery();
                }

                var commandToTakeAllMinions = new SqlCommand("SELECT Name, Age FROM Minions", connection);
                using (var reader = commandToTakeAllMinions.ExecuteReader())
                {
                    while (reader.Read() == true)
                    {
                        Console.WriteLine($"{reader["Name"]} {reader["Age"]}");
                    }
                }
            }
        }
    }
}
