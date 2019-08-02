using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class CrusherEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crusher 5000");
			Tooltip.SetDefault("Crushes Materials");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(3, 5));
		}
		public override void SetDefaults()
		{
      
            item.width = 24;     
            item.height = 30;   
            item.value = 1000;
            item.rare = 1;
			item.maxStack = 99;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Actuator, 10);
			recipe.AddIngredient(null, "SteelBar", 4);
			recipe.AddIngredient(ItemID.Wire, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
