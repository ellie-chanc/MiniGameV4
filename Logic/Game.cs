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
        private bool timeout;
        private Thread countDown;
        private int gameRoundTime = 10;

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

            timeout = false;

            countDown = new Thread(() => StartCountdown(ref gameRoundTime, ref shouldExit, ref timeout));

        }

        public void Run()
        {
            Console.CursorVisible = false;
            countDown.Start();
            while (!shouldExit && !timeout)
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
                            // foodList.Add(new Food(board.GetCurrentWidth(), board.GetCurrentHeight()));
                            dashboard.UpdatePlayerState(p.GetCurrentState());
                            gameRoundTime = 10;
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

            if (timeout)
            {
                Console.Clear();
                Console.WriteLine("Too slow! Game Over. Program exiting...");
                Thread.Sleep(2000);
            }

            if (shouldExit)
            {
                Console.Clear();
                Console.WriteLine("Escape key pressed. Program exiting...");
            }

            countDown.Join();
        }

        private void StartCountdown(ref int sec, ref bool shouldExit, ref bool timeOut)
        {
            while (sec > 0)
            {
                if (shouldExit)
                {
                    return;
                }
                Thread.Sleep(1000);
                sec--;
            }
            timeOut = true;
        }
    }
}
