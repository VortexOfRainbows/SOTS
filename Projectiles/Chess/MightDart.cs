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

namespace SOTS.Projectiles.Chess
{    
    public class MightDart : ModProjectile 
    {	int bounce = 1;            
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mighty Dart");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(294);
            aiType = 294; 
			projectile.thrown = true;
			projectile.friendly = true;
			projectile.timeLeft = 900;
			projectile.hostile = false;
			projectile.tileCollide = true;
			projectile.alpha = 255;
			projectile.penetrate = -1;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
		
			  Vector2 vector14;
		 Player player  = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
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
						
						if(Main.rand.Next(2) == 0)
						{
				modPlayer.needleSpeed *= 1.05f;
				
				float Speed = modPlayer.needleSpeed;  //projectile speed
				if(Speed > 20f)
				{
					Speed = 20f;
				}
                Vector2 vector8 = new Vector2(projectile.position.X + (projectile.width / 2), projectile.position.Y + (projectile.height / 2));
			
                int damage = (int)(projectile.damage * 1.1f);  //projectile damage
				if(damage > (int)(200 * player.thrownDamage))
				{
					damage = (int)(200 * player.thrownDamage);
				}
                int type = mod.ProjectileType("MightDart");  //put your projectile
                Main.PlaySound(23, (int)projectile.position.X, (int)projectile.position.Y, 17);
                float rotation = (float)Math.Atan2(vector8.Y - vector14.Y, vector8.X - vector14.X);
                int num54 = Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, 0);
						
			projectile.Kill();
						}
						else
						{
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}			
						}
						
			return false;
		}
		public override void AI()
		{
		 Player player  = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			
                if(modPlayer.needle == true)
				{
					projectile.timeLeft = 6;
				}
				else
				{
					modPlayer.needleSpeed = 0.1f;
				}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if(target.type == mod.NPCType("Libra"))
			{
					Main.NewText("Well, there you go... I guess I could've disguised these items better...", 255, 255, 255);
					
				
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, (mod.ItemType("SMaterial")), 1);
					target.timeLeft = 1;
					target.timeLeft--;
					target.life = 0;
					
			}
		}
	}
}
		