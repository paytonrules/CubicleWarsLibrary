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

		public void Observe(StateMachine stateMachine)
		{
			stateMachine.StateChanged += delegate(object sender, EventArgs e) {
				if (Waiting != null 
				    && stateMachine.CurrentPlayer.Owns (this)
				    && stateMachine.CurrentState == State.WaitingForSelection) {
					Waiting();
				}
			};
		}
	}
}

