using System;

namespace CubicleWarsLibrary
{
	public class CellPhoneUnit : Unit
	{
		public int Health { get; set; }
		public CellPhoneUnit()
		{

		}

		public void AttackWith(Unit enemy)
		{
			Health -= enemy.AttackStrengthAgainst(this);
		}

		public bool Alive()
		{
			return Health != 0;
		}

		public int AttackStrengthAgainst (Unit enemy)
		{
			if (enemy is DroneUnit) {
				return 1;
			}

			return 0;
		}
	}
}

