using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.PLunar
{
	public class StarRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Rod");
			Tooltip.SetDefault("The power of dying stars gives you the power to create");
		}
		public override void SetDefaults()
		{
            item.damage = 226;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 52;     //gun image width
            item.height =52  ;   //gun image  height
            item.useTime = 30;  //how fast 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 5;
            item.UseSound = SoundID.Item28;
            item.autoReuse = true;
            item.shoot = 12; 
            item.shootSpeed = 12;
			item.mana = 4;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 32;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360)); // This defines the projectiles random spread . 30 degree spread.
				  

                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);


              }
              return false; 
	}
	}
}
