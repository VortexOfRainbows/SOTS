using SOTS.Items.Celestial;
using SOTS.Items.Fragments;
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
		}
		public override void SetDefaults()
		{
            item.width = 26;     
            item.height = 42;  
            item.value = Item.sellPrice(0, 0, 25, 0);
            item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CritBonusDamage += 6;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Amber, 5);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class BloodstainedCoin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodstained Coin");
			Tooltip.SetDefault("Critical strikes have a 50% chance to deal 30 more damage\nReceiving damage has a 50% chance to bleed you");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 35, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Vertebrae, 5);
			recipe.AddIngredient(ModContent.ItemType<Goblinsteel>(), 8);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class PutridCoin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Coin");
			Tooltip.SetDefault("Critical strikes have a 50% chance to deal 30 more damage\nReceiving damage has a 50% chance to poison you");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 35, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RottenChunk, 5);
			recipe.AddIngredient(ModContent.ItemType<Goblinsteel>(), 8);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class PolishedCoin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polished Coin");
			Tooltip.SetDefault("Critical strikes have a 50% chance to deal 40 more damage\n3% increased crit chance\nImmunity to bleeding and poisoned");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
			item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.meleeCrit += 3;
			player.rangedCrit += 3;
			player.magicCrit += 3;
			player.thrownCrit += 3;
			if (Main.rand.NextBool(2))
			{
				modPlayer.CritBonusDamage += 20;
			}
			player.buffImmune[BuffID.Bleeding] = true;
			player.buffImmune[BuffID.Poisoned] = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PutridCoin>(), 1);
			recipe.AddIngredient(ItemID.MedicatedBandage, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BloodstainedCoin>(), 1);
			recipe.AddIngredient(ItemID.MedicatedBandage, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class FocusCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Crystal");
			Tooltip.SetDefault("Critical strikes deal 50 more damage\n5% increased crit chance\nImmunity to bleeding and poisoned debuffs");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 22;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.accessory = true;
			item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.meleeCrit += 5;
			player.rangedCrit += 5;
			player.magicCrit += 5;
			player.thrownCrit += 5;
			modPlayer.CritBonusDamage += 25;
			player.buffImmune[BuffID.Bleeding] = true;
			player.buffImmune[BuffID.Poisoned] = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PolishedCoin>(), 1);
			recipe.AddIngredient(ModContent.ItemType<OtherworldlyAmplifier>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DissolvingNether>(), 1);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
