using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class Vulcan : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic XIX : Vulcan");
			Tooltip.SetDefault("L");
		}
		public override void SetDefaults()
		{
            item.damage = 100;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 36;     //gun image width
            item.height = 50;   //gun image  height
            item.useTime = 360;  //how fast 
            item.useAnimation = 360;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 1000000000;
            item.rare = 10;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("VulcanOrb"); 
            item.shootSpeed = 6;
			item.expert = true;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "LMaterial", 1);
			recipe.AddIngredient(null, "Geo", 1);
			recipe.AddIngredient(null, "SpectreManipulator", 1);
			recipe.AddIngredient(null, "ReanimationMaterial", 13);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null, "TheHardCore", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
			{
              return true; 
			}
	}
}
