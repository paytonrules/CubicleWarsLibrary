using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Stateless;

namespace CubicleWarsLibrary
{
	public class InvalidPlayer : Exception
	{
		public InvalidPlayer(String message) : base(message)
		{
		}
	}

	public class CubicleWarsStateMachine : StateMachine
	{
		public State CurrentState 
		{ 
			get
			{
				return machine.State;
			}
		}

		public Player CurrentPlayer 
		{
			get 
			{ 
				return players[0]; 
			}
		}

		public Player Opponent
		{
			get
			{
				return players[1];
			}
		}

		protected StateMachine<State, Trigger> machine;
		protected StateMachine<State, Trigger>.TriggerWithParameters<Unit> clickWeapon;
		protected List<Player> players;

		public CubicleWarsStateMachine(Player playerOne, Player playerTwo)
		{
			players = new List<Player> {playerOne, playerTwo};

			machine = new StateMachine<State, Trigger>(State.WaitingForSelection);
			clickWeapon = machine.SetTriggerParameters<Unit>(Trigger.ClickWeapon);

			machine.Configure(State.WaitingForSelection)
				.Permit(Trigger.ClickWeapon, State.Selecting);

			machine.Configure(State.Selecting)
				.OnEntryFrom(clickWeapon, unit => TryToSelectWeapon(unit))
				.Permit(Trigger.AssignWeapon, State.Attacking)
				.Permit(Trigger.InvalidSelection, State.WaitingForSelection);

			machine.Configure (State.Attacking)
				.OnEntry(SwapWaitingUnit)
				.Permit(Trigger.ClickWeapon, State.ResolvingAttack);

			machine.Configure (State.ResolvingAttack)
				.OnEntryFrom(clickWeapon, unit => ResolveAttack(unit))
				.Permit (Trigger.PlayerDead, State.PlayerWins)
				.Permit (Trigger.NextTurn, State.WaitingForSelection)
				.Permit (Trigger.AssignWeapon, State.Attacking)
				.OnExit(() => SwitchPlayers());

			CurrentPlayer.WaitForCommand();
		}

		// Probably could be a weapon, not a unit
		public void Select (Unit unit)
		{
			machine.Fire(clickWeapon, unit);
		}

		public void AddUnitToPlayer(string playerName, Unit unit)
		{
			var player = players.SingleOrDefault(p => p.Name == playerName);
			if (player == null)
				throw new InvalidPlayer("The Player Name was not found in the game");

			player.AddUnit(unit);
		}

		private void TryToSelectWeapon(Unit weapon)
		{
			if (CurrentPlayer.Owns(weapon))
			{
				CurrentPlayer.SetWeapon(weapon);
				machine.Fire(Trigger.AssignWeapon);
			}
			else
			{
				machine.Fire(Trigger.InvalidSelection);
			}
		}

		private void SwapWaitingUnit()
		{
			CurrentPlayer.StopWaitingForCommand();
			Opponent.WaitForAttack();
		}

		private void SwitchPlayers()
		{
			players.Reverse();
		}

		private void ResolveAttack(Unit enemy)
		{
			if (CurrentPlayer.Owns (enemy) )
			{
				CurrentPlayer.SetWeapon(enemy);
				machine.Fire (Trigger.AssignWeapon);
			}
			else
			{
				enemy.AttackWith(CurrentPlayer.Weapon());
				
				if (players[1].LivingUnits() == 0)
					machine.Fire (Trigger.PlayerDead);
				else 
					machine.Fire (Trigger.NextTurn);
			}
		}
	}
}