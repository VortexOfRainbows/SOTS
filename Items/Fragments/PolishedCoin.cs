using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;


namespace SOTS.Items.Fragments
{
	public class PolishedCoin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polished Coin");
			Tooltip.SetDefault("Critical strikes have a 50% chance to deal 40 more damage\n3% increased crit chance\nImmunity to bleeding and poisoned debuffs");
		}
		public override void SetDefaults()
		{
            item.width = 24;     
            item.height = 26;  
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
			item.accessory = true;
			item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
			player.meleeCrit += 3;
			player.rangedCrit += 3;
			player.magicCrit += 3;
			player.thrownCrit += 3;
			
			if(Main.rand.Next(2) == 0)
			{
				modPlayer.CritBonusDamage += 20;
			}
            player.buffImmune[BuffID.Bleeding] = true; 
            player.buffImmune[BuffID.Poisoned] = true; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PutridCoin", 1);
			recipe.AddIngredient(ItemID.AdhesiveBandage, 1);
			recipe.AddIngredient(ItemID.Bezoar, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BloodstainedCoin", 1);
			recipe.AddIngredient(ItemID.AdhesiveBandage, 1);
			recipe.AddIngredient(ItemID.Bezoar, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PutridCoin", 1);
			recipe.AddIngredient(ItemID.MedicatedBandage, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BloodstainedCoin", 1);
			recipe.AddIngredient(ItemID.MedicatedBandage, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
