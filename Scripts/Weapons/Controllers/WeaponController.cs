using Godot;

namespace IntoTheCrypt.Weapons.Controllers
{
    public abstract class WeaponController : Spatial
    {
        public abstract WeaponStats Stats { get; protected set; }
    }
}