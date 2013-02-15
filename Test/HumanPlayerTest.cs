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

			var player = new HumanPlayer(new ArrayList {unit});

			Assert.IsTrue (player.Owns(unit));
		}

		[Test]
		public void ItAllowsYouToSetTheWeapon()
		{
			var unit = Substitute.For<Unit>();
			
			var player = new HumanPlayer(new ArrayList {unit});
			player.SetWeapon(unit);

			Assert.AreEqual(unit, player.Weapon());
		}
	}
}