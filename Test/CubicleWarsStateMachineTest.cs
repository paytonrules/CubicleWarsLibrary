using System;
using NUnit.Framework;
using NSubstitute;

namespace CubicleWarsLibrary
{
	[TestFixture]
	public class CubicleWarsStateMachineTest
	{
		Player 					playerOne;
		Player 					playerTwo;
		CubicleWarsStateMachine	stateMachine;
		
		[SetUp]
		public void BeforeEach() {
			playerOne = Substitute.For<Player>();
			playerTwo = Substitute.For<Player>();
			
			stateMachine = new CubicleWarsStateMachine(playerOne, playerTwo);
		}
		
		[Test]
		public void ItStartsWithPlayerOne()
		{
			Assert.AreEqual(playerOne, stateMachine.CurrentPlayer);
		}
		
		[Test]
		public void ItAssignsTheSelectedObjectToThePlayerOnSelect()
		{
			var unit = Substitute.For<Unit>();
			playerOne.Owns(unit).Returns(true);
			
			stateMachine.Select(unit);
			
			playerOne.Received().SetWeapon(unit);
		}

		[Test]
		public void ItDoesNotAssignTheUnitToThePlayerIfTheSelectedUnitIsNotEligible()
		{
			var unit = Substitute.For<Unit>();
			playerOne.Owns(unit).Returns(false);

			stateMachine.Select(unit);

			playerOne.DidNotReceive().SetWeapon(unit);
		}

		[Test]
		public void ItAllowsAttackingAfterSelecting()
		{
			var unit = Substitute.For<Unit>();
			var enemyUnit = Substitute.For<Unit>();

			playerOne.Owns(unit).Returns(true);
			playerOne.Weapon().Returns(unit);
			playerOne.Owns(enemyUnit).Returns(false);

			stateMachine.Select(unit);
			stateMachine.Select(enemyUnit);

			enemyUnit.Received().AttackWith(unit);
		}

		[Test]
		public void ItDoesNotAttackWithoutSelecting()
		{
			var unit = Substitute.For<Unit>();
			var enemyUnit = Substitute.For<Unit>();

			playerOne.Weapon().Returns(unit);
			playerOne.Owns(enemyUnit).Returns(false);
			
			stateMachine.Select(enemyUnit);
			
			enemyUnit.DidNotReceive().AttackWith(unit);
		}

		[Test]
		public void ItDoesAllowChangingTheSelectedObject()
		{
			var unit = Substitute.For<Unit>();
			var secondUnit = Substitute.For<Unit>();

			playerOne.Owns(unit).Returns(true);
			playerOne.Owns(secondUnit).Returns(true);
			
			stateMachine.Select(unit);
			stateMachine.Select(secondUnit);

			playerOne.Received().SetWeapon(secondUnit);
		}

		[Test]
		public void ItAllowsTheSecondPlayerToSelectAfterTheFirstHasAttacked()
		{
			var unit = Substitute.For<Unit>();
			var enemyUnit = Substitute.For<Unit>();
			
			playerOne.Owns(unit).Returns(true);
			playerTwo.Owns(enemyUnit).Returns(true);
			
			stateMachine.Select(unit);
			stateMachine.Select(enemyUnit);
			stateMachine.Select(enemyUnit);
			
			playerTwo.Received().SetWeapon(enemyUnit);
		}

		[Test]
		public void ItRequiresTheSecondPlayerToSelectAnObjectBeforeItAttacks()
		{
			var unit = Substitute.For<Unit>();
			var enemyUnit = Substitute.For<Unit>();
			
			playerOne.Owns(unit).Returns(true);
			playerTwo.Owns(enemyUnit).Returns(true);
			playerTwo.Weapon().Returns (enemyUnit);
			
			stateMachine.Select(unit);
			stateMachine.Select(enemyUnit);
			stateMachine.Select(unit);

			unit.DidNotReceive().AttackWith(enemyUnit);
		}

		[Test]
		public void AfterAnAttackPlayerOneCanWin()
		{
			var unit = Substitute.For<Unit>();
			var enemyUnit = Substitute.For<Unit>();
			playerOne.Owns(unit).Returns(true);
			playerTwo.Owns(enemyUnit).Returns(true);
			playerTwo.LivingUnits().Returns(0);

			stateMachine.Select(unit);
			stateMachine.Select(enemyUnit);

			Assert.AreEqual(CubicleWarsStateMachine.State.PlayerWins, stateMachine.CurrentState);
		}

		// Game needs to end at the end of the battle
		// ends when everybody on one team has a health of zero
		// So let's start doing health
		// Health and attacks on each item.  
		// Wonder how you'll skip a turn
	}
}

