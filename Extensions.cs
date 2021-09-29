using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    static class Extensions
    {
        public static List<Area> Check (this List<Area> areas, int startRow, int startCol, int endRow, int endCol)
        {
            return areas.Where(x => x.Row >= startRow && x.Row <= endRow && x.Column >= startCol && x.Column <= endCol).ToList();
        }

        
    }

    public class Board
    {
        public List<Area> Areas { get; set; }
        public Board()
        {
            Areas = new List<Area>();
            for (int i = 1; i<11; i++)
            {
                for (int j = 1; j<11; j++)
                {
                    Areas.Add(new Area(i, j));
                }
            }
        }
    }
}
