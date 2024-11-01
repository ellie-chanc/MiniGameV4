using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MiniGameV4.Exceptions;

namespace MiniGameV4.Record
{
    class UserAccount
    {
        private string userName;
        private string connectionStr = "Data Source=L-6BKGP34;Initial Catalog=MiniGameV4;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";

        public UserAccount()
        {
            userName = SetUserName();

            // create new player if not already existed in database
            if (!CheckExistingPlayer())
            {
                using (SqlConnection con = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        // set command properties
                        command.Connection = con;
                        command.CommandText = "INSERT INTO Player(Name) VALUES (@Name);";

                        // set parameters of command
                        SqlParameter nameParam = new SqlParameter("Name", System.Data.SqlDbType.NVarChar);
                        nameParam.Value = userName;
                        command.Parameters.Add(nameParam);

                        // open connection and execute query
                        con.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }

            // create new game record in database
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandText = "INSERT INTO GameRecord(PlayerKey, FoodConsumed) VALUES(@PlayerKey, @FoodConsumed);";

                    SqlParameter PlayerKeyParam = new SqlParameter("PlayerKey", System.Data.SqlDbType.Int);
                    PlayerKeyParam.Value = GetPlayerKey();
                    command.Parameters.Add(PlayerKeyParam);

                    SqlParameter FoodConsumedParam = new SqlParameter("FoodConsumed", System.Data.SqlDbType.Int);
                    FoodConsumedParam.Value = 0;
                    command.Parameters.Add(FoodConsumedParam);

                    con.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private string SetUserName()
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

        public int GetPlayerKey()
        {
            int key = 0;

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandText = "SELECT PlayerKey FROM Player WHERE Name = @name;";
                    command.Parameters.AddWithValue("@name", userName);
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            key = (int)reader["PlayerKey"];
                        }
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
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandText = "SELECT Name FROM Player WHERE Name = @name;";
                    command.Parameters.AddWithValue("@name", userName);
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())  // bool if it could read any data from database
                        {
                            if ((string)reader["Name"] == userName)
                            {
                                exist = true;
                            }
                        }
                    }
                }
            }

            return exist;
        }

        public string GetUserName()
        {
            return userName;
        }
    }
}
