using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Domain.Test
{
    [TestClass]
    public class FleetTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WhenFleetIsCreated_WithIncompleteSetOfShips_ExceptionIsThrown()
        {
            var ship1 = new Ship(new Cell(0, 0), Direction.Horizontal, 2);
            var ship2 = new Ship(new Cell(2, 2), Direction.Horizontal, 2);

            var fleet = new Fleet(new[] { ship1, ship2 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WhenFleetIsCreated_WithShipsInConflict_ExceptionIsThrown()
        {
            var ship1 = new Ship(new Cell(2, 2), Direction.Diagonal, 2);
            var ship2 = new Ship(new Cell(3, 3), Direction.Diagonal, 2);

            var fleet = new Fleet(new[] { ship1, ship2 }, list => true);
        }

        [TestMethod]
        public void GivenAFleet_WhenShipIsBombedForTheFirstTime_ThenHitIsReported()
        {
            var ship = new Ship(new Cell(0, 0), Direction.Horizontal, 3);
            var fleet = new Fleet(new[] { ship }, list => true);

            BombardmentResult hitResult = fleet.Bomb(new Cell(0, 0));

            Assert.AreEqual(BombardmentResult.Hit, hitResult);
        }

        [TestMethod]
        public void GivenAFleet_WhenShipIsHitOnAllCells_ThenDestroyedIsReported()
        {
            var ship = new Ship(new Cell(0, 0), Direction.Horizontal, 2);
            var fleet = new Fleet(new[] { ship }, list => true);

            BombardmentResult hitResult1 = fleet.Bomb(new Cell(0, 0));
            BombardmentResult hitResult2 = fleet.Bomb(new Cell(0, 1));

            Assert.AreEqual(BombardmentResult.Hit, hitResult1);
            Assert.AreEqual(BombardmentResult.Destroyed, hitResult2);
        }

        [TestMethod]
        public void GivenAFleet_AndShipIsAlreadyDestroyed_WhenShipIsHitAgain_ThenWaterIsReported()
        {
            var ship = new Ship(new Cell(0, 0), Direction.Horizontal, 2);
            var fleet = new Fleet(new[] { ship }, list => true);

            fleet.Bomb(new Cell(0, 0));
            fleet.Bomb(new Cell(0, 1));

            BombardmentResult hitResult = fleet.Bomb(new Cell(0, 1));

            Assert.AreEqual(BombardmentResult.Water, hitResult);
        }
    }
}
