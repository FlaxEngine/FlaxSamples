using FlaxEngine;

public class ExitOnEsc : Script
{
    public override void OnUpdate()
    {
        if (Input.GetKeyUp(KeyboardKeys.Escape))
            Engine.RequestExit();
    }
}
