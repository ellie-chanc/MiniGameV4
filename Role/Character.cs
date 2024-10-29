using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGameV4.Role
{
    internal abstract class Character
    {
        protected const int characterLength = 5;
        protected int characterX;
        protected int characterY;
        protected int characterXLimit;
        protected int characterYLimit;
        protected CharacterState currentState;
        protected Random random = new Random();

        public bool CheckConsumed(int objectX, int objectY)
        {
            for (int i = 0; i < characterLength; i++)
            {
                if (characterY == objectY && characterX + i == objectX)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetCharacterX()
        {
            return characterX;
        }

        public int GetCharacterY()
        {
            return characterY;
        }

        protected void ShowCharacter()
        {
            Console.SetCursorPosition(characterX, characterY);
            Console.Write(StateToFace(currentState));
        }

        protected string StateToFace(CharacterState state)
        {
            string face;
            switch (state)
            {
                case CharacterState.happy:
                    face = "(^_^)";
                    break;
                case CharacterState.excited:
                    face = "(>_<)";
                    break;
                case CharacterState.sick:
                    face = "(X_X)";
                    break;
                case CharacterState.hurt:
                    face = "(ToT)";
                    break;
                case CharacterState.bad:
                    face = "<'o'>";
                    break;
                default:
                    face = "     ";
                    break;
            }
            return face;
        }

        protected void ClearCharacter(int lastX, int lastY)
        {
            // Clear the characters at the previous position
            Console.SetCursorPosition(lastX, lastY);
            for (int i = 0; i < StateToFace(currentState).Length; i++)
            {
                Console.Write(" ");
            }
        }

        protected void PositionWithinBounds()
        {
            // Keep player position within the bounds of the Terminal window
            characterX = characterX < 0 ? 0 : characterX >= characterXLimit ? characterXLimit : characterX;
            characterY = characterY < 0 ? 0 : characterY >= characterYLimit ? characterYLimit : characterY;
        }
    }
}