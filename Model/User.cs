using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGameV4.Model
{
    internal class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // collection navigation containing dependents
        // a user can contain multiple GameRecords
        public ICollection<GameRecord> GameRecords { get; } = new List<GameRecord>();
    }
}
