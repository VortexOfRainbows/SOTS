using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class SightHeal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Cross");
			Tooltip.SetDefault("Spawns an aura of healing\nHeal all in the aura by 50 every time they're hit\nDamage is forced on those hit");
		}
		public override void SetDefaults()
		{
            item.damage = 1;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 36;     //gun image width
            item.height = 36;   //gun image  height
            item.useTime = 200;  //how fast 
            item.useAnimation = 200;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 125000;
            item.rare = 6;
            item.UseSound = SoundID.Item13;
            item.autoReuse = true;
            item.shoot =  mod.ProjectileType("SightHealFriendly"); 
            item.shootSpeed = 24;
			item.mana = 200;

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
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 0, type, damage, 0, Main.myPlayer, 0.0f, 0);
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 0,  mod.ProjectileType("SightHoney"), damage, 0, Main.myPlayer, 0.0f, 0);
            
					return false;
					
          }  
	}
}
