using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class SightSnap : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harmony Staff");
			Tooltip.SetDefault("Rapidly fires out barrages of leafy projectiles from your cursor");
		}
		public override void SetDefaults()
		{
            item.damage = 32;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 30;     //gun image width
            item.height = 30;   //gun image  height
            item.useTime = 9;  //how fast 
            item.useAnimation = 9;
            item.useStyle = 1;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 125000;
            item.rare = 8;
            item.UseSound = SoundID.Item13;
            item.autoReuse = true;
            item.shoot = 227; 
            item.shootSpeed = 12;
			item.mana = 5;

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
                Projectile.NewProjectile(vector14.X,  vector14.Y, 4, 4, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 4, -4, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, -4, 4, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, -4, -4, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 4, 0, 206, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, -4, 0, 206, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 4, 206, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, -4, 206, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 3, 3, 206, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 3, -3, 206, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, -3, 3, 206, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, -3, -3, 206, damage, 0, Main.myPlayer, 0.0f, 0);
					return false;
					
          }  
	}
}
