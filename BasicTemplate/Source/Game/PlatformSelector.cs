using System.Linq;
using FlaxEngine;

/// <summary>
/// Enables the actor that is attached to on a given set of allowed platforms.
/// </summary>
public class PlatformSelector : Script
{
    private bool _restorePrev;
    private Quality _prevAAQuality, _prevSSRQuality, _prevSSAOQuality, _prevShadowMapsQuality, _prevShadowsQuality, _prevVolumetricFogQuality;

    /// <summary>
    /// List of platforms to scale their quality down.
    /// </summary>
    public PlatformType[] Platforms = { PlatformType.Android, PlatformType.iOS, PlatformType.Switch };

    /// <summary>
    /// Inverts the Platforms list check.
    /// </summary>
    public bool Invert;

#if FLAX_EDITOR
    /// <summary>
    /// Enables visual preview in Editor (during play-mode).
    /// </summary>
    public bool PreviewInEditor = false;
#endif

    /// <summary>
    /// Forces graphics quality to Low.
    /// </summary>
    public bool UseLowGraphicsQuality = true;

    /// <summary>
    /// Disables any dynamic shadows on all lights in the level (sets ShadowsCastingMode.StaticOnly).
    /// </summary>
    public bool DisableDynamicShadows = true;

    /// <inheritdoc />
    public override void OnAwake()
    {
        if (!Enabled)
            return;
        bool enable = Platforms != null && (Platforms.Contains(Platform.PlatformType) ^ Invert);
#if FLAX_EDITOR
        enable |= PreviewInEditor;
#endif
        Actor.IsActive = enable;
        if (!Actor.IsActiveInHierarchy)
            return;

        if (UseLowGraphicsQuality)
        {
            _restorePrev = true;
            _prevAAQuality = Graphics.AAQuality;
            _prevSSRQuality = Graphics.SSRQuality;
            _prevSSAOQuality = Graphics.SSAOQuality;
            _prevShadowMapsQuality = Graphics.ShadowMapsQuality;
            _prevShadowsQuality = Graphics.ShadowsQuality;
            _prevVolumetricFogQuality = Graphics.VolumetricFogQuality;

            Graphics.AAQuality = Quality.Low;
            Graphics.SSRQuality = Quality.Low;
            Graphics.SSAOQuality = Quality.Low;
            Graphics.ShadowMapsQuality = Quality.Low;
            Graphics.ShadowsQuality = Quality.Low;
            Graphics.VolumetricFogQuality = Quality.Low;
        }

        if (DisableDynamicShadows)
        {
            DoDisableDynamicShadows(Scene);
        }
    }

    public override void OnDisable()
    {
        if (_restorePrev)
        {
            _restorePrev = false;
            Graphics.AAQuality = _prevAAQuality;
            Graphics.SSRQuality = _prevSSRQuality;
            Graphics.SSAOQuality = _prevSSAOQuality;
            Graphics.ShadowMapsQuality = _prevShadowMapsQuality;
            Graphics.ShadowsQuality = _prevShadowsQuality;
            Graphics.VolumetricFogQuality = _prevVolumetricFogQuality;
        }
    }

    private void DoDisableDynamicShadows(Actor a)
    {
        if (a is LightWithShadow l)
        {
            l.ContactShadowsLength = 0;
            l.ShadowsMode = ShadowsCastingMode.StaticOnly;
        }

        for (int i = 0; i < a.ChildrenCount; i++)
            DoDisableDynamicShadows(a.GetChild(i));
    }
}
