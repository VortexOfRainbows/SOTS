using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.HolyRelics
{    
    public class Circuit : ModProjectile 
    {	int bounce = 1;           
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Circuit");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(294);
            aiType = 294; 
			projectile.thrown = true;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.magic = false;
			projectile.timeLeft = 7000;
			projectile.penetrate = -1;
		}
		public override void AI()
		{
			
			//int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 60);

			
			//Main.dust[num1].noGravity = true;
			//Main.dust[num1].velocity *= 0.1f;
			
			//int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 66);

			
			//Main.dust[num2].noGravity = true;
		//	Main.dust[num2].velocity *= 0.1f;
			
		 Player player  = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
				float Speed = modPlayer.needleSpeed;  //projectile speed 	
				if(modPlayer.needle == true)
				{
					projectile.timeLeft -= (int)(30 * Speed);
					
				}
				else
				{
				modPlayer.needleSpeed = 0.1f;
				projectile.Kill();
				}
             
		}
		public override void Kill(int timeLeft)
		{ 
			
            Player player = Main.player[Main.myPlayer];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
				float Speed = modPlayer.needleSpeed;  //projectile speed
			
                if(modPlayer.needle == true)
				{
					 Vector2 vector14;
				Main.PlaySound(SoundID.Item10, projectile.position);
				
				
				if (player.gravDir == 1f)
					{
					vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						vector14.X = (float)Main.mouseX + Main.screenPosition.X;
						
				
					modPlayer.needleSpeed *= 1.01f;
				if(Speed > 0.5f)
				{
					Speed =  0.5f;
					modPlayer.needleSpeed =  0.5f;
				}
                Vector2 vector8 = new Vector2(projectile.position.X + (projectile.width / 2), projectile.position.Y + (projectile.height / 2));
			
                int damage = (int)(projectile.damage * 1.05f);  //projectile damage
				if(damage > (int)(400 * player.thrownDamage))
				{
					damage = (int)(400 * player.thrownDamage);
				}
                int type = mod.ProjectileType("Circuit");  //put your projectile
                Main.PlaySound(23, (int)projectile.position.X, (int)projectile.position.Y, 17);
                float rotation = (float)Math.Atan2(vector8.Y - vector14.Y, vector8.X - vector14.X);
                int num54 = Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1) + (float)(Main.rand.Next(-10, 11)/100f), (float)((Math.Sin(rotation) * Speed) * -1) + (float)(Main.rand.Next(-10, 11)/100f), type, damage, 0f, 0);
						
		}
		
	}
}
}
