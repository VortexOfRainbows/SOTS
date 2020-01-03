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

namespace SOTS.Projectiles.Pyramid
{    
    public class FlyingSnake : ModProjectile 
    {	
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snakey Boi");
			
		}
		
        public override void SetDefaults()
        {
			projectile.width = 42;
			projectile.height = 32;
            Main.projFrames[projectile.type] = 4;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 960;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.minion = true;
			projectile.alpha = 0;
            projectile.netImportant = true;
            projectile.minionSlots = 0f;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.rotation);
			writer.Write(projectile.spriteDirection);
			writer.Write(projectile.frame);
			writer.Write(projectile.timeLeft);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.rotation = reader.ReadSingle();
			projectile.spriteDirection = reader.ReadInt32();
			projectile.frame = reader.ReadInt32();
			projectile.timeLeft = reader.ReadInt32();
		}
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			
			projectile.tileCollide = true;
			projectile.netUpdate = true;
            projectile.frameCounter++;
			if(!projectile.active)
			{
				projectile.Kill();
			}
            if (projectile.frameCounter >= 9)
            {
				projectile.friendly = true;
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
			float minDist = 400;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float dXP = 0f;
			float dYP = 0f;
			float distanceP = 0;
			float speed = 0.1f * Main.rand.Next(5,16);
			if(projectile.friendly == true && projectile.hostile == false)
			{
				for(int i = 0; i < Main.npc.Length - 1; i++)
				{
					NPC target = Main.npc[i];
					if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active)
					{
						dX = target.Center.X - projectile.Center.X;
						dY = target.Center.Y - projectile.Center.Y;
						distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
						if(distance < minDist)
						{
							minDist = distance;
							target2 = i;
							dXP = target.Center.X - player.Center.X;
							dYP = target.Center.Y - player.Center.Y;
							distanceP = (float) Math.Sqrt((double)(dXP * dXP + dYP * dYP));
						}
					}
				}
				
				float playerdX = player.Center.X - projectile.Center.X;
				float playerdY = player.Center.Y - projectile.Center.Y - 90;
				float playerDistance = (float)Math.Sqrt((double)(playerdX * playerdX + playerdY * playerdY));
				if(playerDistance >= 1800)
				{
					projectile.position.X = player.position.X;
					projectile.position.Y = player.position.Y;
					
				}
				
				if(target2 != -1 && playerDistance <= 848 && distanceP <= 848)
				{
					NPC toHit = Main.npc[target2];
					if(toHit.active == true)
					{
						
					dX = toHit.Center.X - projectile.Center.X;
					dY = toHit.Center.Y - projectile.Center.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					speed /= distance;
					projectile.velocity *= 0.9975f;
					projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
				else if(playerDistance <= 400)
				{
					if(player.active == true)
					{
						
					speed /= playerDistance;
					projectile.velocity *= 0.991f;
					projectile.velocity += new Vector2(playerdX * speed, playerdY * speed) * 1.2f;
					}
				}
				else if(playerDistance >= 200)
				{
					if(player.active == true)
					{
						
					speed /= playerDistance;
					projectile.velocity *= 0.95f;
					projectile.velocity += new Vector2(playerdX * speed, playerdY * speed) * 1.2f;
					}
				}
			}
        }
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X * 0.35f; 
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y * 0.35f;
				}
			return false;
		}
	}
}
		