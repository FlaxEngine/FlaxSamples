using FlaxEngine;

namespace Game
{
    /// <summary>
    /// Handles weapon functions as well as bullet spawning.
    /// </summary>
    public class Weapon : Script
    {
        [EditorOrder(0), AssetReference(typeof(WeaponData))]
        public JsonAsset WeaponSettings;

        [EditorOrder(1)]
        public Actor BulletSpawnPoint;

        private float _timeToFire;
        private WeaponData _data;

        public override void OnStart()
        {
            base.OnStart();

            if (WeaponSettings == null)
            {
                Debug.LogError("No settings for the weapon were found");
                return;
            }

            _data = WeaponSettings.CreateInstance<WeaponData>();
        }

        public override void OnUpdate()
        {
            if (Input.GetAction("Fire") && Time.GameTime >= _timeToFire)
            {
                _timeToFire = Time.GameTime + 1.0f / _data.FireRate;

                var bullet = new RigidBody
                {
                    Name = "Bullet",
                    StaticFlags = StaticFlags.None,
                    UseCCD = true,
                };

                new StaticModel
                {
                    Model = _data.BulletModel,
                    Parent = bullet,
                    StaticFlags = StaticFlags.None
                };

                // Ideally you would also use raycasting to determine if something was hit
                new SphereCollider
                {
                    Parent = bullet,
                    StaticFlags = StaticFlags.None
                };

                bullet.Transform = new Transform(BulletSpawnPoint.Position, Quaternion.Identity, _data.BulletScale);
                Level.SpawnActor(bullet);

                bullet.LinearVelocity = BulletSpawnPoint.Direction * _data.BulletVelocity;

                Destroy(bullet, _data.BulletLifetime);
            }
        }
    }
}
