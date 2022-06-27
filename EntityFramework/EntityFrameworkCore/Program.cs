using Microsoft.Data.SqlClient;
using System;

namespace EntityFrameworkCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var createDBConnection = new SqlConnection("Server=.;Integrated Security=true;TrustServerCertificate=true;Database=master"))
            {

                createDBConnection.Open();

                var commandCreateDB = new SqlCommand("CREATE DATABASE DBMinions", createDBConnection);
                commandCreateDB.ExecuteNonQuery();
            }

            using (var connection = new SqlConnection("Server=.;Integrated Security=true;TrustServerCertificate=true;Database=DBMinions"))
            {
                connection.Open();
                var commandCreateTableCountries = new SqlCommand("CREATE TABLE Countries " +
                                                                        "(Id INT PRIMARY KEY IDENTITY," +
                                                                "Name VARCHAR(50))", connection);
                commandCreateTableCountries.ExecuteNonQuery();
                var commandToInsertIntoTableCountries = new SqlCommand("INSERT INTO Countries VALUES ('Bulgaria'), ('Germany'), ('UK'), ('Turkey'), ('Greece')", connection);
                commandToInsertIntoTableCountries.ExecuteNonQuery();


                var commandCreateTableTowns = new SqlCommand("CREATE TABLE Towns " +
                                                               "(Id INT PRIMARY KEY IDENTITY" +
                                                              ",Name VARCHAR(100)," +
                                                              "CountryId INT REFERENCES Countries(Id))", connection);
                commandCreateTableTowns.ExecuteNonQuery();
                var commandToInsertIntoTableTowns = new SqlCommand("INSERT INTO Towns VALUES ('Stara Zagora',1 ),('Berlin', 2),('London',3),('Istanbul',4),('Athens', 5)", connection);
                commandToInsertIntoTableTowns.ExecuteNonQuery();


                var commandCreateTableEvilnessFactors = new SqlCommand("CREATE TABLE EvilnessFactors" +
                                                            "(Id INT PRIMARY KEY IDENTITY," +
                                                            "Name VARCHAR(20))", connection);
                commandCreateTableEvilnessFactors.ExecuteNonQuery();
                var commandToInsertIntoTableEvilnessFactors = new SqlCommand("INSERT INTO EvilnessFactors VALUES ('Super good'), ('Good'),('Bad'),('Evil'), ('Super evil')", connection);
                commandToInsertIntoTableEvilnessFactors.ExecuteNonQuery();
                

                var commandCreateTaleVillians = new SqlCommand("CREATE TABLE Villians" +
                                                                "(Id INT PRIMARY KEY IDENTITY," +
                                                                "Name VARCHAR(100)," +
                                                                "EvilnessFactorId INT REFERENCES EvilnessFactors(Id))", connection);
                commandCreateTaleVillians.ExecuteNonQuery();
                var commandToInsertIntoVillians = new SqlCommand("INSERT INTO Villians VALUES ('Gru', 1),('Victor', 2),('Jiri', 3),('Corki', 4),('Bill', 5)", connection);
                commandToInsertIntoVillians.ExecuteNonQuery();



                var commandCreateTableMinions = new SqlCommand("CREATE TABLE Minions" +
                                                               "( Id INT PRIMARY KEY IDENTITY," +
                                                               "Name VARCHAR(100)," +
                                                               "Age INT ," +
                                                               "TownId INT REFERENCES Towns(Id))", connection);
                commandCreateTableMinions.ExecuteNonQuery();
                var commandToInsertIntoTableMinions = new SqlCommand("INSERT INTO Minions VALUES ('Pier', 34,1), ('Kevin', 24,2), ('Rob', 43,3), ('Stivie', 22,4), ('John', 23,5)", connection);
                commandToInsertIntoTableMinions.ExecuteNonQuery();

                var commandCreateTableMinionsVillians = new SqlCommand("CREATE TABLE MinionsVillians" +
                                                                        "(MinionId INT REFERENCES Minions(Id)," +
                                                                        "VillianId INT REFERENCES Villians(Id)," +
                                                                        "PRIMARY KEY (MinionId, VillianId))", connection);
                commandCreateTableMinionsVillians.ExecuteNonQuery();
                var commandToInsertIntoTableMinionsVillians = new SqlCommand("INSERT INTO MinionsVillians VALUES (1,1),(1,2),(1,3),(2,1),(2,2),(2,3),(3,1),(3,4),(4,4),(5,5)", connection);
                commandToInsertIntoTableMinionsVillians.ExecuteNonQuery();
            }
            
            
            
        }
    }
}
