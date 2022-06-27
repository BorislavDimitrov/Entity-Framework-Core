using Microsoft.Data.SqlClient;
using System;

namespace RemoveVillain
{
    class Program
    {
        static void Main(string[] args)
        {
            int input = int.Parse(Console.ReadLine());
            using (var connection = new SqlConnection("Server=.;Integrated Security=true;TrustServerCertificate=true;Database=DBMinions"))
            {
                connection.Open();
                var commandToGetVillainName = new SqlCommand("SELECT Name FROM Villians WHERE Id = @villainId", connection);
                commandToGetVillainName.Parameters.AddWithValue("@villainId", input);
                string villainname = commandToGetVillainName.ExecuteScalar() as string;

                if (villainname == null)
                {
                    Console.WriteLine("No such villain was found.");
                    Environment.Exit(0);
                }

                var commandToDeleteMinionsVillain = new SqlCommand("DELETE FROM MinionsVillians WHERE VillianId = @villainId", connection);
                commandToDeleteMinionsVillain.Parameters.AddWithValue("@villainId",input);
                int count = commandToDeleteMinionsVillain.ExecuteNonQuery();

                var commandToDeleteVillain = new SqlCommand("DELETE FROM Villians WHERE Id = @villainId", connection);
                commandToDeleteVillain.Parameters.AddWithValue("@villainId", input);
                commandToDeleteVillain.ExecuteNonQuery();

                Console.WriteLine($"{villainname} was deleted.");
                Console.WriteLine($"{count} minions were released.");
            }
        }
    }
}
