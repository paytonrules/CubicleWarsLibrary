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

		public HumanPlayer(String name, IEnumerable<Unit> units)
		{
			this.units = new List<Unit>(units);
			this.Name = name;
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
			return units.Count(unit => unit.Alive());
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