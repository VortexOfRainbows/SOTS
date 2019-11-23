using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Celestial
{
	public class DanceOfDeath : ModItem
	{	int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dance of Death");
			Tooltip.SetDefault("Cast many demon scythes around you");
		}
		public override void SetDefaults()
		{
            item.damage = 60; 
            item.magic = true; 
            item.width = 28;   
            item.height = 30;   
            item.useTime = 34;   
            item.useAnimation = 34;
            item.useStyle = 5;    
            item.noMelee = true;  
            item.knockBack = 6.5f;
            item.value = Item.sellPrice(0, 9, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("DeathBlade"); 
            item.shootSpeed = 9.5f;
			item.mana = 14;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddIngredient(null, "TomeOfTheReaper", 1);
			recipe.AddIngredient(null, "ShiftingSands", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
         {
				for(int i = 0; i < 4; i++)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(90 * i)); 
					Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
				return false; 
		}
	}
}
