using System;

namespace CubicleWarsLibrary
{
	public interface Player
	{
		void SetWeapon(Unit unit);
		bool Owns(Unit unit);
		Unit Weapon();
	}
}

