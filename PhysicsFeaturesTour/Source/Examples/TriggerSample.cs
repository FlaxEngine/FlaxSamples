using FlaxEngine;

namespace PhysicsFeaturesTour
{
	public class TriggerSample : Script
	{
		[Serialize]
		private bool _lightOn;

		public Light LightToControl;

		[NoSerialize]
		public bool LightOn
		{
			get { return _lightOn; }
			set
			{
				_lightOn = value;
				if (LightToControl)
					LightToControl.Color = value ? Color.Green : Color.Red;
			}
		}

		void Start()
		{
			// Restore state
			LightOn = _lightOn;
		}

		void OnTriggerEnter(Collider c)
		{
			// Check player
			if (c is CharacterController)
			{
				LightOn = true;
			}
		}

		void OnTriggerExit(Collider c)
		{
			// Check player
			if (c is CharacterController)
			{
				LightOn = false;
			}
		}
	}
}
