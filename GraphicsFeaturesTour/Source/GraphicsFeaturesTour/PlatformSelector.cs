using System.Linq;
using FlaxEngine;

/// <summary>
/// Enables the actor that is attached to on a given set of allowed platforms.
/// </summary>
public class PlatformSelector : Script
{
    public PlatformType[] Platforms;
    public bool Invert;
    public bool UseLowGraphicsQuality = true;
    public bool DisableDynamicShadows = true;

    /// <inheritdoc />
    public override void OnAwake()
    {
        if (Platforms == null || !Enabled)
            return;
        Actor.IsActive = Platforms.Contains(Platform.PlatformType) ^ Invert;
        if (Actor.IsActiveInHierarchy)
        {
            if (UseLowGraphicsQuality)
            {
                Graphics.AAQuality = Quality.Low;
                Graphics.SSRQuality = Quality.Low;
                Graphics.ShadowMapsQuality = Quality.Low;
                Graphics.ShadowsQuality = Quality.Low;
                Graphics.VolumetricFogQuality = Quality.Low;
            }

            if (DisableDynamicShadows)
            {
                DoDisableDynamicShadows(Scene);
            }
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
