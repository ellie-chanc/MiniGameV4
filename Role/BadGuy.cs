using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGameV4.Role
{
    internal class BadGuy : Character
    {
        public BadGuy(int width, int height)
        {
            characterXLimit = width;
            characterYLimit = height;
            characterX = random.Next(0, width);
            characterY = random.Next(0, height);
            currentState = CharacterState.bad;
            ShowCharacter();
        }

        public bool CheckAttacked(int objectX, int objectY)
        {
            for (int i = 0; i < characterLength; i++)
            {
                if (characterY == objectY && characterX == objectX + i)
                {
                    return true;
                }
                for (int j = 0; j < characterLength; j++)
                {
                    if (characterY == objectY && characterX + j == objectX)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // move the badguy in random direction
        public void UpdatePosition(List<BadGuy> c)
        {
            int lastX = characterX;
            int lastY = characterY;
            int initialX = characterX;
            int initialY = characterY;
            bool moveAgain = false;

            do
            {
                if (moveAgain)
                {
                    characterX = initialX;
                    characterY = initialY;
                }

                switch (random.Next(0, 4))
                {
                    case 0:
                        characterY--;
                        break;
                    case 1:
                        characterY++;
                        break;
                    case 2:
                        characterX--;
                        break;
                    case 3:
                        characterX++;
                        break;
                    default:
                        break;
                }
                PositionWithinBounds();
                moveAgain = CheckOverlap(c);
            }
            while (moveAgain);

            ClearCharacter(lastX, lastY);
            ShowCharacter();
        }

        // check if this badguy is overlapping with other badguys
        protected bool CheckOverlap(List<BadGuy> c)
        {
            for (int i = 0; i < c.Count; i++)
            {
                if (c[i] != this)
                {
                    if (c[i].GetCharacterX() == characterX || c[i].GetCharacterY() == characterY)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
