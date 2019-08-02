using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class MistBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mist Ball");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.damage = 42;
			item.magic = true;
			item.width = 18;
			item.height = 32;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 10;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("VampireShotM"); 
            item.shootSpeed = 13;
			item.mana = 8;
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
              int numberProjectiles = 2;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(18)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);

              }
              return false; 
			  
	}
	}
}