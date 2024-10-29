using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGameV4.Exceptions
{
    internal class SpecialCharacterException : Exception
    {
        public SpecialCharacterException(string message) : base(message) { }
    }
}
