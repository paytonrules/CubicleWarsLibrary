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
		public void ItStoresTheName()
		{
			unity.Name.Returns("Eric");
			var cellPhone = new StandardUnit(conflictResolver, unity);

			Assert.AreEqual("Eric", cellPhone.UnitName);
		}

		[Test]
		public void ItLosesTheDamageBasedOnTheUnits()
		{
			var unit = Substitute.For<Unit>();
			unity.InitialHealth.Returns(10);
			var cellPhone = new StandardUnit(conflictResolver, unity);
			
			unit.AttackStrengthAgainst(cellPhone).Returns(1);
			
			cellPhone.AttackWith(unit);
			
			Assert.AreEqual(9, cellPhone.Health);
		}
		
		[Test]
		public void ItIsAliveWhenItsHealthIsNotZero ()
		{
			unity.InitialHealth.Returns(10);
			var cellPhone = new StandardUnit(conflictResolver, unity);
			
			Assert.IsTrue (cellPhone.Alive ());
		}
		
		[Test]
		public void ItIsDeadWhenItsHealthIsZero ()
		{
			unity.InitialHealth.Returns (0);
			var cellPhone = new StandardUnit(conflictResolver, unity);
			
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
		public void ItFiresTheAttackedEventWhenItsAttacked()
		{
			var unity = Substitute.For<UnityObject>();
			var conflictResolver = Substitute.For<ConflictResolver>();
			var unit = new StandardUnit(conflictResolver, unity);
			bool attacked = false;

			unit.Attacked += () => attacked = true;
			unit.AttackWith(unit);

			Assert.IsTrue(attacked);
		}

		[Test]
		public void ItFiresTheDeadEventWhenItDies()
		{
			var unity = Substitute.For<UnityObject>();
			unity.InitialHealth.Returns(1);
			var conflictResolver = Substitute.For<ConflictResolver>();

			var unit = new StandardUnit(conflictResolver, unity);
			conflictResolver.Resolve(unit, unit).Returns(1);

			bool dead = false;
			unit.Dead += () => dead = true;

			unit.AttackWith(unit);

			Assert.IsTrue (dead);
		}

		[Test]
		public void ItFiresWaitingForPickMe()
		{
			var unity = Substitute.For<UnityObject>();
			var conflictResolver = Substitute.For<ConflictResolver>();
			var unit = new StandardUnit(conflictResolver, unity);

			bool waiting = false;
			unit.Waiting += () => waiting = true;

			unit.PickMe();
			
			Assert.IsTrue(waiting);
		}

		[Test]
		public void ItDoesntCrashIfNobodyIsWaiting()
		{
			var unity = Substitute.For<UnityObject>();
			var conflictResolver = Substitute.For<ConflictResolver>();
			var unit = new StandardUnit(conflictResolver, unity);
			
			unit.PickMe();
		}

		[Test]
		public void ItFiresDoneWaitingWhenNotReadyForACommand()
		{
			var unity = Substitute.For<UnityObject>();
			var conflictResolver = Substitute.For<ConflictResolver>();
			var unit = new StandardUnit(conflictResolver, unity);
			
			bool doneWaiting = false;
			unit.DoneWaiting += () => doneWaiting = true;
			
			unit.NotReadyForCommand();
			
			Assert.IsTrue(doneWaiting);
		}

		[Test]
		public void ItDoesntCrashIfNobodyIsListeningForDoneWaiting()
		{
			var unity = Substitute.For<UnityObject>();
			var conflictResolver = Substitute.For<ConflictResolver>();
			var unit = new StandardUnit(conflictResolver, unity);
			
			unit.NotReadyForCommand();
		}
	}
}