using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class CursedImpale : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Impale");
			Tooltip.SetDefault("Fires shadowflame upon hitting an enemy");
		}
		public override void SetDefaults()
		{

			item.damage = 26;
			item.melee = true;
			item.width = 44;
			item.height = 44;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 6;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = mod.ProjectileType("CurseSpear"); 
            item.shootSpeed = 5;
			item.noUseGraphic = true;
			item.noMelee = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CursedMatter", 7);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
              int numberProjectiles = 1;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return true; 
		}
		
	}
}
	
