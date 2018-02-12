using FlaxEngine;

namespace PhysicsFeaturesTour
{
	public class MovingObj : Script
	{
	    private Vector3 _startPos;
	    private float _startTime;

	    public Vector3 MoveAxis = Vector3.UnitX;

	    public float Speed = 1;

	    public float Amplitude = 50;

		private void OnEnable()
		{
		    _startTime = Time.GameTime;
            _startPos = Actor.LocalPosition;
		}

	    private void OnDisable()
	    {
	        Actor.LocalPosition = _startPos;
	    }

		private void Update()
		{
		    float pos = Mathf.Sin((Time.GameTime - _startTime) * Speed) * Amplitude;
		    Actor.LocalPosition = _startPos + MoveAxis * pos;
		}
	}
}
