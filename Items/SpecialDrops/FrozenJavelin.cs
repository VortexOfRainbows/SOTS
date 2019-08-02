using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class FrozenJavelin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frozen Javelin");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			
			item.CloneDefaults(3094);
			item.damage = 22;
			item.thrown = true;
			item.width = 34;
			item.height = 34;
			item.useStyle = 1;
			item.knockBack = 2;
			item.value = 250;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("FrozenJavelin"); 
            item.shootSpeed = 14;
			item.consumable = true;;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(1)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
		}
		public override void CaughtFishStack(ref int stack)
		{
			stack = Main.rand.Next(25,76);
		}
	}
}