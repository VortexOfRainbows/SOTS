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
	public class SpiritGlove : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Glove");
			Tooltip.SetDefault("Increases void regen by 0.5 and melee crit by 6%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 26;     
            item.height = 30;   
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 6;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.05f;
			player.meleeCrit += 6;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SoulResidue", 32);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}