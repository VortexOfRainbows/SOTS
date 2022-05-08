using Microsoft.Xna.Framework;
using SOTS.Items;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace SOTS.Void
{
	public class VoidPrefix : ModPrefix
	{
		private readonly byte _power;
		public override float RollChance(Item item)
			=> 6f / _power;
		public override bool CanRoll(Item item)
			=> true;
		public override PrefixCategory Category => PrefixCategory.Accessory;
		public VoidPrefix()	{ }
		public VoidPrefix(byte power)
		{
			_power = power;
		}
        public override bool Autoload(ref string name)
		{
			if (!base.Autoload(ref name))
			{
				return false;
			}
			mod.AddPrefix("Awakened", new VoidPrefix(10));
			mod.AddPrefix("Omniscient", new VoidPrefix(20));
			return false;
		}
		public override void Apply(Item item) => Item.GetGlobalItem<PrefixItem>().extraVoid = _power;
        public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1.0f + _power / 25f;
			valueMult *= multiplier;
		}
	}
	public class VoidGainPrefix : ModPrefix
	{
		private readonly byte _power;
		public override float RollChance(Item item)
			=> 0.4f / _power;
		public override bool CanRoll(Item item)
			=> true;
		public override PrefixCategory Category => PrefixCategory.Accessory;
		public VoidGainPrefix() { }
		public VoidGainPrefix(byte power)
		{
			_power = power;
		}
		public override bool Autoload(ref string name)
		{
			if (!base.Autoload(ref name))
			{
				return false;
			}
			mod.AddPrefix("Chained", new VoidGainPrefix(1));
			mod.AddPrefix("Soulbound", new VoidGainPrefix(2));
			return false;
		}
		public override void Apply(Item item) => Item.GetGlobalItem<PrefixItem>().extraVoidGain = _power;
		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1.0f + 0.4f * _power;
			valueMult *= multiplier;
		}
	}
	public class Famished : ModPrefix
	{
		public override float RollChance(Item item)
			=> 1f;
		public override bool CanRoll(Item item)
        {
			return Item.modItem as VoidItem != null;
        }
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;
		public Famished() { }
		public override bool Autoload(ref string name)
		{
			if (!base.Autoload(ref name))
			{
				return false;
			}
			mod.AddPrefix("Famished", new Famished());
			return false;
		}
		public override void Apply(Item item)
        {
			Item.GetGlobalItem<PrefixItem>().voidCostMultiplier = 1.25f;
		}
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
			damageMult -= 0.10f;
			knockbackMult -= 0.10f;
            base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
        }
        public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 0.9f;
			valueMult *= multiplier;
		}
	}
	public class Precarious : ModPrefix
	{
		public override float RollChance(Item item)
			=> 1f;
		public override bool CanRoll(Item item)
		{
			return Item.modItem as VoidItem != null;
		}
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;
		public Precarious() { }
		public override bool Autoload(ref string name)
		{
			if (!base.Autoload(ref name))
			{
				return false;
			}
			mod.AddPrefix("Precarious", new Precarious());
			return false;
		}
		public override void Apply(Item item)
		{
			Item.GetGlobalItem<PrefixItem>().voidCostMultiplier = 1.2f;
		}
		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			damageMult += 0.10f;
			useTimeMult -= 0.05f;
			knockbackMult += 0.10f;
			base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
		}
		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1.03f;
			valueMult *= multiplier;
		}
	}
	public class Potent : ModPrefix
	{
		public override float RollChance(Item item)
			=> 0.75f;
		public override bool CanRoll(Item item)
		{
			return Item.modItem as VoidItem != null;
		}
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;
		public Potent() { }
		public override bool Autoload(ref string name)
		{
			if (!base.Autoload(ref name))
			{
				return false;
			}
			mod.AddPrefix("Potent", new Potent());
			return false;
		}
		public override void Apply(Item item)
		{
			Item.GetGlobalItem<PrefixItem>().voidCostMultiplier = 0.9f;
		}
		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			damageMult += 0.05f;
			useTimeMult -= 0.05f;
			base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
		}
		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1.07f;
			valueMult *= multiplier;
		}
	}
	public class Omnipotent : ModPrefix
	{
		public override float RollChance(Item item)
			=> 0.5f;
		public override bool CanRoll(Item item)
		{
			return Item.modItem as VoidItem != null;
		}
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;
		public Omnipotent() { }
		public override bool Autoload(ref string name)
		{
			if (!base.Autoload(ref name))
			{
				return false;
			}
			mod.AddPrefix("Omnipotent", new Omnipotent());
			return false;
		}
		public override void Apply(Item item)
		{
			Item.GetGlobalItem<PrefixItem>().voidCostMultiplier = 0.9f;
		}
		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			damageMult += 0.15f;
			useTimeMult -= 0.10f;
			knockbackMult += 0.15f;
			critBonus += 5;
			base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
		}
		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1.16f;
			valueMult *= multiplier;
		}
	}
}