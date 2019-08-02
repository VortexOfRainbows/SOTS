using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Enchants
{
	public class DNA : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic VIII : DNA");
			Tooltip.SetDefault("Strands of creation");
		}
		public override void SetDefaults()
		{

			item.damage = 220;
			item.melee = true;
			item.width = 66;
			item.height = 66;
			item.useTime = 48;
			item.useAnimation = 12;
			item.useStyle = 1;
			item.knockBack = 3;
			item.value = 1000000000;
			item.rare = 10;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("DNA2Leader"); 
            item.shootSpeed = 5;
			item.expert = true;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"HallowedSickle", 1);
			recipe.AddIngredient(null,"RimBow", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"CoreOfStatus", 3);
			recipe.AddIngredient(null,"CoreOfCreation", 4);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(315)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
					Vector2 perturbedSpeed1 = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(45)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed1.X, perturbedSpeed1.Y, mod.ProjectileType("DNA3Leader"), damage, knockBack, player.whoAmI);
              }
              return false; 
			}
		}

	}