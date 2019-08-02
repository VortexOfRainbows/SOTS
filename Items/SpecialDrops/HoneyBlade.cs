using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class HoneyBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Honey Blade");
			Tooltip.SetDefault("A weirdly shaped piece of coagulated honey\nRight click secondary\nFound in pools of honey");
		}
		public override void SetDefaults()
		{
            item.damage = 43;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 42;     //gun image width
            item.height = 42;   //gun image  height
            item.useTime = 22;  //how fast 
            item.useAnimation = 22;
            item.useStyle = 1;    
            item.knockBack = 4f;
            item.value = 95000;
            item.rare = 4;
            item.UseSound = SoundID.Item18;
            item.autoReuse = true;
			item.shoot = 181;
			item.shootSpeed = 12;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
				if(player.altFunctionUse == 2){
				
				 int numberProjectiles1 = 1;
              for (int i = 0; i < numberProjectiles1; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0)); // This defines the projectiles random spread . 30 degree spread.

				  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
				
				
				}
				
				return false;
			
			
		}
		 public override bool AltFunctionUse(Player player)
        {
            return true;
        }
 
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)     //2 is right click
            {
 
                item.useTime = 7;
                item.useAnimation = 7;
				item.useStyle = 5;    
				item.damage = 21;
                item.UseSound = SoundID.Item18;
				Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
				
				item.noMelee = true; //so the item's animation doesn't do damage
 
 
 
            }
            else
            {
 
                item.useTime = 16;
                item.useAnimation = 16;
				item.useStyle = 1;    
                item.UseSound = SoundID.Item18;
				Item.staff[item.type] = false; //this makes the useStyle animate as a staff instead of as a gun
				
				item.damage = 43;
				item.reuseDelay = 0;
				item.noMelee = false; //so the item's animation doesn't do damage
			
			}
            return base.CanUseItem(player);
        }
	}
}
