using System.Timers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniGameV4.BoardSetting;
using MiniGameV4.Role;
using MiniGameV4.Consumable;
using MiniGameV4.Record;

namespace MiniGameV4.Logic
{
    internal class Game
    {
        private Board board;
        private Player p;
        private const int initialBadGuyNumber = 3;
        private List<BadGuy> badGuyList;
        private List<Food> foodList;
        private Dashboard dashboard;
        private bool shouldExit;
        private bool roundTimeout;
        private bool gameTimeout;
        private Thread countDown;
        private int roundTime = 10;  // 10 seconds for each round
        private int gameTime = 20;  // 20 seconds for the game

        // constructor for initialising game
        public Game()
        {
            // create empty board
            board = new Board();

            // create and display dashboard
            dashboard = new Dashboard(board.GetCurrentHeight(), board.GetCurrentWidth());

            // create and display player
            p = new Player(board.GetCurrentWidth(), board.GetCurrentHeight());

            // create and display badguy
            badGuyList = new List<BadGuy>();
            for (int i = 0; i < initialBadGuyNumber; i++)
            {
                badGuyList.Add(new BadGuy(board.GetCurrentWidth(), board.GetCurrentHeight()));
            }

            // create and display food
            foodList = new List<Food>();
            foodList.Add(new Food(board.GetCurrentWidth(), board.GetCurrentHeight()));

            shouldExit = false;

            roundTimeout = false;

            gameTimeout = false;

            countDown = new Thread(() => StartCountdown(ref gameTime, ref roundTime, ref shouldExit, ref gameTimeout, ref roundTimeout));

        }

        public void Run()
        {
            Console.CursorVisible = false;
            countDown.Start();
            while (!shouldExit && !roundTimeout && !gameTimeout)
            {
                if (board.TerminalResized())
                {
                    Console.Clear();
                    Console.WriteLine("Console was resized. Program exiting...");
                    shouldExit = true;
                }
                else
                {
                    // move player
                    shouldExit = p.UpdatePosition();

                    // move all badguy
                    for (int i = 0; i < badGuyList.Count; ++i)
                    {
                        badGuyList[i].UpdatePosition(badGuyList);
                    }

                    for (int i = 0; i < badGuyList.Count; i++)
                    {
                        // if badguy attacked the player
                        if (badGuyList[i].CheckAttacked(p.GetCharacterX(), p.GetCharacterY()) && (p.GetCurrentState() != CharacterState.hurt))
                        {
                            p.Hurt();
                            dashboard.UpdatePlayerState(p.GetCurrentState());
                            badGuyList.Add(new BadGuy(board.GetCurrentWidth(), board.GetCurrentHeight()));
                        }
                        // if badguy consumed the food
                        for (int j = 0; j < foodList.Count; ++j)
                        {
                            if (badGuyList[i].CheckConsumed(foodList[j].GetFoodX(), foodList[j].GetFoodY()))
                            {
                                foodList[j].UpdateFood();
                            }
                        }
                    }

                    // if player consumed the food
                    for (int i = 0; i < foodList.Count; ++i)
                    {
                        if (p.CheckConsumed(foodList[i].GetFoodX(), foodList[i].GetFoodY()))
                        {
                            dashboard.UpdateConsumedFood();
                            p.ChangeState(foodList[i].GetCurrentFoodType());
                            foodList[i].UpdateFood();
                            dashboard.UpdatePlayerState(p.GetCurrentState());
                            roundTime = 10;
                        }
                    }

                    // freeze sick player
                    if (p.GetCurrentState() == CharacterState.sick)
                    {
                        p.Freeze();
                        dashboard.UpdatePlayerState(p.GetCurrentState());
                    }
                }
            }

            if (roundTimeout)
            {
                Console.Clear();
                Console.WriteLine("Too slow! Game Over. Program exiting...");
                Thread.Sleep(2000);
            }
            else if (gameTimeout)
            {
                Console.Clear();
                Console.WriteLine("Congratulations! This is the end of the game. Program exiting...");
                Thread.Sleep(2000);
            }
            else if (shouldExit)
            {
                Console.Clear();
                Console.WriteLine("Escape key pressed. Program exiting...");
            }

            countDown.Join();
        }

        private void StartCountdown(ref int gameTime, ref int roundTime, ref bool shouldExit, ref bool gameTimeout, ref bool roundTimeout)
        {
            while (gameTime > 0)
            {
                if (shouldExit)
                {
                    return;
                }

                if (roundTime <= 0)
                {
                    roundTimeout = true;
                    return;
                }

                Thread.Sleep(1000);
                roundTime--;
                gameTime--;
            }
            gameTimeout = true;
        }
    }
}
