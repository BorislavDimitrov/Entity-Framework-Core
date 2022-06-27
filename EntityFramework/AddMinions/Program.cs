using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AddMinions
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> minionInfo = Console.ReadLine().Split().ToList();
            string minionName = minionInfo[1];
            int minionAge = int.Parse(minionInfo[2]);
            string minionTown = minionInfo[3];

            List<string> villainInfo = Console.ReadLine().Split().ToList();
            string villainName = villainInfo[1];

            using (var connection = new SqlConnection("Server=.;Integrated Security=true;TrustServerCertificate=true;Database=DBMinions"))
            {
                connection.Open();
                var commandSearchForTown = new SqlCommand("SELECT Id FROM Towns WHERE Name = @townName", connection);
                commandSearchForTown.Parameters.AddWithValue("@townName", minionTown);
                int? townId = (int?)commandSearchForTown.ExecuteScalar();

                if (townId == null)
                {
                    var commandToAddTown = new SqlCommand("INSERT INTO Towns (Name) VALUES (@townName)", connection);
                    commandToAddTown.Parameters.AddWithValue("@townName", minionTown);
                    commandToAddTown.ExecuteNonQuery();
                    Console.WriteLine($"Town {minionTown} was successfuly added to database.");
                }
                townId = (int?)commandSearchForTown.ExecuteScalar();

                var commandToFindVillain = new SqlCommand("SELECT Id FROM Villians WHERE Name = @Name", connection);
                commandToFindVillain.Parameters.AddWithValue("@Name", villainName);

                int? villainId = (int?)commandToFindVillain.ExecuteScalar();

                if (villainId == null)
                {
                    var commandToAddVillain = new SqlCommand("INSERT INTO Villians (Name, EvilnessFactorId)  VALUES (@villainName, 4)", connection);
                    commandToAddVillain.Parameters.AddWithValue("@villainName", villainName);
                    commandToAddVillain.ExecuteNonQuery();
                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }
                villainId = (int)commandToFindVillain.ExecuteScalar();

                var commandToSearchForMinion = new SqlCommand("SELECT Id FROM Minions WHERE Name = @Name", connection);
                commandToSearchForMinion.Parameters.AddWithValue("@Name", minionName);
                int? minionId = (int?)commandToSearchForMinion.ExecuteScalar();

                if (minionId == null)
                {
                    var commandToAddMinion = new SqlCommand("INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)", connection);
                    commandToAddMinion.Parameters.AddWithValue("@nam",minionName);
                    commandToAddMinion.Parameters.AddWithValue("@Age", minionAge);
                    commandToAddMinion.Parameters.AddWithValue("@TownId", townId);
                    commandToAddMinion.ExecuteNonQuery();
                }
                minionId = (int?)commandToSearchForMinion.ExecuteScalar();

                var commandToSetMinionToVillain = new SqlCommand("INSERT INTO MinionsVillians (MinionId, VillianId) VALUES (@minionId, @villainId)", connection);
                commandToSetMinionToVillain.Parameters.AddWithValue("@villainId", villainId);
                commandToSetMinionToVillain.Parameters.AddWithValue("@minionId", minionId);
                commandToSetMinionToVillain.ExecuteNonQuery();

                Console.WriteLine($"Succesfuly added {minionName} to be minion of {villainName}");
            }
        }
    }
}
