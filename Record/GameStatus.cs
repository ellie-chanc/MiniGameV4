using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniGameV4.Data;
using MiniGameV4.Model;

namespace MiniGameV4.Record
{
    internal class GameStatus
    {
        private GameRecord gameRecord;
        private GameContext gameContext;

        public GameStatus(GameContext context, int userId)
        {
            gameContext = context;

            // insert new game record in database
            gameRecord = new GameRecord();
            gameRecord.FoodConsumed = 0;
            gameRecord.UserId = userId;
            gameContext.Add(gameRecord);
            gameContext.SaveChanges();
        }

        public void UpdateConsumedFoodNumber()
        {
            gameRecord.FoodConsumed++;
            gameContext.SaveChanges();
        }
    }
}
