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
			playerOne.Name.Returns("PlayerOne");
			playerTwo = Substitute.For<Player>();
			playerTwo.Name.Returns("PlayerTwo");
			
			stateMachine = new CubicleWarsStateMachine(playerOne, playerTwo);
		}
		
		[Test]
		public void ItStartsWithPlayerOne()
		{
			Assert.AreEqual(playerOne, stateMachine.CurrentPlayer);
		}

		[Test] 
		public void ItInformsPlayerOneItIsWaitingOnStartup()
		{
			playerOne.Received().WaitForCommand();
		}

		[Test]
		public void ItAllowsAddingAUnitToAPlayer()
		{
			var unit = Substitute.For<Unit>();

			stateMachine.AddUnitToPlayer("PlayerOne", unit);
			stateMachine.AddUnitToPlayer("PlayerTwo", unit);

			playerOne.Received().AddUnit(unit);
			playerTwo.Received().AddUnit(unit);
		}

		[Test]
		public void ItRemindsThePlayerItIsWaitingIfItIsWaitingForCommand()
		{
			var unit = Substitute.For<Unit>();
			stateMachine.AddUnitToPlayer("PlayerOne", unit);

			playerOne.Received(2).WaitForCommand();
		}

		[Test]
		public void ItDoesNotInformPlayerIfItIsntTheCurrentOne()
		{
			var unit = Substitute.For<Unit>();
			stateMachine.AddUnitToPlayer("PlayerTwo", unit);
			
			playerTwo.DidNotReceive().WaitForCommand();
		}

		[Test]
		public void ItDoesNotAllowAddingUnitsIfWeAreNotWaitingForACommand()
		{
			var unit = Substitute.For<Unit>();
			var playersCurrentUnit = Substitute.For<Unit>();
			playerOne.Owns(playersCurrentUnit).Returns (true);

			stateMachine.Select(playersCurrentUnit);

			try {
				stateMachine.AddUnitToPlayer("PlayerTwo", unit);
				Assert.Fail ("This did not fail when it should");
			} catch (Exception e) {
				Assert.IsInstanceOf<InvalidOperationException>(e);
			}

		}

		[Test]
		public void ItGivesAUsefulErrorMessageIfThePlayerDoesntExist()
		{
			var unit = Substitute.For<Unit>();

			try {
				stateMachine.AddUnitToPlayer("Not a Player", unit);
				Assert.Fail ("Did not throw an expected exception");
			}
			catch (Exception e) {
				Assert.IsInstanceOf<InvalidPlayer>(e);
			}
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
		public void ItStopsWaitingForCommandWhenTheCurrentPlayerUnitIsSelected()
		{
			var unit = Substitute.For<Unit>();
			playerOne.Owns (unit).Returns (true);

			stateMachine.Select (unit);

			playerOne.Received().StopWaitingForCommand();
		}

		[Test]
		public void ItMakesTheOpponentWaitForAttackOnTransitionToAttack()
		{
			var unit = Substitute.For<Unit>();
			playerOne.Owns (unit).Returns (true);
			
			stateMachine.Select (unit);
			
			playerTwo.Received().WaitForAttack();
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
			playerTwo.LivingUnits().Returns(3);
			
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
			playerTwo.LivingUnits().Returns (3);
			
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

			Assert.AreEqual(State.PlayerWins, stateMachine.CurrentState);
		}

		[Test]
		public void AfterPlayerOneWinsTheEventIsFiredCorrectly()
		{
			var unit = Substitute.For<Unit>();
			var enemyUnit = Substitute.For<Unit>();
			playerOne.Owns(unit).Returns(true);
			playerTwo.Owns(enemyUnit).Returns(true);
			playerTwo.LivingUnits().Returns(0);

			String winner = null;
			stateMachine.GameOver += delegate(string theWinner) {
				winner = theWinner;
			};
			
			stateMachine.Select(unit);
			stateMachine.Select(enemyUnit);

			Assert.AreEqual("PlayerOne", winner);
		}

		[Test]
		public void AfterPlayerTwoWinsTheEventIsFiredCorrectly()
		{
			var unit = Substitute.For<Unit>();
			var enemyUnit = Substitute.For<Unit>();
			playerOne.Owns(unit).Returns(true);
			playerTwo.Owns(enemyUnit).Returns(true);
			playerTwo.LivingUnits().Returns(3);
			playerOne.LivingUnits().Returns(0);
			
			String winner = null;
			stateMachine.GameOver += delegate(string theWinner) {
				winner = theWinner;
			};
			
			stateMachine.Select(unit);
			stateMachine.Select(enemyUnit);
			stateMachine.Select(enemyUnit);
			stateMachine.Select(unit);
			
			Assert.AreEqual("PlayerTwo", winner);
		}

		[Test]
		public void ItDoesNotSwitchTheCurrentPlayerAfterAddingAUnit()
		{
			var unit = Substitute.For<Unit>();

			Assert.AreEqual("PlayerOne", stateMachine.CurrentPlayer.Name);

			stateMachine.AddUnitToPlayer("PlayerOne", unit);

			Assert.AreEqual("PlayerOne", stateMachine.CurrentPlayer.Name);
		}
	}
}

