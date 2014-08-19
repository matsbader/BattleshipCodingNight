using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Domain.Test
{
    [TestClass]
    public class ShipTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WhenShipIsCreated_WithSize0_ExceptionIsThrown()
        {
            var ship = new Ship(new Cell(0, 0), Direction.Horizontal, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WhenShipIsCreated_WithSizeLargerThanTheBoard_ExceptionIsThrown()
        {
            var ship = new Ship(new Cell(0, 0), Direction.Horizontal, Game.BoardSize + 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenShipIsCreated_WithCellOutsideBoard_ExceptionIsThrown()
        {
            var ship = new Ship(new Cell(0, Game.BoardSize - 2), Direction.Horizontal, 3);
        }

        [TestMethod]
        public void WhenShipIsCreated_ThenAllPropertiesAreCorrectlyInitialized()
        {
            const int Size = 3;
            const Direction Direction = Direction.Diagonal;

            var ship = new Ship(new Cell(0, 0), Direction, Size);

            Assert.AreEqual(Size, ship.Size);
            Assert.AreEqual(Direction, ship.Direction);
            Assert.AreEqual(0, ship.BombedCells.Count());
        }

        [TestMethod]
        public void GivenAShipWithHorizontalDirection_WhenQueriedForCells_Succeeds()
        {
            var ship = new Ship(new Cell(1, 2), Direction.Horizontal, 3);

            IEnumerable<Cell> cells = ship.InitializedCells.ToList();

            Assert.AreEqual(3, cells.Count());
            Assert.IsTrue(cells.Contains(new Cell(1, 2)));
            Assert.IsTrue(cells.Contains(new Cell(1, 3)));
            Assert.IsTrue(cells.Contains(new Cell(1, 4)));
        }

        [TestMethod]
        public void GivenAShipWithDiagonallDirection_WhenQueriedForCells_Succeeds()
        {
            var ship = new Ship(new Cell(4, 4), Direction.Diagonal, 2);

            IEnumerable<Cell> cells = ship.InitializedCells.ToList();

            Assert.AreEqual(2, cells.Count());
            Assert.IsTrue(cells.Contains(new Cell(4, 4)));
            Assert.IsTrue(cells.Contains(new Cell(5, 5)));
        }

        [TestMethod]
        public void GivenAShip_WhenShipIsBombedForTheFirstTime_ThenHitIsReported()
        {
            var ship = new Ship(new Cell(0, 0), Direction.Horizontal, 3);

            BombardmentResult hitResult = ship.Bomb(new Cell(0, 0));

            Assert.AreEqual(BombardmentResult.Hit, hitResult);
            Assert.AreEqual(1, ship.BombedCells.Count());
        }

        [TestMethod]
        public void GivenAShip_WhenShipIsHitOnAllCells_ThenDestroyedIsReported()
        {
            var ship = new Ship(new Cell(0, 0), Direction.Horizontal, 2);

            BombardmentResult hitResult1 = ship.Bomb(new Cell(0, 0));
            BombardmentResult hitResult2 = ship.Bomb(new Cell(0, 1));

            Assert.AreEqual(BombardmentResult.Hit, hitResult1);
            Assert.AreEqual(BombardmentResult.Destroyed, hitResult2);
            Assert.AreEqual(2, ship.BombedCells.Count());
        }

        [TestMethod]
        public void GivenAShip_AndShipIsAlreadyDestroyed_WhenShipIsHitAgain_ThenWaterIsReported()
        {
            var ship = new Ship(new Cell(0, 0), Direction.Horizontal, 2);

            ship.Bomb(new Cell(0, 0));
            ship.Bomb(new Cell(0, 1));

            BombardmentResult hitResult = ship.Bomb(new Cell(0, 1));

            Assert.AreEqual(BombardmentResult.Water, hitResult);
            Assert.AreEqual(2, ship.BombedCells.Count());
        }

        [TestMethod]
        public void GivenTwoShips_AndTheyHaveSameRoot_ThenTheyAreInConflict()
        {
            var ship1 = new Ship(new Cell(2, 2), Direction.Horizontal, 3);
            var ship2 = new Ship(new Cell(2, 2), Direction.Vertical, 4);

            Assert.IsTrue(ship1.IsInConflictWith(ship2));
            Assert.IsTrue(ship2.IsInConflictWith(ship1));
        }

        [TestMethod]
        public void GivenTwoShips_AndTheyAreAdjacent_ThenTheyAreInConflict()
        {
            var ship1 = new Ship(new Cell(2, 2), Direction.Horizontal, 3);
            var ship2 = new Ship(new Cell(3, 2), Direction.Horizontal, 4);

            Assert.IsTrue(ship1.IsInConflictWith(ship2));
            Assert.IsTrue(ship2.IsInConflictWith(ship1));
        }

        [TestMethod]
        public void GivenTwoShips_AndTheyIntersect_ThenTheyAreInConflict()
        {
            var ship1 = new Ship(new Cell(0, 5), Direction.Vertical, Game.BoardSize);
            var ship2 = new Ship(new Cell(5, 0), Direction.Horizontal, Game.BoardSize);

            Assert.IsTrue(ship1.IsInConflictWith(ship2));
            Assert.IsTrue(ship2.IsInConflictWith(ship1));
        }

        [TestMethod]
        public void GivenTwoHorizontalShips_AndTheyAreInRows2And4_ThenTheyAreNotInConflict()
        {
            var ship1 = new Ship(new Cell(2, 3), Direction.Horizontal, 5);
            var ship2 = new Ship(new Cell(4, 3), Direction.Horizontal, 5);

            Assert.IsFalse(ship1.IsInConflictWith(ship2));
            Assert.IsFalse(ship2.IsInConflictWith(ship1));
        }
    }
}
