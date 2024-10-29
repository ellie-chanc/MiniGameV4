using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGameV4.BoardSetting
{
    internal class Board
    {
        private int initialWidth;
        private int initialHeight;

        public Board()
        {
            initialWidth = GetCurrentWidth();
            initialHeight = GetCurrentHeight();
            Console.Clear();
        }

        public int GetCurrentWidth()
        {
            return Console.WindowWidth - 5;
        }

        public int GetCurrentHeight()
        {
            return Console.WindowHeight - 1 - 4;
        }

        public bool TerminalResized()
        {
            return GetCurrentWidth() != initialWidth || GetCurrentHeight() != initialHeight;
        }
    }
}