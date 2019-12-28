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
	public class CursedIcosahedron : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Icosahedron");
			Tooltip.SetDefault("Critical strikes may afflict enemies with frostburn, burn, or cursed flames");
		}
		public override void SetDefaults()
		{
            item.width = 26;     
            item.height = 26;  
            item.value = Item.sellPrice(0, 5, 50, 0);
            item.rare = 8;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.CritFrost = true;
			modPlayer.CritFire = true;
			modPlayer.CritCurseFire = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BorealisIcosahedron", 1);
			recipe.AddIngredient(null, "HellfireIcosahedron", 1);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 10); //To be replaced later (dissolving evil)
			recipe.AddIngredient(ItemID.CursedFlame, 10); 
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
