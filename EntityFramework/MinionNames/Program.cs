using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace MinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            int villianId = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection("Server=.;Integrated Security=true;TrustServerCertificate=true;Database=DBMinions"))
            {
                connection.Open();

                var command = new SqlCommand("SELECT Name FROM Villians WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", villianId);

                string name = command.ExecuteScalar() as string;

                if (name == null)
                {
                    Console.WriteLine($"No villian with ID {villianId} exists in the database.");
                    return;
                }

                var minionsCommand = new SqlCommand(@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillians AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillianId = @Id
                                ORDER BY m.Name", connection);
                minionsCommand.Parameters.AddWithValue("@Id",villianId);

                var reader = minionsCommand.ExecuteReader();

                int counter = 0;
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"Villian: {name}");

                while (reader.Read() == true)
                {
                    sb.AppendLine( $"{reader["RowNum"]} {reader["Name"]} {reader["Age"]}");
                    counter++;
                }

                if (counter == 0)
                {
                    Console.WriteLine($"Villian: {name}");
                    Console.WriteLine("(no minions)");
                }
                else
                {
                    Console.WriteLine(sb.ToString());
                }
            }
        }
    }
}
