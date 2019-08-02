using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class MightyAquarius : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aquarius");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 43;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 26;     //gun image width
            item.height = 38;   //gun image  height
            item.useTime = 25;  //how fast 
            item.useAnimation = 25;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 125000;
            item.rare = 5;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("MightArrowFriendly"); 
            item.shootSpeed = 16;
		
			item.reuseDelay = 24;
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
