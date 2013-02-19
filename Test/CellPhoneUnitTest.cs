using System;
using NUnit.Framework;
using NSubstitute;

namespace CubicleWarsLibrary
{
	[TestFixture]
	public class CellPhoneUnitTest
	{
		[Test]
		public void ItDoesNothingWhenAttackedSoFar()
		{
			var unit = Substitute.For<Unit>();
			var cellPhone = new CellPhoneUnit();

			cellPhone.AttackWith(unit);
		}
	}
}

