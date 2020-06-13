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

        public static void HandleDamage(Stats enemy, DamageEnemyMessage damage) => HandleDamage(enemy, damage as DamageMessage);

        public static void HandleDamage(Stats player, DamagePlayerMessage damage) => HandleDamage(player, damage as DamageMessage);
        #endregion

        #endregion

        #region Private

        #region Static Methods
        private static void HandleDamage(Stats stats, DamageMessage damage)
        {
            var totalDamage = MitigateDamageWithArmor(stats.Armor, damage);
            IncreaseBleed(stats, damage);
            IncreaseToxic(stats, damage);
            Damage(stats, totalDamage);
        }

        private static void IncreaseBleed(Stats stats, DamageMessage damage)
        {
            float bleed = damage.Sharpness;
            if (stats.ArmorRating > 0)
            {
                bleed *= Math.Min(bleed / stats.ArmorRating, 1f);
            }

            stats.Bleed += (uint)bleed;
        }

        private static void IncreaseToxic(Stats stats, DamageMessage damage)
        {
            if (!stats.IsBleeding)
            {
                return;
            }
            float toxic = damage.Toxicity;
            if (stats.ToxicResistance > 0)
            {
                toxic = MitigateToxicIncrease(stats, toxic);
            }

            stats.Toxic += (uint)toxic;
        }

        private static uint MitigateDamageWithArmor(Armor armor, DamageMessage damage)
        {
            float rawDamage = damage.Damage;
            float factor = Math.Min(1f, rawDamage / armor.Rating);

            return (uint)Math.Ceiling(rawDamage * factor);
        }

        private static float MitigateToxicIncrease(Stats stats, float toxic)
        {
            var toxicResistance = stats.ToxicResistance;
            float factor = 1f;
            if (toxicResistance > toxic)
            {
                factor = toxic / toxicResistance;
            }
            return toxic * factor;
        }
        #endregion

        #endregion
    }
}
