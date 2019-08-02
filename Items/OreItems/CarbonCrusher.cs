using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class CarbonCrusher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Carbon Crusher");
			Tooltip.SetDefault("Crushes even the smallest of compounds using the strengh and unbreakability of the diamond");
		}
		public override void SetDefaults()
		{

			item.damage = 46;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 4;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("CarbonCrush"); 
            item.shootSpeed = 5;
			item.noUseGraphic = true;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"StoneSwinger", 1);
			recipe.AddIngredient(ItemID.Diamond, 3);
			recipe.AddIngredient(ItemID.PlatinumBroadsword, 1);
			recipe.AddIngredient(null,"GoblinRockBar", 8);
			recipe.AddIngredient(null,"BrassBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}