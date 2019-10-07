using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.SpecialDrops.Legendary
{
	public class TheMelter : VoidItem
	{
		int frozenTime = 480;
		int voidCost2 = 160;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Insignius");
			Tooltip.SetDefault("Legendary drop\nLevels up as you progress\nCalls down a chain explosion from the heavens\n'EXPLOSION!'");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 40;
            item.magic = true;  
            item.width = 60;    
            item.height = 64;   
            item.noUseGraphic = false;
            item.magic = true;   
			item.channel = true;
			item.autoReuse = false;
			item.knockBack = 1f;   
            item.rare = 10;  
            item.useTime = 480; 
            item.UseSound = SoundID.Item13;  
            item.useStyle = 5;  
            item.shootSpeed = 2;       
            item.useAnimation = 480;                       
            item.shoot = mod.ProjectileType("ExplosionCast"); 
			Item.staff[item.type] = true; 
            item.value = Item.sellPrice(1, 25, 0, 0);
			item.noMelee = true;
			
		}
		public override void GetVoid(Player player)
		{
			if(SOTSWorld.legendLevel == 1)
			{
				voidCost2 = 159;
			}
			if(SOTSWorld.legendLevel == 2)
			{
				voidCost2 = 157;
			}
			if(SOTSWorld.legendLevel == 3)
			{
				voidCost2 = 154;
			}
			if(SOTSWorld.legendLevel == 4)
			{
				voidCost2 = 150;
			}
			if(SOTSWorld.legendLevel == 5)
			{
				voidCost2 = 145;
			}
			if(SOTSWorld.legendLevel == 6)
			{
				voidCost2 = 139;
			}
			if(SOTSWorld.legendLevel == 7)
			{
				voidCost2 = 132;
			}
			if(SOTSWorld.legendLevel == 8)
			{
				voidCost2 = 124;
			}
			if(SOTSWorld.legendLevel == 9)
			{
				voidCost2 = 112;
			}
			if(SOTSWorld.legendLevel == 10)
			{
				voidCost2 = 100;
			}
			if(SOTSWorld.legendLevel == 11)
			{
				voidCost2 = 95;
			}
			if(SOTSWorld.legendLevel == 12)
			{
				voidCost2 = 90;
			}
			if(SOTSWorld.legendLevel == 13)
			{
				voidCost2 = 85;
			}
			if(SOTSWorld.legendLevel == 14)
			{
				voidCost2 = 80;
			}
			if(SOTSWorld.legendLevel == 15)
			{
				voidCost2 = 75;
			}
			if(SOTSWorld.legendLevel == 16)
			{
				voidCost2 = 70;
			}
			if(SOTSWorld.legendLevel == 17)
			{
				voidCost2 = 65;
			}
			if(SOTSWorld.legendLevel == 18)
			{
				voidCost2 = 60;
			}
			if(SOTSWorld.legendLevel == 19)
			{
				voidCost2 = 55;
			}
			if(SOTSWorld.legendLevel == 20)
			{
				voidCost2 = 50;
			}
			if(SOTSWorld.legendLevel == 21)
			{
				voidCost2 = 50;
			}
			if(SOTSWorld.legendLevel == 22)
			{
				voidCost2 = 50;
			}
			if(SOTSWorld.legendLevel == 23)
			{
				voidCost2 = 50;
			}
			if(SOTSWorld.legendLevel == 24)
			{
				voidCost2 = 50;
			}
			if(SOTSWorld.legendLevel == 25)
			{
				voidCost2 = 50;
			}
			voidMana = voidCost2;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			player.AddBuff(mod.BuffType("FrozenThroughTime"), (int)(frozenTime * 1.25f), false);	
            int numberProjectiles = 1;  //This defines how many projectiles to shot
            for (int index = 0; index < numberProjectiles; ++index)
            {
                Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
                vector2_1.Y -= (float)(100 * index);
                float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if ((double)num13 < 0.0) num13 *= -1f;
                if ((double)num13 < 20.0) num13 = 20f;
                float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                float num15 = item.shootSpeed / num14;
                float num16 = num12 * num15;
                float num17 = num13 * num15;
                float SpeedX = num16;  //this defines the projectile X position speed and randomnes
                float SpeedY = num17;  //this defines the projectile Y position speed and randomnes
				Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX * 0.02f, SpeedY * 0.02f, mod.ProjectileType("Explosion"), damage, 1, Main.myPlayer, 0.0f, 2);
           
                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, type, damage, 1, Main.myPlayer, 0.0f, 2);
            }
            return false;
		}
		public override void UpdateInventory(Player player)
		{
		    frozenTime = 480;
		    voidCost2 = 160;
			item.damage = 40;
			
			if(SOTSWorld.legendLevel == 1)
			{
				item.damage = 45;
				frozenTime = 470;
				voidCost2 = 159;
			}
			if(SOTSWorld.legendLevel == 2)
			{
				item.damage = 60;
				frozenTime = 460;
				voidCost2 = 157;
			}
			if(SOTSWorld.legendLevel == 3)
			{
				item.damage = 70;
				frozenTime = 450;
				voidCost2 = 154;
			}
			if(SOTSWorld.legendLevel == 4)
			{
				item.damage = 90;
				frozenTime = 438;
				voidCost2 = 150;
			}
			if(SOTSWorld.legendLevel == 5)
			{
				item.damage = 100;
				frozenTime = 425;
				voidCost2 = 145;
			}
			if(SOTSWorld.legendLevel == 6)
			{
				item.damage = 120;
				frozenTime = 410;
				voidCost2 = 139;
			}
			if(SOTSWorld.legendLevel == 7)
			{
				item.damage = 140;
				frozenTime = 393;
				voidCost2 = 132;
			}
			if(SOTSWorld.legendLevel == 8)
			{
				item.damage = 150;
				frozenTime = 380;
				voidCost2 = 124;
			}
			if(SOTSWorld.legendLevel == 9)
			{
				item.damage = 170;
				frozenTime = 370;
				voidCost2 = 112;
			}
			if(SOTSWorld.legendLevel == 10)
			{
				item.damage = 190;
				frozenTime = 350;
				voidCost2 = 100;
			}
			if(SOTSWorld.legendLevel == 11)
			{
				item.damage = 210;
				frozenTime = 345;
				voidCost2 = 95;
			}
			if(SOTSWorld.legendLevel == 12)
			{
				item.damage = 230;
				frozenTime = 330;
				voidCost2 = 90;
			}
			if(SOTSWorld.legendLevel == 13)
			{
				item.damage = 245;
				frozenTime = 300;
				voidCost2 = 85;
			}
			if(SOTSWorld.legendLevel == 14)
			{
				item.damage = 250;
				frozenTime = 280;
				voidCost2 = 80;
			}
			if(SOTSWorld.legendLevel == 15)
			{
				item.damage = 275;
				frozenTime = 260;
				voidCost2 = 75;
			}
			if(SOTSWorld.legendLevel == 16)
			{
				item.damage = 325;
				frozenTime = 250;
				voidCost2 = 70;
			}
			if(SOTSWorld.legendLevel == 17)
			{
				item.damage = 340;
				frozenTime = 240;
				voidCost2 = 65;
			}
			if(SOTSWorld.legendLevel == 18)
			{
				item.damage = 360;
				frozenTime = 230;
				voidCost2 = 60;
			}
			if(SOTSWorld.legendLevel == 19)
			{
				item.damage = 380;
				frozenTime = 220;
				voidCost2 = 55;
			}
			if(SOTSWorld.legendLevel == 20)
			{
				item.damage = 400;
				frozenTime = 210;
				voidCost2 = 50;
			}
			if(SOTSWorld.legendLevel == 21)
			{
				item.damage = 420;
				frozenTime = 200;
				voidCost2 = 50;
			}
			if(SOTSWorld.legendLevel == 22)
			{
				item.damage = 440;
				frozenTime = 180;
				voidCost2 = 50;
			}
			if(SOTSWorld.legendLevel == 23)
			{
				item.damage = 460;
				frozenTime = 150;
				voidCost2 = 50;
			}
			if(SOTSWorld.legendLevel == 24)
			{
				item.damage = 480;
				frozenTime = 125;
				voidCost2 = 50;
			}
			if(SOTSWorld.legendLevel == 25)
			{
				item.damage = 500;
				frozenTime = 100;
				voidCost2 = 50;
			}
			
            item.useTime = frozenTime; 
            item.useAnimation = frozenTime; 
		
		}
	}
}

