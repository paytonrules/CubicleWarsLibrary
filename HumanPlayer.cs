using System;
using System.Collections;

namespace CubicleWarsLibrary
{
	public class HumanPlayer : Player
	{
		ArrayList units;
		Unit currentWeapon;

		public HumanPlayer(ArrayList units)
		{
			this.units = units;
		}

		public void SetWeapon(Unit unit)
		{
			currentWeapon = unit;
		}
		
		public bool Owns(Unit unit)
		{
			return units.Contains(unit);
		}

		public Unit Weapon()
		{
			return currentWeapon;
		}
	}
}