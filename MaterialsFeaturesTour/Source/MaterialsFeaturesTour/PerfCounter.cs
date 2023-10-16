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
#if !BUILD_RELEASE && FLAX_1_7_OR_NEWER
        if (_label.Visible)
        {
            ProfilerGPU.Enabled = true; // Force enable GPU profiler to get GPU timings
        }
#endif
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
        if (!_label.Visible)
            return;
#if !BUILD_RELEASE
        var stats = ProfilingTools.Stats;
        _label.Text = string.Format(_format, stats.FPS, stats.DrawGPUTimeMs, stats.DrawCPUTimeMs, stats.UpdateTimeMs);
#else
        _label.Text = string.Format(_format, Engine.FramesPerSecond, 0, 0, 0);
#endif
    }
}
