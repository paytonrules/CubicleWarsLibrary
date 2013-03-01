using System;

namespace CubicleWarsLibrary
{
	public interface ConflictResolver
	{
		int Resolve(Unit attacker, Unit defender);
	}
}

