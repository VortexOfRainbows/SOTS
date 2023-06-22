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
	public class Awakened : ModPrefix
	{
		public override float RollChance(Item item)
			=> 0.75f;
		public override bool CanRoll(Item item)
			=> true;
		public override PrefixCategory Category => PrefixCategory.Accessory;
		public override void Apply(Item item) => item.GetGlobalItem<PrefixItem>().extraVoid = 10;
        public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1.4f;
			valueMult *= multiplier;
		}
	}
	public class Omniscient : ModPrefix
	{
		public override float RollChance(Item item)
			=> 0.5f;
		public override bool CanRoll(Item item)
			=> true;
		public override PrefixCategory Category => PrefixCategory.Accessory;
		public override void Apply(Item item) => item.GetGlobalItem<PrefixItem>().extraVoid = 20;
		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1.8f;
			valueMult *= multiplier;
		}
	}
	public class Chained : ModPrefix
	{
		public override float RollChance(Item item)
			=> 0.5f;
		public override bool CanRoll(Item item)
			=> true;
		public override PrefixCategory Category => PrefixCategory.Accessory;
		public override void Apply(Item item) => item.GetGlobalItem<PrefixItem>().extraVoidGain = 1;
		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1.4f;
			valueMult *= multiplier;
		}
	}
	public class Soulbound : ModPrefix
	{
		public override float RollChance(Item item)
			=> 0.25f;
		public override bool CanRoll(Item item)
			=> true;
		public override PrefixCategory Category => PrefixCategory.Accessory;
		public override void Apply(Item item) => item.GetGlobalItem<PrefixItem>().extraVoidGain = 2;
		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1.8f;
			valueMult *= multiplier;
		}
	}
	public class Famished : ModPrefix
	{
		public override float RollChance(Item item)
			=> 1f;
		public override bool CanRoll(Item item)
        {
			return item.ModItem as VoidItem != null;
        }
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;
		public override void Apply(Item item)
        {
			item.GetGlobalItem<PrefixItem>().voidCostMultiplier = 1.25f;
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
			return item.ModItem as VoidItem != null;
		}
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;
		public override void Apply(Item item)
		{
			item.GetGlobalItem<PrefixItem>().voidCostMultiplier = 1.2f;
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
			return item.ModItem as VoidItem != null;
		}
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;
		public override void Apply(Item item)
		{
			item.GetGlobalItem<PrefixItem>().voidCostMultiplier = 0.9f;
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
			=> 0.7f;
		public override bool CanRoll(Item item)
		{
			return item.ModItem as VoidItem != null;
		}
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;
		public override void Apply(Item item)
		{
			item.GetGlobalItem<PrefixItem>().voidCostMultiplier = 0.9f;
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
	public class Chthonic : ModPrefix
	{
		public override float RollChance(Item item)
			=> 0.8f;
		public override bool CanRoll(Item item)
		{
			return item.ModItem as VoidItem != null;
		}
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;
		public override void Apply(Item item)
		{
			item.GetGlobalItem<PrefixItem>().voidCostMultiplier = 1.1f;
		}
		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			damageMult += 0.24f;
			useTimeMult += 0.06f; //this is bad
			knockbackMult -= 0.06f;
			base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
		}
		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1.06f;
			valueMult *= multiplier;
		}
	}
}