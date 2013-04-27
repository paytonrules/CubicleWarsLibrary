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
		protected StateMachine<State, Trigger>.TriggerWithParameters<Player, Unit> addUnit;
		protected List<Player> players;

		public event GameOverEvent GameOver = delegate { };

		public CubicleWarsStateMachine(Player playerOne, Player playerTwo)
		{
			players = new List<Player> {playerOne, playerTwo};

			machine = new StateMachine<State, Trigger>(State.WaitingForSelection);
			clickWeapon = machine.SetTriggerParameters<Unit>(Trigger.ClickWeapon);
			addUnit = machine.SetTriggerParameters<Player, Unit>(Trigger.AddUnit);

			machine.Configure(State.WaitingForSelection)
				.Permit(Trigger.ClickWeapon, State.Selecting)
				.Permit(Trigger.AddUnit, State.AddingUnit); 

			machine.Configure (State.AddingUnit)
				.OnEntryFrom(addUnit, (player, unit) => AddUnit(player, unit))
				.Permit(Trigger.AddedUnit, State.WaitingForSelection);

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
				.Permit (Trigger.NextTurn, State.SwitchingPlayers)
				.Permit (Trigger.AssignWeapon, State.Attacking);

			machine.Configure (State.SwitchingPlayers)
				.OnEntry(SwitchPlayers)
				.Permit (Trigger.SwitchedPlayers, State.WaitingForSelection);

			machine.Configure(State.PlayerWins)
				.OnEntry(AnnouncePlayerWins);

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

			machine.Fire(addUnit, player, unit);
		}

		private void AddUnit(Player player, Unit unit)
		{
			player.AddUnit(unit);
			if (player == CurrentPlayer) {
				player.WaitForCommand();
			}

			machine.Fire (Trigger.AddedUnit);
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
			machine.Fire (Trigger.SwitchedPlayers);
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

		private void AnnouncePlayerWins()
		{
			GameOver(CurrentPlayer.Name);
		}
	}
}