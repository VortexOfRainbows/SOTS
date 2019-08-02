using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
namespace SOTS.Items.SoldStuff
{
	public class BiggHands : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bigg Hands");
			Tooltip.SetDefault("Fires whatever bullet you have\nStays loyal");
		}
		public override void SetDefaults()
		{
            item.damage = 29;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 52;     //gun image width
            item.height = 22;   //gun image  height
            item.useTime = 19;  //how fast 
            item.useAnimation = 19;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 3;
            item.value = 125000;
            item.rare = 6;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = 10; 
            item.shootSpeed = 21;
			item.useAmmo = AmmoID.Bullet;
			item.reuseDelay = 8;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
		public override bool ConsumeAmmo(Player p)
		{
			return false;
}

	}
}
