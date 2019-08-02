using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.PLunar
{
	public class StormStaff  : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Staff");
			Tooltip.SetDefault("The power of weather is in your hands");
		}
		public override void SetDefaults()
		{
            item.damage = 126;  //gun damage
            item.summon = true;   //its a gun so set this to true
            item.width =64;     //gun image width
            item.height = 65;   //gun image  height
            item.useTime = 34;  //how fast 
            item.useAnimation = 34;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 30000;
            item.rare = 9;
            item.UseSound = SoundID.Item13;
            item.autoReuse = true;
            item.shoot =656; 
            item.shootSpeed = 24;
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
                Projectile.NewProjectile(vector14.X + Main.rand.Next(-63, 64),  vector14.Y + Main.rand.Next(-63, 64),0 ,0, type, damage, 1, Main.myPlayer, 0.0f, 1);
                Projectile.NewProjectile(vector14.X + Main.rand.Next(-63, 64),  vector14.Y + Main.rand.Next(-63, 64),0 ,0, type, damage, 1, Main.myPlayer, 0.0f, 1);
                Projectile.NewProjectile(vector14.X + Main.rand.Next(-63, 64),  vector14.Y + Main.rand.Next(-63, 64),0 ,0, type, damage, 1, Main.myPlayer, 0.0f, 1);
            
					return false;
					
          }  
	}
}
