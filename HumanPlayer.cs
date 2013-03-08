using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CubicleWarsLibrary
{
	public class HumanPlayer : Player
	{
		IList<Unit> units;
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

		public int LivingUnits ()
		{
			return units.Count(unit => unit.Alive());
		}
	}
}