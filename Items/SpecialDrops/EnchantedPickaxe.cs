using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
namespace SOTS.Items.SpecialDrops
{
	public class EnchantedPickaxe : ModItem
	{	int fireTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Pickaxe");
			Tooltip.SetDefault("Shoots an echanted pickaxe beam that breaks blocks");
		}
		public override void SetDefaults()
		{
            item.damage = 12;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 34;     //gun image width
            item.height = 34;   
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("EnchantedPickaxeProj");
			item.shootSpeed = 7f;
			item.useTurn = true;
            item.useTime = 8;
            item.useAnimation = 16;
			item.pick = 75;
			item.axe = 15;
			item.useStyle = 1;
			item.knockBack = 5;
            item.value = 100000;
            item.rare = 4;
			item.UseSound = SoundID.Item1;
			item.tileBoost++;
			item.autoReuse = true;
			item.channel = true;
			
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 0;
			  fireTimer++;
			  if(fireTimer >= 6)
			  {
				  fireTimer = 0;
				  numberProjectiles = 1;
			  }
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
