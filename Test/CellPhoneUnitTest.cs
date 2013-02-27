using System;
using NUnit.Framework;
using NSubstitute;

namespace CubicleWarsLibrary
{
	[TestFixture]
	public class CellPhoneUnitTest
	{
		[Test]
		public void ItLosesTheDamageBasedOnTheUnits()
		{
			var unit = Substitute.For<Unit>();
			var cellPhone = new CellPhoneUnit { Health = 10 };

			unit.AttackStrengthAgainst(cellPhone).Returns(1);

			cellPhone.AttackWith(unit);

			Assert.AreEqual(9, cellPhone.Health);
		}

		[Test]
		public void ItIsAliveWhenItsHealthIsNotZero ()
		{
			var cellPhone = new CellPhoneUnit { Health = 10 };

			Assert.IsTrue (cellPhone.Alive ());
		}

		[Test]
		public void ItIsDeadWhenItsHealthIsZero ()
		{
			var cellPhone = new CellPhoneUnit { Health = 0 };

			Assert.IsFalse (cellPhone.Alive ());
		}

		[Test]
		public void ItsAttackStrengthAgainstHackersIsZero ()
		{
			var sub = Substitute.For<UnityObject>();
			var hacker = new HackerUnit(sub);
			var cellPhone = new CellPhoneUnit();

			Assert.AreEqual(0, cellPhone.AttackStrengthAgainst (hacker));
		}

		[Test]
		public void ItsAttackStrengthAgainstDronesIsOne()
		{
			var drone = new DroneUnit();
			var cellPhone = new CellPhoneUnit();
			
			Assert.AreEqual(1, cellPhone.AttackStrengthAgainst (drone));
		}
	}
}

