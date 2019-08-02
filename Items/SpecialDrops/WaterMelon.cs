using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;


namespace SOTS.Items.SpecialDrops
{
	public class WaterMelon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Watermelon Gun");
			Tooltip.SetDefault("Slimes eat lots of watermelons");
		}
		public override void SetDefaults()
		{
            item.damage = 1;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 16;     //gun image width
            item.height = 42;   //gun image  height
            item.useTime = 0;  //how fast 
            item.useAnimation = 3;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 0;
            item.rare = 4;
            item.UseSound = SoundID.Item13;
            item.autoReuse = true;
            item.shoot = 51; 
            item.shootSpeed = 24;
			

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 3;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}

}
