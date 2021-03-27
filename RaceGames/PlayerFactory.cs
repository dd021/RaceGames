using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceGames
{
    /// <summary>
    /// This class is a Factory for Player class object
    /// </summary>
    public static class PlayerFactory
    {
        /// <summary>
        /// This Factory method is used to generate Player Object according to given number
        /// </summary>
        /// <param name="number">Player Number</param>
        /// <returns>Player Object if valid number given</returns>
        public static Player GetPlayer(int number)
        {
            if(number == 1)
            {
                return new AI();
            }
            else if (number == 2)
            {
                return new Bob();
            }
            else if(number == 3)
            {
                return new Joe();
            }
            return null;
        }
    }
}
