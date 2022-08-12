using SOTS.Items.Celestial;
using SOTS.Items.Fragments;
using SOTS.Items.AbandonedVillage;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.CritBonus
{
	public class OtherworldlyAmplifier : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Otherworldly Amplifier");
			Tooltip.SetDefault("Critical strikes deal 12 more damage");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 26;     
            Item.height = 42;  
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CritBonusDamage += 6;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Amber, 5).AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 4).AddTile(TileID.Anvils).Register();
		}
	}
	public class BloodstainedCoin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodstained Coin");
			Tooltip.SetDefault("Critical strikes have a 50% chance to deal 30 more damage\nReceiving damage has a 50% chance to bleed you");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (Main.rand.NextBool(2))
			{
				modPlayer.CritBonusDamage += 15;
				if (modPlayer.onhit == 1)
				{
					player.AddBuff(BuffID.Bleeding, 1020, false); //17 seconds
				}
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Vertebrae, 5).AddIngredient(ModContent.ItemType<AncientSteelBar>(), 8).AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 4).AddTile(TileID.Anvils).Register();
		}
	}
	public class PutridCoin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Coin");
			Tooltip.SetDefault("Critical strikes have a 50% chance to deal 30 more damage\nReceiving damage has a 50% chance to poison you");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (Main.rand.NextBool(2))
			{
				modPlayer.CritBonusDamage += 15;
				if (modPlayer.onhit == 1)
				{
					player.AddBuff(BuffID.Poisoned, 300, false); //5 seconds
				}
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.RottenChunk, 5).AddIngredient(ModContent.ItemType<AncientSteelBar>(), 8).AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 4).AddTile(TileID.Anvils).Register();
		}
	}
	public class PolishedCoin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polished Coin");
			Tooltip.SetDefault("Critical strikes have a 50% chance to deal 40 more damage\n3% increased crit chance\nImmunity to bleeding and poisoned");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
			Item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.GetCritChance(DamageClass.Generic) += 3;
			if (Main.rand.NextBool(2))
			{
				modPlayer.CritBonusDamage += 20;
			}
			player.buffImmune[BuffID.Bleeding] = true;
			player.buffImmune[BuffID.Poisoned] = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PutridCoin>(), 1).AddIngredient(ItemID.MedicatedBandage, 1).AddTile(TileID.TinkerersWorkbench).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<BloodstainedCoin>(), 1).AddIngredient(ItemID.MedicatedBandage, 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
	public class FocusCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Crystal");
			Tooltip.SetDefault("Critical strikes deal 50 more damage\n5% increased crit chance\nImmunity to bleeding and poisoned debuffs");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
			Item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.GetCritChance(DamageClass.Generic) += 5;
			modPlayer.CritBonusDamage += 25;
			player.buffImmune[BuffID.Bleeding] = true;
			player.buffImmune[BuffID.Poisoned] = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PolishedCoin>(), 1).AddIngredient(ModContent.ItemType<OtherworldlyAmplifier>(), 1).AddIngredient(ModContent.ItemType<DissolvingNether>(), 1).AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 5).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
