using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class LihzahrdWrench : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Blast");
			Tooltip.SetDefault("It casts a demonic ritual on your cursor\nGrants extreme damage but limits your movement");
		}
		public override void SetDefaults()
		{
            item.damage = 7500;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 42;     //gun image width
            item.height = 42;   //gun image  height
            item.useTime = 240;  //how fast 
            item.useAnimation = 240;
            item.useStyle = 5;    
            item.knockBack = 8;
            item.value = 10000;
            item.rare = 7;
			item.UseSound = SoundID.Item12;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("SunStarter"); 
            item.shootSpeed = 1f;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
			item.mana = 150;

		}public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {	
				player.AddBuff(mod.BuffType("FrozenThroughTime"), 600, false);
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
						
              
				  Projectile.NewProjectile(vector14.X, vector14.Y, 0, 0f, mod.ProjectileType("SunStarter"), damage, knockBack, player.whoAmI);
				
				
				
				
				return false;
			
			
			
			
		}
	}
}
