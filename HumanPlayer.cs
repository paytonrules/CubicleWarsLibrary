using System;
using System.Collections;
using System.Collections.Generic;

namespace CubicleWarsLibrary
{
	public class HumanPlayer : Player
	{
		List<Unit> units;
		Unit currentWeapon;

		public HumanPlayer(IEnumerable<Unit> units)
		{
			this.units = new List<Unit>(units);
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