using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Domain
{
    public class Fleet
    {
        private readonly IList<Ship> ships; 

        public Fleet(IList<Ship> ships)
            : this(ships, IsCompleteFleet)
        {
        }

        public Fleet(IList<Ship> ships, Func<IList<Ship>, bool> fleetValidator)
        {
            if (!fleetValidator(ships))
            {
                throw new ArgumentException("This set of ships is not a complete fleet.", "ships");
            }

            if (AreInConflict(ships))
            {
                throw new ArgumentException("This constellation of ships is invalid. They touch or overlap.", "ships");
            }

            this.ships = ships;
        }

        public BombardmentResult Bomb(Cell cell)
        {
            Ship hitShip = this.ships.FirstOrDefault(ship => ship.InitializedCells.Contains(cell));
            
            if (hitShip == null)
            {
                return BombardmentResult.Water;
            }

            return hitShip.Bomb(cell);
        }

        public bool AreAllShipsDestroyed()
        {
            return this.ships.All(ship => ship.IsDestroyed);
        }

        public static bool IsCompleteFleet(IList<Ship> ships)
        {
            // Sub: 2 Squares - 2 Ships
            // Destroyer: 3 Squares - 4 Ships
            // Cruiser: 4 Squares - 2 Ships
            // Battleship: 5 Squares - 1 Ship
         
            return ((ships.Count(ship => ship.Size == 2) == 2) 
                && (ships.Count(ship => ship.Size == 3) == 4)
                && (ships.Count(ship => ship.Size == 4) == 2) 
                && (ships.Count(ship => ship.Size == 5) == 1));
        }

        private static bool AreInConflict(IEnumerable<Ship> ships)
        {
            Ship[] shipArray = ships.ToArray();

            for (int i = 0; i < shipArray.Length - 1; i++)
            {
                for (int j = 0; j < shipArray.Length; j++)
                {
                    if (AreInConflict(shipArray[i], shipArray[j]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool AreInConflict(Ship ship1, Ship ship2)
        {
            return ship1.IsInConflictWith(ship2);
        }
    }
}