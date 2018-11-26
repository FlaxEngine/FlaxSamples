using FlaxEngine;

namespace PhysicsFeaturesTour
{
	public class CollisionSample : Script
	{
		private float timeout;
		
		public StaticModel ModelToControl;

		public MaterialBase MaterialOn;
		public MaterialBase MaterialOff;

		void Start()
		{
			timeout = 0;
		}

		void Update()
		{
			bool isOn = false;
			if (timeout > 0)
			{
				timeout -= Time.UnscaledDeltaTime;
				isOn = timeout > 0;
			}
			if (ModelToControl)
				ModelToControl.Entries[0].Material = isOn ? MaterialOn : MaterialOff;
		}

		void OnCollisionEnter(Collision c)
		{
			timeout = 0.5f;
		}
	}
}
