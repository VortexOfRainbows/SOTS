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
	public class VoidenBracelet : ModItem
	{	int timer = 1;
		float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voiden Bracelet");
			Tooltip.SetDefault("Increases void damage and magic damage by 8%\nDecreases void cost by 10%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 28;     
            item.height = 22;   
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = 5;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidDamage += 0.08f;
			voidPlayer.voidCost -= 0.10f;
			player.magicDamage += 0.08f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CursedMatter", 4);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}