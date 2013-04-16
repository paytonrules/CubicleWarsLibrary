using System;
using System.Collections;
using NUnit.Framework;
using NSubstitute;

namespace CubicleWarsLibrary
{
	[TestFixture]
	public class HumanPlayerTest
	{
		[Test]
		public void ItIsCreatedWithItsUnitsAndAName()
		{
			var unit = Substitute.For<Unit>();

			var player = new HumanPlayer("PlayerOne", new Unit[] {unit});

			Assert.IsTrue(player.Owns(unit));
			Assert.AreEqual("PlayerOne", player.Name);
		}

		[Test]
		public void ItAllowsYouToSetTheWeapon()
		{
			var unit = Substitute.For<Unit>();
			
			var player = new HumanPlayer("", new Unit[] {unit});
			player.SetWeapon(unit);

			Assert.AreEqual(unit, player.Weapon());
		}

		[Test]
		public void ItsReturnsItsNumberOfLivingUnits()
		{
			var unit = Substitute.For<Unit>();
			var unitTwo = Substitute.For<Unit>();
			unit.Alive().Returns(true);
			unitTwo.Alive().Returns(true);

			var player = new HumanPlayer("", new Unit[] {unit, unitTwo});

			Assert.AreEqual(2, player.LivingUnits());
		}

		[Test]
		public void ItSkipsAnyDeadUnits()
		{
			var unit = Substitute.For<Unit>();
			var unitTwo = Substitute.For<Unit>();
			unit.Alive().Returns(true);
			unitTwo.Alive().Returns(false);
			
			var player = new HumanPlayer("", new Unit[] {unit, unitTwo});
			
			Assert.AreEqual(1, player.LivingUnits());
		}

		[Test]
		public void ItTellsAllItsUnitsToWaitForACommand()
		{
			var unit = Substitute.For<Unit>();
			var unitTwo = Substitute.For<Unit>();

			var player = new HumanPlayer("", new Unit[] {unit, unitTwo});
			player.WaitForCommand();

			unit.Received().PickMe();
			unitTwo.Received().PickMe();
		}

		[Test]
		public void ItTellsAllItsUnitsToStopWaiting()
		{
			var unit = Substitute.For<Unit>();
			var unitTwo = Substitute.For<Unit>();
			
			var player = new HumanPlayer("", new Unit[] {unit, unitTwo});
			player.StopWaitingForCommand();
			
			unit.Received().NotReadyForCommand();
			unitTwo.Received().NotReadyForCommand();
		}

		[Test]
		public void ItTellsAllItsUnitsToWaitForAttack()
		{
			var unit = Substitute.For<Unit>();
			var unitTwo = Substitute.For<Unit>();
			
			var player = new HumanPlayer("", new Unit[] {unit, unitTwo});
			player.WaitForAttack();
			
			unit.Received().PickMe();
			unitTwo.Received().PickMe();
		}

		[Test]
		public void ItAllowsAddingAUnit()
		{
			var unit = Substitute.For<Unit>();

			var player = new HumanPlayer("", new Unit[] {});

			player.AddUnit(unit);

			Assert.IsTrue (player.Owns(unit));
		}
	}
}