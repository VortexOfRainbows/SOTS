using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class FrightEruption : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eruption");
			Tooltip.SetDefault("Fires out a barrage of flaming projectiles on your cursor");
		}
		public override void SetDefaults()
		{
            item.damage = 100;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 60;     //gun image width
            item.height = 60;   //gun image  height
            item.useTime = 70;  //how fast 
            item.useAnimation = 70;
            item.useStyle = 1;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 125000;
            item.rare = 8;
            item.UseSound = SoundID.Item13;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("DracoBeacon"); 
            item.shootSpeed = 12;
			item.mana = 100;

		}
		
          public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
			  Vector2 vector14;
					
						if (player.gravDir == 1f)
					{
					vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						vector14.X = (float)Main.mouseX + Main.screenPosition.X;
						
                Projectile.NewProjectile(vector14.X,  vector14.Y, 5, 0, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, -5, 0, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 5, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, -5, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 5, 5, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 5, -5, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, -5, 5, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, -5, -5, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 0, 296, damage, 0, Main.myPlayer, 0.0f, 0);
					return false;
					
          }  
	}
}
