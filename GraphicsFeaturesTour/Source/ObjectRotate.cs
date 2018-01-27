using System;
using System.Collections.Generic;
using FlaxEngine;

namespace GraphicsFeaturesTour
{
	public class ObjectRotate : Script
	{
	    private Quaternion _originalRotation;
        
	    [Limit(-20, 20, 0.01f), Tooltip("Object rotation speed")]
	    public float RotateSpeed = 1.0f;
        
	    private void OnEnable()
	    {
	        _originalRotation = Actor.Orientation;
	    }

	    private void OnDisable()
	    {
	        Actor.Orientation = _originalRotation;
	    }
        
        private void FixedUpdate()
		{
		    var euler = Actor.LocalEulerAngles;
		    euler.Y += RotateSpeed * Time.DeltaTime * 20;
		    Actor.LocalEulerAngles = euler;
        }
	}
}
