using FlaxEngine;

namespace GraphicsFeaturesTour
{
	public class ObjectRotate : Script
	{
	    private Quaternion _originalRotation;
        
	    [Limit(-20, 20, 0.01f), Tooltip("Object rotation speed")]
	    public float RotateSpeed = 1.0f;
        
        public override void OnEnable()
	    {
	        _originalRotation = Actor.Orientation;
	    }

        public override void OnDisable()
	    {
	        Actor.Orientation = _originalRotation;
	    }
        
        public override void OnFixedUpdate()
		{
		    var euler = Actor.LocalEulerAngles;
		    euler.Y += RotateSpeed * Time.DeltaTime * 20;
		    Actor.LocalEulerAngles = euler;
        }
	}
}
