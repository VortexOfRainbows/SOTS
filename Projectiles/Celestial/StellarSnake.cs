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

namespace SOTS.Projectiles.Celestial
{    
    public class StellarSnake : ModProjectile 
    {	
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snakey Boi Part II");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;    
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 1;
			projectile.height = 24;
			projectile.width = 42;
            Main.projFrames[projectile.type] = 4;
			projectile.penetrate = 9;
			projectile.friendly = true;
			projectile.timeLeft = 3600;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.alpha = 0;
			projectile.netImportant = true;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.rotation);
			writer.Write(projectile.spriteDirection);
			writer.Write(damageCounter);
			writer.Write(latch);
			writer.Write(enemyIndex);
			writer.Write(diffPosX);
			writer.Write(diffPosY);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.rotation = reader.ReadSingle();
			projectile.spriteDirection = reader.ReadInt32();
			damageCounter = reader.ReadInt32();
			latch = reader.ReadBoolean();
			enemyIndex = reader.ReadInt32();
			diffPosX = reader.ReadSingle();
			diffPosY = reader.ReadSingle();
		}
		int damageCounter = 0;
		bool latch = false;
		int enemyIndex = -1;
		float diffPosX = 0;
		float diffPosY = 0;
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			
			projectile.tileCollide = true;
			if(projectile.penetrate <= 4)
			{
				projectile.tileCollide = false;
			}
			
            projectile.frameCounter++;
            if (projectile.frameCounter >= 9)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 4;
            }
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			projectile.spriteDirection = 1;
			if(projectile.velocity.X < 0)
			{
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - MathHelper.ToRadians(180);
				projectile.spriteDirection = -1;
			}
			if(latch && enemyIndex != -1)
			{
				projectile.netUpdate = true;
				NPC target = Main.npc[enemyIndex];
				if(target.active && !target.friendly)
				{
					projectile.aiStyle = 0;
					projectile.position.X = target.Center.X - projectile.width/2 - diffPosX;
					projectile.position.Y = target.Center.Y - projectile.height/2 - diffPosY;
				}
				else
				{
					enemyIndex = -1;
					projectile.aiStyle = 1;
					latch = false;
					projectile.penetrate += 9;
					projectile.timeLeft += 120;
					projectile.damage += 12;
					projectile.damage = (int)(projectile.damage * 1.25f);
					projectile.tileCollide = true;
					projectile.friendly = true;
				}
			}
			else
			{
				float minDist = 960;
				int target2 = -1;
				float dX = 0f;
				float dY = 0f;
				float distance = 0;
				float speed = 1.55f;
				if(projectile.friendly == true && projectile.hostile == false)
				{
					for(int i = 0; i < Main.npc.Length - 1; i++)
					{
						NPC target = Main.npc[i];
						if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5)
						{
							dX = target.Center.X - projectile.Center.X;
							dY = target.Center.Y - projectile.Center.Y;
							distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
							if(distance < minDist)
							{
								minDist = distance;
								target2 = i;
							}
						}
					}
					
					if(target2 != -1)
					{
					NPC toHit = Main.npc[target2];
						if(toHit.active == true)
						{
							
						dX = toHit.Center.X - projectile.Center.X;
						dY = toHit.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
						projectile.velocity *= 0.935f;
						projectile.velocity += new Vector2(dX * speed, dY * speed);
						}
					}
				}
			}
			if(!projectile.friendly)
			{
				damageCounter++;
				if(damageCounter >= 18)
				{
					damageCounter = 0;
					projectile.friendly = true;
				}
			}
			if(projectile.damage <= 8)
			{
				projectile.Kill();
			}
			if(projectile.timeLeft % 3 == 0 && !latch)
			{
				for(int i = 0; i < 360; i += 40)
				{
				Vector2 circularLocation = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 21);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(180));
				
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			
			Player player = Main.player[projectile.owner];
			projectile.friendly = false;
            target.immune[projectile.owner] = 0;
			projectile.tileCollide = false;
			latch = true;
			projectile.damage = (int)(projectile.damage * 0.9275f);
			projectile.damage -= 2;
			for(int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				if(npc == target)
				{
					if(diffPosX == 0)
					{
						diffPosX = npc.Center.X - projectile.Center.X;
						diffPosX *= 0.75f;
					}
					if(diffPosY == 0)
					{
						diffPosY = npc.Center.Y - projectile.Center.Y;
						diffPosY *= 0.75f;
					}
					enemyIndex = i;
					break;
				}
			}
			if(target.life <= 0)
			{
					enemyIndex = -1;
					projectile.aiStyle = 1;
					latch = false;
					projectile.tileCollide = true;
					projectile.friendly = true;
			}
			for(int i = 0; i < 360; i += 15)
			{
				Vector2 circularLocation = new Vector2(-30, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(target.Center.X + circularLocation.X - 4, target.Center.Y + circularLocation.Y - 4), 4, 4, 21);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = circularLocation * -0.15f;
			}
			projectile.velocity *= 0.45f;
        }
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 360; i += 8)
			{
				Vector2 circularLocation = new Vector2(-32, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 21);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = circularLocation * 0.35f;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.damage = (int)(projectile.damage * 0.975f);
			projectile.damage -= 2;
				projectile.penetrate--;
				if(projectile.penetrate < 1)
				{
					projectile.Kill();
				}
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X * 0.55f; 
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y * 0.35f;
				}
			return false;
		}
	}
}
		