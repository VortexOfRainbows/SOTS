using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class Traingun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trains");
			Tooltip.SetDefault("I like Trains. Dev Item");
		}
		public override void SetDefaults()
		{
            item.damage = 100000;  //gun damage
			item.magic = true;
            item.width = 200;     //gun image width
            item.height = 200;   //gun image  height
            item.useTime = 18;  //how fast 
            item.useAnimation = 18;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 1000000;
            item.rare = 11;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Trains"); 
            item.shootSpeed = 10;
			item.expert = true;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 14;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
