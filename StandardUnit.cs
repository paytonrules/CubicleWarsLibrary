using System;

namespace CubicleWarsLibrary
{
	public class StandardUnit : Unit
	{
		public int Health { get; set; }
		public string UnitName { get; set; }

		protected ConflictResolver Resolver { get; set; }
		protected UnityObject Unity { get; set; }

		public StandardUnit(ConflictResolver resolver, UnityObject unity)
		{
			Resolver = resolver;
			Unity = unity;
		}

		public void AttackWith (Unit enemy)
		{
			Health -= enemy.AttackStrengthAgainst (this);
			if (Unity != null) {
				Unity.Attacked ();
			}
		}

		public bool Alive()
		{
			return Health != 0;
		}

		public int AttackStrengthAgainst(Unit enemy)
		{
			return Resolver.Resolve(enemy, this);
			/*
			var lookupTable = new Dictionary<Type, Dictionary<Type, int>>();

			lookupTable[typeof(CellPhoneUnit)] = new Dictionary<Type, int>();
			lookupTable[typeof(CellPhoneUnit)][typeof(DroneUnit)] = 1;
			lookupTable[typeof(CellPhoneUnit)][typeof(HackerUnit)] = 0;

			return lookupTable[this.GetType()][enemy.GetType()];*/
			/*if (enemy is DroneUnit) {
				return 1;
			}

			return 0;*/
		}
		// One basic unit they all use
		// Write the conflict resolver
	}
}

