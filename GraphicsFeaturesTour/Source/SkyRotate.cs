using FlaxEngine;

namespace GraphicsFeaturesTour
{
	public class SkyRotate : Script
	{
		private Quaternion _originalRotation;

        [Tooltip("Directional light to animate when player enters trigger area")]
	    public DirectionalLight Light;

		[Limit(-20, 20, 0.01f), Tooltip("Sun rotation speed")]
		public float RotateSpeed = 4.0f;
		
	    private bool _isPlayerInside;

		private void OnEnable()
		{
			if (Light)
				_originalRotation = Light.Orientation;
		}

		private void OnDisable()
		{
			if (Light)
				Light.Orientation = _originalRotation;
		}

		private void FixedUpdate()
		{
		    if (_isPlayerInside)
		    {
				// Rotate directional light
			    var euler = Light.EulerAngles;
			    euler.Y += RotateSpeed * Time.DeltaTime * 20;
			    Light.EulerAngles = euler;
		    }
		}

	    private void OnTriggerEnter(Collider c)
	    {
		    if (Light && c is CharacterController)
		    {
			    _isPlayerInside = true;
		    }
	    }

	    private void OnTriggerExit(Collider c)
	    {
            if (Light && c is CharacterController)
	        {
	            _isPlayerInside = false;
		        Light.Orientation = _originalRotation;
	        }
        }
    }
}
