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
	public class SoulCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Charm");
			Tooltip.SetDefault("Critical strikes steal life and regenerate void\n3% increased crit chance");
		}
		public override void SetDefaults()
		{
            item.width = 30;     
            item.height = 34;  
            item.value = Item.sellPrice(0, 4, 50, 0);
            item.rare = 8;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
			player.meleeCrit += 3;
			player.rangedCrit += 3;
			player.magicCrit += 3;
			player.thrownCrit += 3;
			
			modPlayer.CritLifesteal += 2 + Main.rand.Next(2);
			modPlayer.CritVoidsteal += 2.25f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CloverCharm", 1);
			recipe.AddIngredient(null, "VoidCharm", 1);
			recipe.AddIngredient(ItemID.Ectoplasm, 10); 
			//recipe.AddIngredient(ItemID.Ectoplasm, 1); //To be replaced later (dissolving tide)
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
