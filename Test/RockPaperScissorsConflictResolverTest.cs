using System;
using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;

namespace CubicleWarsLibrary
{
	public class RockPaperScissorsConflictResolverTest
	{
		[Test]
		public void ItLooksUpTheDamageFromTheUnits()
		{
			var dictionary = new Dictionary<string, Dictionary<string, int>>();
			dictionary["hacker"] = new Dictionary<string, int>();
			dictionary["hacker"]["cellPhone"] = 10;

			var hacker = Substitute.For<Unit>();
			hacker.UnitName.Returns("hacker");
			var cellPhone = Substitute.For<Unit>();
			cellPhone.UnitName.Returns("cellPhone");

			var resolver = new RockPaperScissorsConflictResolver(dictionary);

			Assert.AreEqual(10, resolver.Resolve(hacker, cellPhone));
		}

		[Test]
		public void ItHasNoDamageIfTheEnemyIsNotInTheTable()
		{
			var dictionary = new Dictionary<string, Dictionary<string, int>>();
			dictionary["hacker"] = new Dictionary<string, int>();

			var hacker = Substitute.For<Unit>();
			hacker.UnitName.Returns("hacker");
			var cellPhone = Substitute.For<Unit>();
			cellPhone.UnitName.Returns("cellPhone");
			
			var resolver = new RockPaperScissorsConflictResolver(dictionary);
			
			Assert.AreEqual(0, resolver.Resolve(hacker, cellPhone));
		}
	}
}

