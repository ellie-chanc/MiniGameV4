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
using System.Net.Http.Headers;

namespace MiniGameV4.Record
{
    internal class Dashboard
    {
        private UserAccount userAcc;
        private string playerName;
        private int dashboardYUpperLimit;
        private int dashboardXLimit;
        private const int dashboardHeight = 3;
        private int consumedFoodNumber;
        private CharacterState playerState;
        private string connectionStr = "Data Source=L-6BKGP34;Initial Catalog=MiniGameV4;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";

        public Dashboard(int height, int width)
        {
            userAcc = new UserAccount();
            playerName = userAcc.GetUserName();
            dashboardYUpperLimit = height + 1;
            dashboardXLimit = width;
            consumedFoodNumber = 0;
            PrintDashboard();
        }

        public void UpdateConsumedFood()
        {
            consumedFoodNumber++;

            // update database
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandText = "UPDATE GameRecord SET FoodConsumed = @FoodConsumed WHERE GameRecordKey = @GameRecordKey;";

                    SqlParameter FoodConsumedParam = new SqlParameter("FoodConsumed", System.Data.SqlDbType.Int);
                    FoodConsumedParam.Value = consumedFoodNumber;
                    command.Parameters.Add(FoodConsumedParam);

                    SqlParameter GameRecordKeyParam = new SqlParameter("GameRecordKey", System.Data.SqlDbType.Int);
                    GameRecordKeyParam.Value = GetGameRecordKey();
                    command.Parameters.Add(GameRecordKeyParam);

                    con.Open();
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

        private int GetGameRecordKey()
        {
            int key = 0;

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandText = "SELECT GameRecordKey FROM GameRecord WHERE PlayerKey = @PlayerKey;";
                    command.Parameters.AddWithValue("@PlayerKey", userAcc.GetPlayerKey());
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            key = (int)reader["GameRecordKey"];
                        }
                    }
                }
            }

            return key;
        }
    }
}