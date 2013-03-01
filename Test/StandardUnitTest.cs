using System;
using NUnit.Framework;
using NSubstitute;

namespace CubicleWarsLibrary
{
	[TestFixture]
	public class StandardUnitTest
	{
		ConflictResolver conflictResolver;
		UnityObject unity;

		[SetUp]
		public void BeforeEach() 
		{
			conflictResolver = Substitute.For<ConflictResolver>();
			unity = Substitute.For<UnityObject>();
		}

		[Test]
		public void ItLosesTheDamageBasedOnTheUnits()
		{
			var unit = Substitute.For<Unit>();
			var cellPhone = new StandardUnit(conflictResolver, unity) { Health = 10 };
			
			unit.AttackStrengthAgainst(cellPhone).Returns(1);
			
			cellPhone.AttackWith(unit);
			
			Assert.AreEqual(9, cellPhone.Health);
		}
		
		[Test]
		public void ItIsAliveWhenItsHealthIsNotZero ()
		{
			var cellPhone = new StandardUnit(conflictResolver, unity) { Health = 10 };
			
			Assert.IsTrue (cellPhone.Alive ());
		}
		
		[Test]
		public void ItIsDeadWhenItsHealthIsZero ()
		{
			var cellPhone = new StandardUnit(conflictResolver, unity) { Health = 0 };
			
			Assert.IsFalse (cellPhone.Alive ());
		}
		
		[Test]
		public void ItsAttackStrengthComesFromItsConflictResolver()
		{
			var conflictResolver = Substitute.For<ConflictResolver>();
			var enemy = Substitute.For<Unit>();
			var cellPhone = new StandardUnit(conflictResolver, unity);
			
			conflictResolver.Resolve(enemy, cellPhone).Returns(1);
			
			Assert.AreEqual(1, cellPhone.AttackStrengthAgainst (enemy));
		}

		[Test]
		public void ItNotifiesItsUnityObjectWhenItsAttacked()
		{
			var unity = Substitute.For<UnityObject>();
			var conflictResolver = Substitute.For<ConflictResolver>();
			var unit = new StandardUnit(conflictResolver, unity);
			
			unit.AttackWith(unit);
			
			unity.Received().Attacked();
		}
	}
}

