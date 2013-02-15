using System;
using System.Collections;
using System.Collections.Generic;

namespace CubicleWarsLibrary
{
	public class CubicleWarsStateMachine
	{
		protected enum State {
			Selecting,
			Attacking
		};

		protected State CurrentState { get; set; }
		protected ArrayList players;
		public Player CurrentPlayer 
		{
			get 
			{ 
				return (Player) players[0]; 
			}
		}

		public CubicleWarsStateMachine(Player playerOne, Player playerTwo)
		{
			players = new ArrayList {playerOne, playerTwo};

			CurrentState = State.Selecting;
		}
		
		public void SelectObject(Unit unit)
		{
			if (CurrentPlayer.Owns(unit))
			{
				CurrentPlayer.SetWeapon(unit);
				CurrentState = State.Attacking;
			}
		}

		public void Attack(Unit unit)
		{
			if (CurrentState == State.Attacking)
			{
				unit.AttackWith(CurrentPlayer.Weapon());
				players.Reverse();
				CurrentState = State.Selecting;
			}
		}
	}
}