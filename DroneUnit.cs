using System;

namespace CubicleWarsLibrary
{
	public class DroneUnit : Unit
	{
		public DroneUnit ()
		{
		}

		public void AttackWith(Unit enemy) {}

		public bool Alive ()
		{
			return true;
		}

		public int AttackStrengthAgainst(Unit enemy)
		{
			return 0;
		}
	}
}

