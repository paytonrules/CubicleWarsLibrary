using System;

namespace CubicleWarsLibrary
{
	public delegate void AttackedEvent();

	public class StandardUnit : Unit
	{
		public int Health { get; protected set; }
		public string UnitName { get; set; }
		public event AttackedEvent Attacked;
		public event WaitingEvent Waiting;
		public event DoneWaitingEvent DoneWaiting;
		protected ConflictResolver Resolver { get; set; }

		public StandardUnit(ConflictResolver resolver, UnityObject unity)
		{
			Resolver = resolver;
			Health = unity.Health;
		}

		public void AttackWith (Unit enemy)
		{
			Health -= enemy.AttackStrengthAgainst (this);
			if (Attacked != null) {
				Attacked();
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

		public void NotReadyForCommand ()
		{
			if (DoneWaiting != null)
				DoneWaiting();
		}
		
		public void PickMe ()
		{
			if (Waiting != null)
				Waiting();
		}
	}
}

