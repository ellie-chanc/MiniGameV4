using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MiniGameV4.Exceptions;
using MiniGameV4.Role;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace MiniGameV4.Record
{
    internal class Dashboard
    {
        private string playerName;
        private int dashboardYUpperLimit;
        private int dashboardXLimit;
        private const int dashboardHeight = 3;
        private int consumedFoodNumber;
        private CharacterState playerState;
        private string connectionStr = "Data Source=(local); Initial Catalog=MiniGameV4; Integrated Security=SSPI; Encrypt=false";

        public Dashboard(int height, int width)
        {
            dashboardYUpperLimit = height + 1;
            dashboardXLimit = width;
            consumedFoodNumber = 0;
            playerName = SetPlayerName();

            // create new player if not already existed in database
            if (!CheckExistingPlayer())
            {
                using (SqlConnection con = new SqlConnection(connectionStr))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = con;
                        command.CommandText = "INSERT INTO Player(Name) VALUES (@Name);";
                        command.Parameters.AddWithValue("@Name", playerName);
                        command.ExecuteNonQuery();
                    }
                }
            }

            // create new game record in database
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandText = "INSERT INTO GameRecord(PlayerKey, FoodConsumed) VALUES(@PlayerKey, @FoodConsumed);";
                    command.Parameters.AddWithValue("@PlayerKey", GetPlayerKey());
                    command.Parameters.AddWithValue("@FoodConsumed", 0);
                    command.ExecuteNonQuery();
                }
            }
            
            PrintDashboard();
        }

        public void UpdateConsumedFood()
        {
            consumedFoodNumber++;

            // update database
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandText = "UPDATE GameRecord SET FoodConsumed = @FoodConsumed WHERE GameRecordKey = @GameRecordKey;";
                    command.Parameters.AddWithValue("@FoodConsumed", consumedFoodNumber);
                    command.Parameters.AddWithValue("@GameRecordKey", GetGameRecordKey());
                    command.ExecuteNonQuery();
                }
            }

            PrintDashboard();
        }

        public void UpdatePlayerState(CharacterState state)
        {
            playerState = state;
            PrintDashboard();
        }

        private string SetPlayerName()
        {
            string? name;
            string pattern = @"^[a-zA-Z0-9]*$";
            Regex re = new Regex(pattern);

            Console.SetCursorPosition(0, 0);
            while (true)
            {
                Console.Write("Player's name: ");
                try
                {
                    name = Console.ReadLine();

                    if (string.IsNullOrEmpty(name))
                    {
                        throw new ArgumentException("Input cannot be null or empty.");
                    }

                    if (!re.IsMatch(name))
                    {
                        throw new SpecialCharacterException("Input cannot contain special characters.");
                    }

                    // when no exceptions are thrown
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Invalid input: " + ex.Message);
                }
                catch (SpecialCharacterException ex)
                {
                    Console.WriteLine("Invalid input: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid input: " + ex.Message);
                }
            }

            Console.Clear();
            return name;
        }

        private void PrintDashboard()
        {
            ClearDashboard();
            Console.SetCursorPosition(0, dashboardYUpperLimit);
            Console.WriteLine($"{playerName} is {playerState}!");
            Console.WriteLine($"{playerName} has consumed {consumedFoodNumber} food.");
            Console.WriteLine($"Please move with the arrow keys. You have 10 seconds to consume each food.");
        }

        private void ClearDashboard()
        {
            Console.SetCursorPosition(0, dashboardYUpperLimit);
            for (int i = 0; i < dashboardHeight; i++)
            {
                for (int j = 0; j < dashboardXLimit; j++)
                {
                    Console.Write(" ");
                }
            }

        }

        private int GetPlayerKey()
        {
            int key = 0;

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandText = "SELECT PlayerKey FROM Player WHERE Name = @name;";
                    command.Parameters.AddWithValue("@name", playerName);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        key = (int)reader["PlayerKey"];
                    }
                }
            }

            return key;
        }

        private int GetGameRecordKey()
        {
            int key = 0;

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandText = "SELECT GameRecordKey FROM GameRecord WHERE PlayerKey = @PlayerKey;";
                    command.Parameters.AddWithValue("@PlayerKey", GetPlayerKey());
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        key = (int)reader["GameRecordKey"];
                    }
                }
            }

            return key;
        }

        private bool CheckExistingPlayer()
        {
            bool exist = false;

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandText = "SELECT Name FROM Player WHERE Name = @name;";
                    command.Parameters.AddWithValue("@name", playerName);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if ((string)reader["Name"] == playerName)
                        {

                            exist = true;
                        }
                    }
                }
            }

            return exist;
        }
    }
}