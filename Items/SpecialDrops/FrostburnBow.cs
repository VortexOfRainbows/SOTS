using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class FrostburnBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Burner");
			Tooltip.SetDefault("Turns all arrows into frostburn arrows");
		}
		public override void SetDefaults()
		{

			item.damage = 12;
			item.ranged = true;
			item.width = 26;
			item.height = 42;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 5;
			item.knockBack = 2.5f;
			item.value = 5000;
			item.rare = 1;
			item.UseSound = SoundID.Item5;
			item.autoReuse = false;            
			item.shoot = 172; 
            item.shootSpeed = 11;
			item.useAmmo = AmmoID.Arrow;
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(2)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 172, damage, knockBack, player.whoAmI);

              }
              return false; 
			  
	}
	}
}