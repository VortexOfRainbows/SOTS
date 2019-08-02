using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
namespace SOTS.Items.SoldStuff
{
	public class MidasMaker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Masterpeice");
		}
		public override void SetDefaults()
		{
            item.damage = 16;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 42;     //gun image width
            item.height = 28;   //gun image  height
            item.useTime = 14;  //how fast 
            item.useAnimation = 14;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 250000;
            item.rare = 2;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = 287; 
            item.shootSpeed = 21;
			item.reuseDelay = 8;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(2)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}

	}
}
