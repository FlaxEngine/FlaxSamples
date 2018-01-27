using FlaxEngine;

namespace GraphicsFeaturesTour
{
	public class FogControl : Script
	{
		public ExponentialHeightFog Fog;
		public Actor[] ActorsToToggle;

		private void OnTriggerEnter(Collider c)
		{
			if (Fog && c is CharacterController)
			{
				SetActive(true);
			}
		}

		private void OnTriggerExit(Collider c)
		{
			if (Fog && c is CharacterController)
			{
				SetActive(false);
			}
		}
		
		private void SetActive(bool value)
		{
			Fog.VolumetricFogEnable = value;

			if (ActorsToToggle != null)
			{
				for (int i = 0; i < ActorsToToggle.Length; i++)
				{
					if (ActorsToToggle[i] != null)
						ActorsToToggle[i].IsActive = value;
				}
			}
		}
	}
}
