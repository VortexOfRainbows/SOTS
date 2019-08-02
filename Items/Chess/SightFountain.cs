using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class SightFountain : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Fountain");
			Tooltip.SetDefault("Rains down nature\nX");
		}
		public override void SetDefaults()
		{
            item.damage = 45;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 36;     //gun image width
            item.height = 34;   //gun image  height
            item.useTime = 1;  //how fast 
            item.useAnimation = 9;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 125000;
            item.rare = 9;
            item.UseSound = SoundID.Item13;
            item.autoReuse = true;
            item.shoot = 227; 
            item.shootSpeed = 12;
			item.mana = 2;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun

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
						
				
				
				  Projectile.NewProjectile(vector14.X, vector14.Y, Main.rand.Next(-1,2), -(Main.rand.Next(-5, 0)), 227, damage, 0, 0);
				  Projectile.NewProjectile(vector14.X, vector14.Y, Main.rand.Next(-1,2), -(Main.rand.Next(-5, 0)), 228, damage, 0, 0);
				  Projectile.NewProjectile(vector14.X, vector14.Y, Main.rand.Next(-1,2), -(Main.rand.Next(-5, 0)), 229, damage, 0, 0);
				  Projectile.NewProjectile(vector14.X, vector14.Y, 0, 0, mod.ProjectileType("X"), damage, 0, 0);
			  
					return false;
					
          }  
	}
}
