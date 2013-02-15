using System;

namespace CubicleWarsLibrary
{
	public class HackerUnit : Unit
	{
		public int Health { get; protected set; }
		public const int MAX_HEALTH = 10;
		protected UnityObject Unity { get; set; }

		public HackerUnit(UnityObject unity)
		{
			Health = MAX_HEALTH;
			Unity = unity;
		}

		public void AttackWith(Unit enemy)
		{
			Health -= 1;
			Unity.Attacked();
		}
	}
}

