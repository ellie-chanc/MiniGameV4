using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniGameV4.Consumable;

namespace MiniGameV4.Role
{
    internal class Player : Character
    {
        // constructor sets player initial position
        public Player(int width, int height)
        {
            characterX = 0;
            characterY = 0;
            characterXLimit = width;
            characterYLimit = height;
            currentState = CharacterState.happy;
            ShowCharacter();
        }

        // update player position and return shouldExit or not
        public bool UpdatePosition()
        {
            int lastX = characterX;
            int lastY = characterY;
            bool shouldExit = false;

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    characterY--;
                    break;
                case ConsoleKey.DownArrow:
                    characterY++;
                    break;
                case ConsoleKey.LeftArrow:
                    characterX--;
                    break;
                case ConsoleKey.RightArrow:
                    characterX++;
                    break;
                case ConsoleKey.Escape:
                    shouldExit = true;
                    return shouldExit;
                default:
                    break;
            }

            ClearCharacter(lastX, lastY);
            PositionWithinBounds();
            ShowCharacter();

            return shouldExit;
        }

        public void ChangeState(FoodType currentFoodTypes)
        {
            currentState = (CharacterState)(int)currentFoodTypes;
            ShowCharacter();
        }

        public void Freeze()
        {
            Thread.Sleep(2000);
            currentState = 0;
            ShowCharacter();
        }

        public void Hurt()
        {
            currentState = CharacterState.hurt;
            ShowCharacter();
            Thread.Sleep(2000);
        }

        public CharacterState GetCurrentState()
        {
            return currentState;
        }
    }
}
