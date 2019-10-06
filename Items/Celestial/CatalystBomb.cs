using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Celestial
{
	public class CatalystBomb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Catalyst Bomb");
			Tooltip.SetDefault("'Shatter the environment around you'");
		}
		public override void SetDefaults()
		{
			item.damage = 100;
			item.width = 22;
			item.height = 28;
			item.useStyle = 1;
			item.value = 0;
			item.rare = 8;
			item.useTime = 30;
			item.useAnimation = 30;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = mod.ProjectileType("CatalystBomb"); 
            item.shootSpeed = 12f;
			item.consumable = true;
			item.maxStack = 30;
			item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bomb, 5);
			recipe.AddIngredient(ItemID.Ectoplasm, 3);
			recipe.AddIngredient(null,"SoulResidue", 3);
			recipe.AddIngredient(ItemID.SoulofNight, 3);
			recipe.AddIngredient(ItemID.SoulofSight, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}