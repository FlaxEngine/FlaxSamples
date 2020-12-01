using FlaxEngine;
using FlaxEngine.GUI;

public class PerfCounter : Script
{
    private Label _label;
    private string _format;

    /// <inheritdoc />
    public override void OnEnable()
    {
        _label = Actor.As<UIControl>().Get<Label>();
        _format = _label.Text;
    }

    /// <inheritdoc />
    public override void OnDisable()
    {
        _label.Text = _format;
        _label = null;
    }

    /// <inheritdoc />
    public override void OnUpdate()
    {
#if !BUILD_RELEASE
        var stats = ProfilingTools.Stats;
        _label.Text = string.Format(_format, stats.FPS, stats.DrawGPUTimeMs, stats.DrawCPUTimeMs, stats.UpdateTimeMs);
#else
        _label.Text = string.Format(_format, Engine.FramesPerSecond, 0, 0, 0);
#endif
    }
}
