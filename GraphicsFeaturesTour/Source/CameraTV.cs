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
			get { return resolution; }
			set
			{
				value = Vector2.Clamp(value, new Vector2(1), new Vector2(2000));
				if (resolution != value)
				{
					resolution = value;
					if (output)
					{
						output.Init(PixelFormat.R8G8B8A8_UNorm, (int) resolution.X, (int) resolution.Y);
					}
				}
			}
		}

		private Vector2 resolution = new Vector2(640, 374);
		private RenderTarget output;
		private SceneRenderTask task;
		private MaterialInstance material;
		private bool setMaterial;

		private void OnEnable()
		{
			// Create backbuffer
			if (output == null)
				output = RenderTarget.New();
			output.Init(PixelFormat.R8G8B8A8_UNorm, (int) resolution.X, (int) resolution.Y);

			// Create rendering task
			if (task == null)
				task = RenderTask.Create<SceneRenderTask>();
			task.Order = -100;
			task.Camera = Cam;
			task.Output = output;
			task.Enabled = false;

			// Use dynamic material instance
			if (Material && material == null)
				material = Material.CreateVirtualInstance();
			setMaterial = true;
		}

		private void OnDisable()
		{
			Destroy(ref task);
			Destroy(ref output);
			Destroy(ref material);
		}

		private void Update()
		{
			task.Enabled = true;

			if (setMaterial)
			{
				setMaterial = false;

				if (material)
				{
					material.GetParam("Image").Value = output;
				}

				if (Actor is ModelActor)
				{
					var modelActor = (ModelActor) Actor;
					if (modelActor.HasContentLoaded)
					{
						modelActor.Entries[0].Material = material;
					}
				}
			}
		}
	}
}
