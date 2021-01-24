using FlaxEngine;

namespace Game
{
    /// <summary>
    /// Contains values data that define the behavior of a weapon.
    /// </summary>
    public class WeaponData
    {
        [Range(0, 20), EditorOrder(0), EditorDisplay("Weapon Data")]
        public float FireRate = 20f;

        [Range(1000, 10000), EditorOrder(1), EditorDisplay("Weapon Data")]
        public float BulletVelocity = 1000f;

        [Limit(0, 20), EditorOrder(2), EditorDisplay("Weapon Data")]
        public int BulletLifetime = 10;

        [EditorOrder(3), EditorDisplay("Weapon Data")]
        public Model BulletModel;

        [EditorOrder(3), EditorDisplay("Weapon Data")]
        public Vector3 BulletScale = new Vector3(0.1f);
    }
}
