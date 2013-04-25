using System;
using System.Collections;
using System.Collections.Generic;

namespace CubicleWarsLibrary
{
	public class HumanPlayer : Player
	{
		IList<Unit> units;
		Unit currentWeapon;

		public HumanPlayer(String name, IEnumerable<Unit> units)
		{
			this.units = new List<Unit>(units);
			this.Name = name;
		}

		public HumanPlayer(String name) : this(name, new Unit[] {})
		{
		}

		public string Name { get; protected set; }

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
			int count = 0;
			foreach(var unit in units) {
				if (unit.Alive())
					count++;
			}
			return count;
		}

		public void WaitForCommand()
		{
			foreach(var unit in units) {
				unit.PickMe();
			}
		}

		public void StopWaitingForCommand ()
		{
      		
			foreach(var unit in units) {
				unit.NotReadyForCommand();
			}
		}

		public void WaitForAttack()
		{
			foreach(var unit in units) {
				unit.PickMe();
			}
		}

		public void AddUnit (Unit unit)
		{
			units.Add (unit);
		}
	}
}
