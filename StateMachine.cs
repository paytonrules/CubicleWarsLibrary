using System;

namespace CubicleWarsLibrary
{
	public delegate void StateChangedEventHandler(object sender, EventArgs e);

	public enum State {
		WaitingForSelection,
		Selecting,
		Attacking,
		PlayerWins,
		ResolvingAttack
	};
	
	public enum Trigger {
		ClickWeapon,
		AssignWeapon,
		PlayerDead,
		NextTurn,
		InvalidSelection
	};

	public interface StateMachine
	{
		event StateChangedEventHandler StateChanged;

		State CurrentState { get; }
		Player CurrentPlayer { get; } 
	}
}

