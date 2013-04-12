using System;

namespace CubicleWarsLibrary
{
	public delegate void WaitingEvent();
	public delegate void DoneWaitingEvent();

	public interface Unit
	{
		void AttackWith(Unit enemy);
		bool Alive();
		int AttackStrengthAgainst(Unit enemy);
		string UnitName { get; }
		void Observe(StateMachine stateMachine);

		event WaitingEvent Waiting;
		event DoneWaitingEvent DoneWaiting;
	}
}

