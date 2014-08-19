using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Domain
{
    public class Ship
    {
        private readonly int size;
        
        private readonly Direction direction;
        
        private readonly IReadOnlyCollection<Cell> initializedCells;

        private readonly IList<Cell> bombedCells;

        public Ship(Cell cell, Direction direction, int size)
        {
            if (size <= 0 || size > Game.BoardSize)
            {
                throw new ArgumentOutOfRangeException("size");
            }

            this.direction = direction;
            this.size = size;
            this.initializedCells = GetCells(cell, direction, size).AsReadOnly();
            this.bombedCells = new List<Cell>();
        }

        public Direction Direction
        {
            get { return this.direction; }
        }

        public int Size
        {
            get { return this.size; }
        }

        public IEnumerable<Cell> InitializedCells
        {
            get { return this.initializedCells; }
        }

        public IEnumerable<Cell> BombedCells
        {
            get { return this.bombedCells; }
        }

        public bool IsDestroyed
        {
            get { return this.bombedCells.Count == this.InitializedCells.Count(); }
        }

        public bool HasBeenHit()
        {
            return this.bombedCells.Count > 0;
        }

        public BombardmentResult Bomb(Cell cell)
        {
            if (!this.InitializedCells.Contains(cell))
            {
                return BombardmentResult.Water; // missed the ship.
            }

            if (this.IsDestroyed)
            {
                return BombardmentResult.Water; // we are already down.
            }

            if (!this.bombedCells.Contains(cell))
            {
                this.bombedCells.Add(cell);
            }

            return IsDestroyed ? BombardmentResult.Destroyed : BombardmentResult.Hit;
        }

        /// <summary>
        /// Two ships are in conflict if they touch.
        /// (What does 'touch' mean?!)
        /// (This code could be simplified without diagonal entries)
        /// </summary>
        public bool IsInConflictWith(Ship otherShip)
        {
            Cell[] blockedCells = this.GetBlockedCells().ToArray();
            Cell[] otherShipCells = otherShip.InitializedCells.ToArray();

            return otherShipCells.Any(blockedCells.Contains);
        }

        private static List<Cell> GetCells(Cell cell, Direction direction, int size)
        {
            var cells = new List<Cell>(size);

            try
            {
                for (int i = 0; i < size; i++)
                {
                    cells.Add(cell);

                    if (i < size - 1)
                    {
                        cell = cell.GetNext(direction);
                    }
                }
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid ship placement.");
            }

            return cells;
        }

        /// <summary>
        /// All cells which must not be occupied by any other ship.
        /// </summary>
        private IEnumerable<Cell> GetBlockedCells()
        {
            foreach (Cell cell in this.InitializedCells)
            {
                for (int row = cell.Row - 1; row <= cell.Row + 1; row++)
                {
                    for (int column = cell.Column - 1; column <= cell.Column + 1; column++)
                    {
                        if (Cell.IsOnBoard(row, column))
                        {
                            yield return new Cell(row, column);
                        }
                    }
                }
            }
        }
    }
}
