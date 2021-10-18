using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.Models
{
    public abstract class Ship
    {
        public string Name { get; set; }
        public int Length { get; set; }
    }

    public class Carrier : Ship
    {
        public Carrier()
        {
            Name = "Carrier";
            Length = 5;
        }
    }

    public class Battleship : Ship
    {
        public Battleship()
        {
            Name = "Battleship";
            Length = 4;
        }
    }

    public class Destroyer : Ship
    {
        public Destroyer()
        {
            Name = "Destroyer";
            Length = 3;
        }
    }

    public class Submarine : Ship
    {
        public Submarine()
        {
            Name = "Submarine";
            Length = 3;
        }
    }

    public class PatrolBoat : Ship
    {
        public PatrolBoat()
        {
            Name = "PatrolBoat";
            Length = 2;
        }
    }
}
