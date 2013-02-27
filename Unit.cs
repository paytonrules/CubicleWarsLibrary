using System;

namespace CubicleWarsLibrary
{
	public interface Unit
	{
		void AttackWith(Unit enemy);
		bool Alive();
		int AttackStrengthAgainst(Unit enemy);
	}
}

