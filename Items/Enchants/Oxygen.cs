using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Enchants
{
	public class Oxygen : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic IX : Oxygen");
			Tooltip.SetDefault("O");
		}
		public override void SetDefaults()
		{

			item.damage = 100;
			item.melee = true;
			item.width = 48;
			item.height = 48;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 1;
			item.knockBack = 3;
			item.value = 1000000000;
			item.rare = 10;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("Oxygen"); 
            item.shootSpeed = 16;
			item.expert = true;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"OMaterial", 1);
			recipe.AddIngredient(null,"StarMaterial", 1);
			recipe.AddIngredient(null,"Bloodrayne", 1);
			recipe.AddIngredient(null,"MistBow", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"CoreOfStatus", 5);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}

	}