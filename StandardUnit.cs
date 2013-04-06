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
		}
	}
}

