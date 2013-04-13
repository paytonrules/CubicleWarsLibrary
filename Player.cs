using System;

namespace CubicleWarsLibrary
{
	public interface Player
	{
		void SetWeapon(Unit unit);
		bool Owns(Unit unit);
		Unit Weapon();
		int LivingUnits();
		void WaitForCommand();
		void StopWaitingForCommand();
		void WaitForAttack();
	}
}