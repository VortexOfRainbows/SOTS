using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class GelAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gel Throwing Axe");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Shuriken);
			item.damage = 11;
			item.thrown = true;
			item.width = 30;
			item.height = 30;
			item.useStyle = 1;
			item.knockBack = 1.75f;
			item.value = 100;
			item.rare = 1;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = mod.ProjectileType("GelAxe"); 
            item.shootSpeed *= 1.2f;
			item.consumable = true;
			item.maxStack = 999;
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