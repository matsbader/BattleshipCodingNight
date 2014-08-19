using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Domain.Test
{
    [TestClass]
    public class CellTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenCellIsCreated_AndRowIsNegative_ExceptionIsThrown()
        {
            var cell = new Cell(-1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenCellIsCreated_AndRowIsLargerThanBoard_ExceptionIsThrown()
        {
            var cell = new Cell(Game.BoardSize, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenCellIsCreated_AndColumnIsNegative_ExceptionIsThrown()
        {
            var cell = new Cell(0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenCellIsCreated_AndColumnIsLargerThanBoard_ExceptionIsThrown()
        {
            var cell = new Cell(0, Game.BoardSize);
        }

        [TestMethod]
        public void WhenCellIsCreated_AndColumnAndRowAreOnBoard_ThenCellIsCreated()
        {
            var cell1 = new Cell(1, 1);
            var cell2 = new Cell(Game.BoardSize - 1, 1);
            var cell3 = new Cell(1, Game.BoardSize - 1);
            var cell4 = new Cell(Game.BoardSize - 1, Game.BoardSize - 1);
        }

        [TestMethod]
        public void WhenRowAndColumnAreOnBoard_ThenCellIsOnBoard()
        {
            Assert.IsTrue(Cell.IsOnBoard(1, 1));
            Assert.IsTrue(Cell.IsOnBoard(Game.BoardSize - 1, 1));
            Assert.IsTrue(Cell.IsOnBoard(1, Game.BoardSize - 1));
            Assert.IsTrue(Cell.IsOnBoard(Game.BoardSize - 1, Game.BoardSize - 1));
        }

        [TestMethod]
        public void Given2Cells_WhenBothHaveSameLocation_ThenTheyAreEqual()
        {
            var cell1 = new Cell(2, 3);
            var cell2 = new Cell(2, 3);

            Assert.IsTrue(cell1.Equals(cell2));
            Assert.IsTrue(cell1.Equals((object)cell2));
        }

        [TestMethod]
        public void Given2Cells_WhenRowsAreDifferent_ThenTheyAreNotEqual()
        {
            var cell1 = new Cell(2, 3);
            var cell2 = new Cell(0, 3);

            Assert.IsFalse(cell1.Equals(cell2));
            Assert.IsFalse(cell1.Equals((object)cell2));

            Assert.IsFalse(cell2.Equals(cell1));
            Assert.IsFalse(cell2.Equals((object)cell1));
        }

        [TestMethod]
        public void Given2Cells_WhenColumnsAreDifferent_ThenTheyAreNotEqual()
        {
            var cell1 = new Cell(2, 3);
            var cell2 = new Cell(2, 2);

            Assert.IsFalse(cell1.Equals(cell2));
            Assert.IsFalse(cell1.Equals((object)cell2));

            Assert.IsFalse(cell2.Equals(cell1));
            Assert.IsFalse(cell2.Equals((object)cell1));
        }

        [TestMethod]
        public void GivenACellWithColumn1_WhenQueriedForNextCellInHorizontalDirection_ThenItHasColumn2()
        {
            var cell = new Cell(4, 1);

            var nextCell = cell.GetNext(Direction.Horizontal);

            Assert.AreEqual(4, nextCell.Row);
            Assert.AreEqual(2, nextCell.Column);
        }

        [TestMethod]
        public void GivenACellWithRow0_WhenQueriedForNextCellInVerticalDirection_ThenItHasRow1()
        {
            var cell = new Cell(0, 5);

            var nextCell = cell.GetNext(Direction.Vertical);

            Assert.AreEqual(1, nextCell.Row);
            Assert.AreEqual(5, nextCell.Column);
        }

        [TestMethod]
        public void GivenACellWithRow1AndColumn2_WhenQueriedForNextCellInDiagonalDirection_ThenItHasRow2AndColumn3()
        {
            var cell = new Cell(1, 2);

            var nextCell = cell.GetNext(Direction.Diagonal);

            Assert.AreEqual(2, nextCell.Row);
            Assert.AreEqual(3, nextCell.Column);
        }
    }
}
