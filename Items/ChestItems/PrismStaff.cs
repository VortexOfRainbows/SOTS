using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.ChestItems
{  
    public class PrismStaff : ModItem
    {
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prism Staff");
			Tooltip.SetDefault("Summons a prism that will split your projectiles into 4\nDisappears after spliting 10 projectiles\nIncreases max minions by 1 while in your inventory");
		}
        public override void SetDefaults()
        {
           
            item.damage = 12;
            item.summon = true;
            item.mana = 12;
            item.width = 48;
            item.height = 48;
            item.useTime = 90;
            item.useAnimation = 90;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 52500;
            item.rare = 3;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("Prism");
            item.shootSpeed = 7f;
			item.buffType = mod.BuffType("Prism");
            item.buffTime = 3600;
			item.autoReuse = true;
        }
		public override void UpdateInventory(Player player)
		{
			player.maxMinions += 1;
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
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 0, type, damage, 1, Main.myPlayer, 0.0f, 1);
            
					return false;
					
          }  
    }
}