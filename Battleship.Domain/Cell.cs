using System;

namespace Battleship.Domain
{
    public class Cell
    {
        public Cell(int row, int column)
        {
            if (!IsOnBoard(row, column))
            {
                throw new ArgumentException("Cell is not on board.");
            }

            this.Row = row;
            this.Column = column;
        }

        public int Row
        {
            get; private set;
        }

        public int Column
        {
            get; private set;
        }

        public Cell GetNext(Direction direction)
        {
            switch (direction)
            {
                case Direction.Horizontal:
                    return new Cell(this.Row, this.Column + 1);

                case Direction.Diagonal:
                    return new Cell(this.Row + 1, this.Column + 1);

                case Direction.Vertical:
                    return new Cell(this.Row + 1, this.Column);

                default:
                    throw new NotSupportedException("Unsupported direction.");
            }
        }

        public static bool IsOnBoard(int row, int column)
        {
            if (row < 0 || row >= Game.BoardSize)
            {
                return false;
            }

            if (column < 0 || column >= Game.BoardSize)
            {
                return false;
            }

            return true;
        }

        public bool Equals(Cell other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return this.Row == other.Row && this.Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return this.Equals((Cell)obj);
        }

        public override int GetHashCode()
        {
            return this.Row ^ this.Column;
        }

        public override string ToString()
        {
            return string.Format("Row {0} | Column {1}", this.Row, this.Column);
        }
    }
}