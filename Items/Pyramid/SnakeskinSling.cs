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
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class SnakeskinSling : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snakeskin Sling");
			Tooltip.SetDefault("Increases ranged damage by 5%\nEvery 5th ranged attack launches an additional rock projectile");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 30;     
            item.height = 28;   
            item.value = Item.sellPrice(0, 0, 65, 0);
            item.rare = 4;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.rangedDamage += 0.05f;
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.snakeSling = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Snakeskin", 18);
			recipe.AddIngredient(ItemID.Leather, 3);
			recipe.AddIngredient(ItemID.Wood, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}