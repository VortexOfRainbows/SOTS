using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class StormShock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sapphire Thunderclap");
			Tooltip.SetDefault("Generates thunder with the power of the sapphire");
		}
		public override void SetDefaults()
		{

			item.damage = 13;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = 237; 
            item.shootSpeed = 12;


		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"StoneSwinger", 1);
			recipe.AddIngredient(ItemID.Sapphire, 3);
			recipe.AddIngredient(ItemID.SilverBroadsword, 1);
			recipe.AddIngredient(ItemID.Cloud, 30);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}