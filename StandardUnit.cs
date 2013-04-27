using System;

namespace CubicleWarsLibrary
{

	public class StandardUnit : Unit
	{
		public int Health { get; protected set; }
		public string UnitName { get; set; }
		public event AttackedEvent Attacked = delegate {};
		public event WaitingEvent Waiting = delegate {};
		public event DoneWaitingEvent DoneWaiting = delegate {};
		public event DeathEvent Dead = delegate {};

		protected ConflictResolver Resolver { get; set; }

		public StandardUnit(ConflictResolver resolver, UnityObject unity)
		{
			Resolver = resolver;
			Health = unity.InitialHealth;
			UnitName = unity.Name;
		}

		public void AttackWith (Unit enemy)
		{
			Health -= enemy.AttackStrengthAgainst (this);
			Attacked();
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
			DoneWaiting();
		}
		
		public void PickMe ()
		{
			Waiting();
		}
	}
}

