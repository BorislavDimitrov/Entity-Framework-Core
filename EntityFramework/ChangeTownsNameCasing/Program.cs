using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace ChangeTownsNameCasing
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();

            using (var connection = new SqlConnection("Server=.;Integrated Security=true;TrustServerCertificate=true;Database=DBMinions"))
            {
                connection.Open();
                using var commandToFindCount = new SqlCommand(@"SELECT t.Name 
                                                      FROM Towns as t
                                                      JOIN Countries AS c ON c.Id = t.CountryId
                                                      WHERE c.Name = @countryName", connection);
                commandToFindCount.Parameters.AddWithValue("@countryName", input);
                List<string> towns = new List<string>();

                using (var reader = commandToFindCount.ExecuteReader())
                {
                    while (reader.Read() == true)
                    {
                        towns.Add(reader["Name"] as string);
                    }
                }
                

                if (towns.Count == 0)
                {
                    Console.WriteLine($"No town names were affected");
                    Environment.Exit(0);
                }

                using var commandToUpperNameTowns = new SqlCommand(@"UPDATE Towns
                                                            SET Name = UPPER(Name)
                                                    WHERE CountryId = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)", connection);
                commandToUpperNameTowns.Parameters.AddWithValue("@countryName", input);
                commandToUpperNameTowns.ExecuteNonQuery();

                Console.WriteLine($"{towns.Count} were affected.");
                Console.WriteLine($"[{string.Join(",", towns)}]");
            }
        }
    }
}
