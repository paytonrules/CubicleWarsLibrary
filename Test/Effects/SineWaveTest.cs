using System;
using NUnit.Framework;

namespace CubicleWarsLibrary
{
	namespace Effects
	{
		public class SineWaveTest
		{
			[Test]
			public void ItReturnsANormalSinWaveOnTime()
			{
				var sineWave = new SineWave(1, 1, 0);

				Assert.AreEqual(0, sineWave.at(0));
				Assert.AreEqual(1, sineWave.at((float) Math.PI / 2.0f));
			}

			[Test]
			public void ItRespectsTheAmplitude()
			{
				var sineWave = new SineWave(2, 1, 0);

				Assert.AreEqual(2, sineWave.at ((float) Math.PI / 2.0f));
			}

			[Test]
			public void ItRespectsTheFrequency()
			{
				var sineWave = new SineWave(1, 2, 0);
				
				Assert.AreEqual(1, sineWave.at ((float) Math.PI / 4.0f));
			}

			[Test]
			public void ItRespectsTheOffset()
			{
				var sineWave = new SineWave(1, 1, 0.5f);

				Assert.AreEqual(0.5f, sineWave.at(0));
			}
		}
	}
}

