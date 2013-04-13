using System;

namespace CubicleWarsLibrary
{
	namespace Effects
	{
		public class SineWave
		{
			protected float Amplitude { get;  set; }
			protected float Frequency { get; set; }
			protected float Offset { get; set; }

			public SineWave(float amplitude, float frequency, float offset)
			{
				Amplitude = amplitude;
				Frequency = frequency;
				Offset = offset;
			}

			public float at(float time)
			{
				return (Amplitude * (float)Math.Sin(Frequency * time)) + Offset;
			}
		}
	}
}

