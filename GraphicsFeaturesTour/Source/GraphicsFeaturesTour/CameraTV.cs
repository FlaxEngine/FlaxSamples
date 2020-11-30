using System;
using FlaxEngine;

namespace GraphicsFeaturesTour
{
    /// <summary>
    /// Simple script that renders the camera view to the texture and draws it with material on model.
    /// </summary>
    /// <seealso cref="FlaxEngine.Script" />
    public class CameraTV : Script
    {
        /// <summary>
        /// The camera to draw.
        /// </summary>
        public Camera Cam;

        /// <summary>
        /// The material to use for rendering. Needs to have texture parameter `Image`.
        /// </summary>
        public MaterialBase Material;

        /// <summary>
        /// The camera preview resolution.
        /// </summary>
        [Limit(1, 2000)]
        public Vector2 Resolution
        {
            get => _resolution;
            set
            {
                value = Vector2.Clamp(value, new Vector2(1), new Vector2(2000));
                if (_resolution != value)
                {
                    _resolution = value;
                    if (_output)
                    {
                        // Resize backbuffer
                        UpdateOutput();
                    }
                }
            }
        }

        /// <summary>
        /// The max distance from player to the TV when rendering is enabled. Used to cull additional work if TV is far away from the player.
        /// </summary>
        public float ViewDistance = 2000;

        private Vector2 _resolution = new Vector2(640, 374);
        private GPUTexture _output;
        private SceneRenderTask _task;
        private MaterialInstance _material;

        private void UpdateOutput()
        {
            var desc = GPUTextureDescription.New2D(
                (int) _resolution.X,
                (int) _resolution.Y,
                PixelFormat.R8G8B8A8_UNorm);
            _output.Init(ref desc);
        }

        /// <inheritdoc />
        public override void OnEnable()
        {
            // Create backbuffer
            if (_output == null)
                _output = new GPUTexture();
            UpdateOutput();

            // Create rendering task
            if (_task == null)
                _task = new SceneRenderTask();
            _task.Order = -100;
            _task.Camera = Cam;
            _task.Output = _output;
            _task.ViewFlags = ViewFlags.Reflections | ViewFlags.Decals | ViewFlags.AO | ViewFlags.GI | ViewFlags.DirectionalLights | ViewFlags.PointLights | ViewFlags.SpotLights | ViewFlags.SkyLights | ViewFlags.Shadows | ViewFlags.SpecularLight | ViewFlags.CustomPostProcess | ViewFlags.ToneMapping;
            _task.Enabled = false;

            if (Material && _material == null)
            {
                // Use dynamic material instance
                if (Material.WaitForLoaded())
                    throw new Exception("Failed to load material.");
                _material = Material.CreateVirtualInstance();

                // Set render task output to draw on model
                _material.SetParameterValue("Image", _output);

                // Bind material to parent model
                if (Actor is StaticModel staticModel && staticModel.Model)
                {
                    staticModel.Model.WaitForLoaded();
                    staticModel.SetMaterial(0, _material);
                }
            }

            _task.Enabled = true;
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
            _task.Enabled = Vector3.Distance(Actor.Position, MainRenderTask.Instance.View.Position) <= ViewDistance;
        }

        /// <inheritdoc />
        public override void OnDisable()
        {
            // Unbind temporary material
            if (Actor is StaticModel staticModel && staticModel.Model && staticModel.Model.IsLoaded)
                staticModel.SetMaterial(0, Material);

            // Ensure to cleanup resources
            Destroy(ref _task);
            Destroy(ref _output);
            Destroy(ref _material);
        }
    }
}
