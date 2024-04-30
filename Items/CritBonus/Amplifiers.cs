using SOTS.Items.Celestial;
using SOTS.Items.Fragments;
using SOTS.Items.AbandonedVillage;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Permafrost;

namespace SOTS.Items.CritBonus
{
	public class OtherworldlyAmplifier : ModItem
	{
		public override void SetStaticDefaults()
		{
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
			modPlayer.CritBonusDamage += 8;
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
				modPlayer.CritBonusDamage += 20;
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
				modPlayer.CritBonusDamage += 20;
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
			player.GetCritChance(DamageClass.Generic) += 2;
			if (Main.rand.NextBool(2))
			{
				modPlayer.CritBonusDamage += 30;
			}
			player.buffImmune[BuffID.Bleeding] = true;
			player.buffImmune[BuffID.Poisoned] = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<PutridCoin>().AddIngredient(ItemID.MedicatedBandage).AddIngredient<DissolvingUmbra>().AddTile(TileID.TinkerersWorkbench).Register();
			CreateRecipe(1).AddIngredient<BloodstainedCoin>().AddIngredient(ItemID.MedicatedBandage).AddIngredient<DissolvingUmbra>().AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
	public class FocusCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
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
			player.GetCritChance(DamageClass.Generic) += 4;
			modPlayer.CritBonusDamage += 40;
			player.buffImmune[BuffID.Bleeding] = true;
			player.buffImmune[BuffID.Poisoned] = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PolishedCoin>(), 1).AddIngredient(ModContent.ItemType<OtherworldlyAmplifier>(), 1).AddIngredient(ModContent.ItemType<DissolvingNether>(), 1).AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 5).AddIngredient<SoulOfPlight>(5).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
