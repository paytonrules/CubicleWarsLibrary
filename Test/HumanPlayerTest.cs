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
		public void ItIsCreatedWithItsUnits()
		{
			var unit = Substitute.For<Unit>();

			var player = new HumanPlayer(new Unit[] {unit});

			Assert.IsTrue (player.Owns(unit));
		}

		[Test]
		public void ItAllowsYouToSetTheWeapon()
		{
			var unit = Substitute.For<Unit>();
			
			var player = new HumanPlayer(new Unit[] {unit});
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

			var player = new HumanPlayer(new Unit[] {unit, unitTwo});

			Assert.AreEqual(2, player.LivingUnits());
		}

		[Test]
		public void ItSkipsAnyDeadUnits()
		{
			var unit = Substitute.For<Unit>();
			var unitTwo = Substitute.For<Unit>();
			unit.Alive().Returns(true);
			unitTwo.Alive().Returns(false);
			
			var player = new HumanPlayer(new Unit[] {unit, unitTwo});
			
			Assert.AreEqual(1, player.LivingUnits());
		}
	}
}