using System;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace GraphicsFeaturesTour
{
    public class CameraTV : Script
    {
        public Camera Cam;
        public MaterialBase Material;

        [Limit(1, 2000)]
        public Vector2 Resolution
        {
            get { return _resolution; }
            set
            {
                value = Vector2.Clamp(value, new Vector2(1), new Vector2(2000));
                if (_resolution != value)
                {
                    _resolution = value;
                    if (_output)
                    {
                        // Resize backbuffer
                        _output.Init(PixelFormat.R8G8B8A8_UNorm, (int) _resolution.X, (int) _resolution.Y);
                    }
                }
            }
        }

        private Vector2 _resolution = new Vector2(640, 374);
        private RenderTarget _output;
        private SceneRenderTask _task;
        private MaterialInstance _material;

        public override void OnEnable()
        {
            // Create backbuffer
            if (_output == null)
                _output = RenderTarget.New();
            _output.Init(PixelFormat.R8G8B8A8_UNorm, (int) _resolution.X, (int) _resolution.Y);

            // Create rendering task
            if (_task == null)
                _task = FlaxEngine.Object.New<SceneRenderTask>();
            _task.Order = -100;
            _task.Camera = Cam;
            _task.Output = _output;
            _task.Enabled = false;
            
            if (Material && _material == null)
            {
                // Use dynamic material instance
                if (Material.WaitForLoaded())
                    throw new Exception("Failed to load material.");
                _material = Material.CreateVirtualInstance();

                // Set render task output to draw on model
                _material.GetParam("Image").Value = _output;

                // Bind material to parent model
                if (Actor is StaticModel staticModel && staticModel.Model)
                {
                    staticModel.Model.WaitForLoaded();
                    staticModel.Entries[0].Material = _material;
                }
            }

            _task.Enabled = true;
        }

        public override void OnDisable()
        {
            // Ensure to cleanup resources
            Destroy(ref _task);
            Destroy(ref _output);
            Destroy(ref _material);
        }
    }
}