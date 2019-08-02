using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class ManicMiner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manic Miner");
			Tooltip.SetDefault("Converts mana into rays of mining");
		}
		public override void SetDefaults()
		{

			item.width = 36;
			item.height = 28;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 5;
			item.UseSound = SoundID.Item12;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("ManaMiner"); 
            item.shootSpeed = 8;
			item.mana = 20;
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(2)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);

              }
              return false; 
			  
	}
	}
}