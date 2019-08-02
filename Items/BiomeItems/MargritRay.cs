using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class MargritRay : ModItem
	{ 	float speedUp = 0;
		bool overheated = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Ray");
			Tooltip.SetDefault("Doesn't consume mana, but can overheat if overused");
		}
		public override void SetDefaults()
		{
            item.damage = 17;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 48;     //gun image width
            item.height = 22;   //gun image  height
            item.useTime = 50;  //how fast 
            item.useAnimation = 50;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0.1f;
            item.value = 110000;
            item.rare = 6;
            item.UseSound = SoundID.Item9;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Copulse2"); 
            item.shootSpeed = 7;
			item.mana = 0;
		

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MargritCore", 1);
			recipe.AddIngredient(null, "ObsidianScale", 20);
			recipe.AddIngredient(3081, 12);
			recipe.AddIngredient(3086, 28);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
				speedUp += (6 / (speedUp + 3));
				
				speedUp += 0.05f;
				
				if(speedUp < 36)
				{
				speedUp += 0.25f;
					
				}
				if(speedUp < 30)
				{
				speedUp += 0.75f;
					
				}
				if(speedUp < 10)
				{
				speedUp += 0.75f;
					
				}
				if(speedUp < 20)
				{
				speedUp += 0.75f;
					
				}
              int numberProjectiles = 1;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians((int)(speedUp / 5))); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
		}
		public override void UpdateInventory(Player player)
		{
			if(overheated == false)
			{
				item.useAnimation = (int)(50 - speedUp);
				item.useTime = (int)(50 - speedUp);
				item.shootSpeed = 7 + (speedUp * 0.1f);
			}
				speedUp -= 0.005f;
				if(speedUp < 0)
				{
					speedUp = 0;
					overheated = false;
				}
				if(speedUp > 48)
				{
					overheated = true;
					
				}
				if(overheated == true)
				{
					for(int i = 0; i < 2; i++)
					{
					Dust.NewDust(new Vector2(player.Center.X - 1, player.Center.Y - 1), 1, 1, 6);
					}
					speedUp -= 0.025f;
				}
				if(speedUp < 48 && speedUp > 10)
				{
					for(int i = 0; i < 3; i++)
					{
						if(Main.rand.Next(0, (int)(52 - speedUp)) == 1)
						{
							Dust.NewDust(new Vector2(player.Center.X - 1, player.Center.Y - 1), 1, 1, 6);
						}
					}
				}
				
				
		}
	}
}
