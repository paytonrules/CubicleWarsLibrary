using System;

namespace CubicleWarsLibrary
{
	public delegate void GameOverEvent(String winner);

	public enum State {
		AddingUnit,
		WaitingForSelection,
		Selecting,
		Attacking,
		PlayerWins,
		ResolvingAttack
	};
	
	public enum Trigger {
		AddUnit,
		AddedUnit,
		ClickWeapon,
		AssignWeapon,
		PlayerDead,
		NextTurn,
		InvalidSelection
	};

	public interface StateMachine
	{
		State CurrentState { get; }
		Player CurrentPlayer { get; } 
		void AddUnitToPlayer(String playerName, Unit unit);
		void Select(Unit unit);

		event GameOverEvent GameOver;
	}
}

