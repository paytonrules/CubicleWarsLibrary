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
			unity.Health.Returns(10);
			var cellPhone = new StandardUnit(conflictResolver, unity);
			
			unit.AttackStrengthAgainst(cellPhone).Returns(1);
			
			cellPhone.AttackWith(unit);
			
			Assert.AreEqual(9, cellPhone.Health);
		}
		
		[Test]
		public void ItIsAliveWhenItsHealthIsNotZero ()
		{
			unity.Health.Returns(10);
			var cellPhone = new StandardUnit(conflictResolver, unity);
			
			Assert.IsTrue (cellPhone.Alive ());
		}
		
		[Test]
		public void ItIsDeadWhenItsHealthIsZero ()
		{
			unity.Health.Returns (0);
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


		class FakeStateMachine : StateMachine {
			public event StateChangedEventHandler StateChanged;
			public State CurrentState { get; set; }
			public Player CurrentPlayer { get; set; }

			public void FireStateMachine(Player player) {
			}

			public void FireStateChange() {
				StateChanged(this, EventArgs.Empty);
			}
		}

		[Test]
		public void ItFiresAWaitingEventIfTheCurrentStateIsWaitingForSelectionAndTheCurrentPlayerOwnsThisUnit()
		{
			var unity = Substitute.For<UnityObject>();
			var conflictResolver = Substitute.For<ConflictResolver>();
			var unit = new StandardUnit(conflictResolver, unity);
			var player = Substitute.For<Player>();
			player.Owns (unit).Returns (true);

			var stateMachine = new FakeStateMachine() { CurrentPlayer = player };
			unit.Observe(stateMachine);

			bool waiting = false;
			unit.Waiting += () => waiting = true;

			stateMachine.FireStateChange();

			Assert.IsTrue (waiting);
		}

		[Test]
		public void ItDoesNotFireAWaitingEventIfTheCurrentStateIsWaitingForSelectionAndTheCurrentPlayerDoesntOwnThisUnit()
		{
			var unity = Substitute.For<UnityObject>();
			var conflictResolver = Substitute.For<ConflictResolver>();
			var unit = new StandardUnit(conflictResolver, unity);
			var player = Substitute.For<Player>();
			player.Owns (unit).Returns (false);
			
			var stateMachine = new FakeStateMachine() { CurrentPlayer = player };
			unit.Observe(stateMachine);
			
			bool waiting = false;
			unit.Waiting += () => waiting = true;
			
			stateMachine.FireStateChange();
			
			Assert.IsFalse (waiting);
		}
	}
}

