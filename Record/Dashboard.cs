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
using MiniGameV4.Data;
using MiniGameV4.Model;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Versioning;

namespace MiniGameV4.Record
{
    internal class Dashboard
    {
        private UserAccount userAcc;
        private string playerName;
        private GameStatus gameStatus;
        private int dashboardYUpperLimit;
        private int dashboardXLimit;
        private const int dashboardHeight = 3;
        private int consumedFoodNumber;
        private CharacterState playerState;
        private GameContext gameContext;

        public Dashboard(int height, int width)
        {
            PrintIntro();
            gameContext = new GameContext();
            userAcc = new UserAccount(gameContext);
            playerName = userAcc.GetUsername();
            gameStatus = new GameStatus(gameContext, userAcc.GetUserId());
            dashboardYUpperLimit = height + 1;
            dashboardXLimit = width;
            consumedFoodNumber = 0;
            Console.Clear();
            PrintDashboard();
        }

        public void UpdateConsumedFood()
        {
            consumedFoodNumber++;

            // update database
            gameStatus.UpdateConsumedFoodNumber();

            PrintDashboard();
        }

        public void UpdatePlayerState(CharacterState state)
        {
            playerState = state;
            PrintDashboard();
        }

        public void PrintSummary()
        {
            Console.WriteLine("Summary: ");
            Console.WriteLine($"{playerName} has consumed {consumedFoodNumber} food.");

            var topPlayers = gameContext.GameRecord
                .Join
                (
                    gameContext.User, 
                    g => g.UserId, 
                    u => u.UserId, 
                    (g, u) => new 
                    {
                        g.FoodConsumed, 
                        u.Username,
                        u.FirstName,
                        u.LastName,
                    }
                )
                .OrderByDescending(x => x.FoodConsumed)
                .Take(10)
                .ToList();

            // list top ten players with higest number of food items consumed
            Console.WriteLine("\nTop ten records: ");
            Console.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20} {4, -20}", "Rank", "Highest score", "Username", "First Name", "Last name");

            for (int i = 0; i < topPlayers.Count(); i++)
            {
                Console.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20} {4, -20}", i + 1, topPlayers[i].FoodConsumed, topPlayers[i].Username, topPlayers[i].FirstName, topPlayers[i].LastName);
            }
        }

        private void PrintIntro()
        {
            Console.Clear();
            Console.WriteLine("Introduction: ");
            Console.WriteLine("You are feeling really hungry right now and just wants something to eat!");
            Console.WriteLine("You have to eat a food item { # $ @ } within 10 seconds.");
            Console.WriteLine("Be careful, you will get hurt if you collide with a Bad Guy <'o'>");
            Console.WriteLine("Press [Enter] to continue");
            Console.ReadLine();
            Console.Clear();
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
    }
}