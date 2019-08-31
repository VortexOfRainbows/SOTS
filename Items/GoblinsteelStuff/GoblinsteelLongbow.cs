using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.GoblinsteelStuff
{
	public class GoblinsteelLongbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblinsteel Bow");
			Tooltip.SetDefault("Fires out returning blades");
		}
		public override void SetDefaults()
		{

			item.damage = 16;
			item.ranged = true;
			item.width = 20;
			item.height = 60;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 5;
			item.knockBack = 2.5f;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = 4;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("GoblinBlade"); 
            item.shootSpeed = 20;
			item.useAmmo = AmmoID.Arrow;
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("GoblinBlade"), damage, knockBack, player.whoAmI);

              }
              return false; 
			  
	}
	}
}