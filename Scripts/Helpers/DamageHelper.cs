using IntoTheCrypt.Messages;
using IntoTheCrypt.Models;
using System;

namespace IntoTheCrypt.Helpers
{
    public static class DamageHelper
    {
        #region Public

        #region Static Methods
        public static void Damage(Stats stats, uint damage)
        {
            stats.HP -= damage;
        }

        public static uint DamageFromBleed(Stats stats)
        {
            return (uint)stats.Bleed;
        }

        public static void HandleDamage(Stats enemy, DamageEnemyMessage damage) => HandleDamage(enemy, damage as IDamageMessage);

        public static void HandleDamage(Stats player, DamagePlayerMessage damage) => HandleDamage(player, damage as IDamageMessage);
        #endregion

        #endregion

        #region Private

        #region Classes
        private readonly struct DamageOverview
        {
            #region Public

            #region Constructors
            public DamageOverview(
                uint bleed,
                uint damage,
                uint shred
            )
            {
                Bleed = bleed;
                Damage = damage;
                Shred = shred;
            }
            #endregion

            #region Members
            public readonly uint Bleed;
            public readonly uint Damage;
            public readonly uint Shred;
            #endregion

            #endregion
        }
        #endregion

        #region Constants
        private const double StrengthLogBase = 10d;
        private const float StrengthLogFactor = 30f;
        private const float DamageToShredFactor = 1f;
        private const float DamageToBleedFactor = 1f;
        #endregion

        #region Static Methods
        private static DamageOverview GetDamageOverview(Stats stats, IDamageMessage damageMessage)
        {
            var damage = (uint)(
                Math.Ceiling(
                    Math.Log(
                        (double)(stats.Strength + 1),
                        StrengthLogBase
                    )
                    * StrengthLogFactor
                )
            );
            uint shred = 0;
            uint bleed = 0;
            switch (damageMessage.DamageClass)
            {
                case DamageClass.Blunt:
                    shred = (uint)(damage * DamageToShredFactor * QualityToShredFactor(damageMessage.Quality));
                    break;
                case DamageClass.Sharp:
                    bleed = (uint)(damage * DamageToBleedFactor * QualityToBleedFactor(damageMessage.Quality));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return new DamageOverview(
                bleed: bleed,
                damage: damage,
                shred: shred
            );
        }

        private static void HandleDamage(Stats stats, IDamageMessage damageMessage)
        {
            var damageOverview = GetDamageOverview(stats, damageMessage);
            
            var damage = damageMessage.Damage + damageOverview.Damage;
            damage = MitigateDamage(stats, damage);
            var bleed = damageOverview.Bleed;
            bleed = MitigateBleed(stats, bleed);
            var toxic = damageMessage.Toxicity;
            toxic = MitigateToxic(stats, toxic);

            InflictBleed(stats, bleed);
            InflictToxic(stats, toxic);
            Damage(stats, damage);
            
            ShredArmor(stats, damageOverview.Shred);
        }

        private static void InflictBleed(Stats stats, uint bleed)
        {
            stats.Bleed += bleed;
        }

        private static void InflictToxic(Stats stats, uint toxic)
        {
            if (!stats.IsBleeding)
            {
                return;
            }
            stats.Toxic += toxic;
        }

        private static uint MitigateBleed(Stats stats, uint bleed)
        {
            var tempBleed = (float)bleed;
            if (stats.ArmorRating > 0)
            {
                tempBleed *= Math.Min(bleed / stats.ArmorRating, 1f);
            }
            return (uint)tempBleed;
        }

        private static uint MitigateDamage(Stats stats, uint damage)
        {
            float rawDamage = damage;
            float factor = Math.Min(1f, rawDamage / stats.ArmorRating);

            return (uint)Math.Ceiling(rawDamage * factor);
        }

        private static uint MitigateToxic(Stats stats, uint toxic)
        {
            var toxicResistance = stats.ToxicResistance;
            float factor = 1f;
            var tempToxic = (float)toxic;
            if (toxicResistance > tempToxic)
            {
                factor = tempToxic / toxicResistance;
            }
            return (uint)(tempToxic * factor);
        }

        private static float QualityToBleedFactor(Quality quality)
        {
            switch (quality)
            {
                case Quality.E:
                    return 0.7f;
                case Quality.D:
                    return 0.8f;
                case Quality.C:
                    return 0.9f;
                case Quality.B:
                    return 1.0f;
                case Quality.A:
                    return 1.1f;
                case Quality.S:
                    return 1.2f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private static float QualityToShredFactor(Quality quality)
        {
            switch (quality)
            {
                case Quality.E:
                    return 0.2f;
                case Quality.D:
                    return 0.3f;
                case Quality.C:
                    return 0.4f;
                case Quality.B:
                    return 0.5f;
                case Quality.A:
                    return 0.6f;
                case Quality.S:
                    return 0.7f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ShredArmor(Stats stats, uint shred)
        {
            stats.ArmorRating -= shred;
        }
        #endregion

        #endregion
    }
}
