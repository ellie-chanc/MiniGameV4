using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGameV4.Model
{
    internal class GameRecord
    {
        public int GameRecordId { get; set; }
        public int FoodConsumed { get; set; }

        // GameRecord.UserId is discovered as a foreign key referencing the User.UserId
        // The relationship is discovered as required because Post.BlogId is not nullable
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
