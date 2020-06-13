using System;
using UnityEngine;

namespace IntoTheCrypt.Models
{
    [System.Serializable]
    public class Stats
    {
        #region Public

        #region Constructors
        public Stats(
            uint maxHp,
            uint maxArmorRating,
            uint dexterity,
            uint strength,
            uint bleedResistance,
            uint toxicResistance)
            : this(
                new Armor(maxArmorRating),
                new Health(maxHp),
                new MajorStats(dexterity: dexterity, strength: strength),
                new MinorStats(bleedResistance: bleedResistance, toxicResistance: toxicResistance)
            )
        {
        }

        public Stats(Armor armor, Health health, MajorStats majorStats, MinorStats minorStats)
        {
            Armor = armor;
            Health = health;
            MajorStats = majorStats;
            MinorStats = minorStats;
        }
        #endregion

        #region Members

        public Armor Armor;
        public Health Health;
        public MajorStats MajorStats;
        public MinorStats MinorStats;

        #region Armor
        public uint ArmorRating
        {
            get => Armor.Rating;
            set => Armor.Rating = value;
        }

        public uint MaxArmorRating
        {
            get => Armor.MaxRating;
            set => Armor.MaxRating = value;
        }
        #endregion

        #region Health
        public float HP
        {
            get => Health.HP;
            set => Health.HP = value;
        }

        public float MaxHP
        {
            get => Health.MaxHP;
            set => Health.MaxHP = value;
        }
        #endregion

        #region Major Stats
        public uint Dexterity
        {
            get => MajorStats.Dexterity;
            set => MajorStats.Dexterity = value;
        }

        public uint Strength
        {
            get => MajorStats.Strength;
            set => MajorStats.Strength = value;
        }
        #endregion

        #region Minor Stats
        public uint BleedResistance
        {
            get => MinorStats.BleedResistance;
            set => MinorStats.BleedResistance = value;
        }
        public uint ToxicResistance
        {
            get => MinorStats.ToxicResistance;
            set => MinorStats.ToxicResistance = value;
        }
        #endregion

        #region Bleed
        public float Bleed
        {
            get => _bleed;
            set
            {
                if (value >= 0f)
                {
                    _bleed = value;
                }
                else
                {
                    _bleed = 0f;
                }
            }
        }
        public float BleedReductionRatio
        {
            get
            {
                if (BleedResistance > Bleed)
                {
                    return Bleed / BleedResistance;
                }
                return 0.9f;
            }
        }
        public bool IsBleeding => Bleed > 0f;
        #endregion

        #region Toxic
        public float Toxic
        {
            get => _toxic;
            set
            {
                if (value >= 0f)
                {
                    _toxic = value;
                }
                else
                {
                    _toxic = 0f;
                }
            }
        }
        public uint ToxicLevel => (uint)(Toxic / TOXIC_INCREASE_LEVEL);
        public uint TransientToxic => (uint)Toxic % TOXIC_INCREASE_LEVEL;
        #endregion

        #endregion

        #region Constants
        public const uint TOXIC_DROP_RATE = 4;
        public const uint TOXIC_INCREASE_LEVEL = 15;
        #endregion

        #endregion

        #region Private

        #region Members
        private float _bleed = 0f;
        private float _toxic = 0f;
        #endregion

        #endregion
    }
}