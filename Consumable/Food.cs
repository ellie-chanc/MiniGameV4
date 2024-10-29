using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGameV4.Consumable
{
    internal class Food
    {
        private Random random = new Random();
        private int foodX;
        private int foodY;
        private int foodXLimit;
        private int foodYLimit;
        private FoodType currentType;

        // constructor sets food initial position
        public Food(int width, int height)
        {
            foodXLimit = width;
            foodYLimit = height;
            UpdateFood();
        }

        // update food type and position
        public void UpdateFood()
        {
            // update food to a random food
            currentType = (FoodType)random.Next(0, Enum.GetNames(typeof(FoodType)).Length);

            // update food position to a random location 
            foodX = random.Next(0, foodXLimit);
            foodY = random.Next(0, foodYLimit);
            ShowFood();
        }

        private string TypeToFood(FoodType type)
        {
            string food;
            switch (type)
            {
                case FoodType.ham:
                    food = "@";
                    break;
                case FoodType.money:
                    food = "$";
                    break;
                case FoodType.grass:
                    food = "#";
                    break;
                default:
                    food = " ";
                    break;
            }
            return food;
        }

        // show food on console
        private void ShowFood()
        {
            Console.SetCursorPosition(foodX, foodY);
            Console.Write(TypeToFood(currentType));
        }

        public int GetFoodX()
        {
            return foodX;
        }

        public int GetFoodY()
        {
            return foodY;
        }

        public FoodType GetCurrentFoodType()
        {
            return currentType;
        }
    }
}