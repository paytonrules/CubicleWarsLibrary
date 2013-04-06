using System;
using System.Collections;
using System.Collections.Generic;

namespace CubicleWarsLibrary
{
	public class CubicleWarsStateMachine
	{
		public enum State {
			Selecting,
			Attacking,
			PlayerWins
		};

		public State CurrentState { get; protected set; }
		protected List<Player> players;
		public Player CurrentPlayer 
		{
			get 
			{ 
				return players[0]; 
			}
		}

		public CubicleWarsStateMachine(Player playerOne, Player playerTwo)
		{
			players = new List<Player> {playerOne, playerTwo};

			CurrentState = State.Selecting;
		}

		// Probably could be a weapon, not a unit
		public void Select (Unit unit)
		{
			if (CurrentPlayer.Owns (unit)) {
				CurrentPlayer.SetWeapon(unit);
				CurrentState = State.Attacking;
			} else if (CurrentState == State.Attacking) {
				Attack (unit);
			}
		}

		private void Attack(Unit unit)
		{
			unit.AttackWith(CurrentPlayer.Weapon());
			
			if (players[1].LivingUnits() == 0)
				CurrentState = State.PlayerWins;
			else 
				CurrentState = State.Selecting;

			players.Reverse();
		}
	}
}