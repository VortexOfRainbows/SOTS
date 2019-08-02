using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
namespace SOTS.Items.SoldStuff
{
	public class IchorSprayer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Sprayer");
			Tooltip.SetDefault("How the 'underworld' did you fill this thing up? Like seriously WTF?");
		}
		public override void SetDefaults()
		{
            item.damage = 20;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 44;     //gun image width
            item.height = 30;   //gun image  height
            item.useTime = 26;  //how fast 
            item.useAnimation = 26;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 2f;
            item.value = 125000;
            item.rare = 2;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = 280; 
            item.shootSpeed = 16;
			item.reuseDelay = 8;

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
