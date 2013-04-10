using System;

namespace CubicleWarsLibrary
{
	public delegate void WaitingEvent();

	public interface Unit
	{
		void AttackWith(Unit enemy);
		bool Alive();
		int AttackStrengthAgainst(Unit enemy);
		string UnitName { get; }

		event WaitingEvent Waiting;
	}
}

