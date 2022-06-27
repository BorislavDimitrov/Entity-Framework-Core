using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace PrintAllMinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection("Server=.;Integrated Security=true;TrustServerCertificate=true;Database=DBMinions"))
            {
                connection.Open();
                var commandToTakeAllMinionNames = new SqlCommand("SELECT Name FROM Minions", connection);
                List<string> names = new List<string>();

                using (var reader = commandToTakeAllMinionNames.ExecuteReader())
                {
                    while (reader.Read() == true)
                    {
                        names.Add(reader["Name"] as string);
                    }
                }

                int namesCount = names.Count;

                Console.WriteLine(names[0]);
                Console.WriteLine(names[names.Count - 1]);

                namesCount -= 2;
                for (int i = 1; i < names.Count; i++)
                {
                    if (namesCount == 0)
                    {
                        break;
                    }
                    Console.WriteLine(names[0 + i]);
                    namesCount--;
                    if (namesCount == 0)
                    {
                        break;
                    }
                    Console.WriteLine(names[(names.Count - 1) - i]);
                    namesCount--;
                    if (namesCount == 0)
                    {
                        break;
                    }
                }
            }
        }
    }
}
