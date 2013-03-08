using System;
using System.Collections.Generic;

namespace CubicleWarsLibrary
{
	public class RockPaperScissorsConflictResolver : ConflictResolver
	{
		protected Dictionary<string, Dictionary<string, int>> ConflictTable;

		public RockPaperScissorsConflictResolver(Dictionary<string, Dictionary<string, int>> conflictTable)
		{
			ConflictTable = conflictTable;
		}

		public int Resolve(Unit attacker, Unit defender)
		{
			if (ConflictTable[attacker.UnitName].ContainsKey(defender.UnitName))
				return ConflictTable[attacker.UnitName][defender.UnitName];
			else
				return 0;
		}
	}
}

