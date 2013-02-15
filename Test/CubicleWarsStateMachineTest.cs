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
		public void ItAssignsTheSelectedObjectToThePlayerOnSelectObject()
		{
			var unit = Substitute.For<Unit>();
			playerOne.Owns(unit).Returns(true);
			
			stateMachine.SelectObject(unit);
			
			playerOne.Received().SetWeapon(unit);
		}

		[Test]
		public void ItDoesNotAssignTheUnitIfTheSelectedUnitIsNotEligible()
		{
			var unit = Substitute.For<Unit>();
			playerOne.Owns(unit).Returns(false);

			stateMachine.SelectObject(unit);

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

			stateMachine.SelectObject(unit);
			stateMachine.Attack(enemyUnit);

			enemyUnit.Received().AttackWith(unit);
		}

		[Test]
		public void ItDoesNotAllowAttackWithoutSelecting()
		{
			var unit = Substitute.For<Unit>();
			var enemyUnit = Substitute.For<Unit>();

			playerOne.Weapon().Returns(unit);
			playerOne.Owns(enemyUnit).Returns(false);
			
			stateMachine.Attack(enemyUnit);
			
			enemyUnit.DidNotReceive().AttackWith(unit);
		}

		[Test]
		public void ItDoesAllowChangingTheSelectedObject()
		{
			var unit = Substitute.For<Unit>();
			var secondUnit = Substitute.For<Unit>();

			playerOne.Owns(unit).Returns(true);
			playerOne.Owns(secondUnit).Returns(true);
			
			stateMachine.SelectObject(unit);
			stateMachine.SelectObject(secondUnit);

			playerOne.Received().SetWeapon(secondUnit);
		}

		[Test]
		public void ItAllowsTheSecondPlayerToSelectAfterTheFirstHasAttacked()
		{
			var unit = Substitute.For<Unit>();
			var enemyUnit = Substitute.For<Unit>();
			
			playerOne.Owns(unit).Returns(true);
			playerTwo.Owns(enemyUnit).Returns(true);
			
			stateMachine.SelectObject(unit);
			stateMachine.Attack(enemyUnit);
			stateMachine.SelectObject(enemyUnit);
			
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
			
			stateMachine.SelectObject(unit);
			stateMachine.Attack(enemyUnit);
			stateMachine.Attack(unit);

			unit.DidNotReceive().AttackWith(enemyUnit);
		}
		// Implementing these objects
		// See attacks in game
		// Enemies can die
		// Game Over

	}
}

