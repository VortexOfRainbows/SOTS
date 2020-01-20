using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class WormWoodHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Hook");
			Tooltip.SetDefault("Hooks onto organic matter, dealing damage and connecting you to them\nCan be used again before the hook returns");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.AmethystHook);
			item.damage = 17;
			item.knockBack = 0;
            item.width = 32;  
            item.height = 32;   
            item.value = Item.sellPrice(0, 1, 80, 0);
            item.rare = 4;
			item.shoot = mod.ProjectileType("PinkyHook"); 
            item.shootSpeed = 13.33f;

			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WormWoodCore", 1);
			recipe.AddIngredient(null, "Wormwood", 24);
			recipe.AddIngredient(ItemID.PinkGel, 16);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}
