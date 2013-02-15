using System;
using NUnit.Framework;
using NSubstitute;

namespace CubicleWarsLibrary
{
	[TestFixture]
	public class HackerUnitTest
	{
		[Test]
		public void ItLosesOneHealthWhenAttackedByAUnit()
		{
			var enemy = Substitute.For<Unit>();
			var unity = Substitute.For<UnityObject>();
			var hacker = new HackerUnit(unity);

			hacker.AttackWith(enemy);

			Assert.AreEqual(HackerUnit.MAX_HEALTH - 1, hacker.Health);
		}

		[Test]
		public void ItNotifiesItsUnityObjectWhenItsAttacked()
		{
			var unity = Substitute.For<UnityObject>();
			var hacker = new HackerUnit(unity);

			hacker.AttackWith(hacker);

			unity.Received().Attacked();
		}
	}
}

