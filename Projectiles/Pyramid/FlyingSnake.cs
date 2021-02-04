using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

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
            projectile.minionSlots = 0f;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 40;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		int counter = 0;
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.rotation);
			writer.Write(projectile.spriteDirection);
			writer.Write(projectile.frame);
			writer.Write(projectile.timeLeft);
			writer.Write(counter);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.rotation = reader.ReadSingle();
			projectile.spriteDirection = reader.ReadInt32();
			projectile.frame = reader.ReadInt32();
			projectile.timeLeft = reader.ReadInt32();
			counter = reader.ReadInt32();
		}
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			
			projectile.tileCollide = true;
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
			float minDist = 480;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float dXP = 0f;
			float dYP = 0f;
			float distanceP = 0;
			float speed = 0.8f + projectile.ai[0] * 0.15f; 
			bool foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				int i = player.MinionAttackTargetNPC;
				NPC target = Main.npc[i];
				if (target.CanBeChasedBy())
				{
					bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height);
					dX = target.Center.X - projectile.Center.X;
					dY = target.Center.Y - projectile.Center.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					if (distance < 800 && lineOfSight)
					{
						minDist = distance;
						target2 = i;
						dXP = target.Center.X - player.Center.X;
						dYP = target.Center.Y - player.Center.Y;
						distanceP = (float)Math.Sqrt((double)(dXP * dXP + dYP * dYP));
						foundTarget = true;
					}
				}
			}
			if (projectile.friendly == true && projectile.hostile == false)
			{
				for(int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if(target.CanBeChasedBy() && !foundTarget)
					{
						bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height);
						dX = target.Center.X - projectile.Center.X;
						dY = target.Center.Y - projectile.Center.Y;
						distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
						if(distance < minDist && lineOfSight)
						{
							minDist = distance;
							target2 = i;
							dXP = target.Center.X - player.Center.X;
							dYP = target.Center.Y - player.Center.Y;
							distanceP = (float) Math.Sqrt((double)(dXP * dXP + dYP * dYP));
							foundTarget = true;
						}
					}
				}
				counter++;
				int unique = (int)projectile.ai[0] % 3;

				if(Main.myPlayer == projectile.owner)
				{
					if(counter % 2 == 0)
						projectile.netUpdate = true;
				}
				float playerdX = player.Center.X - projectile.Center.X;
				float playerdY = player.Center.Y - projectile.Center.Y - 90;
				Vector2 playerd = new Vector2(playerdX, playerdY) + new Vector2(0, -45).RotatedBy(MathHelper.ToRadians((unique * 120) + counter * 2));
				float playerDistance = (float)Math.Sqrt((double)(playerd.X * playerd.X + playerd.Y * playerd.Y));
				
				if (playerDistance > 1200f && !foundTarget)
				{
					if (player.active == true)
					{
						projectile.position.X = player.position.X;
						projectile.position.Y = player.position.Y;
					}
				}
				if (!foundTarget)
				{
					projectile.tileCollide = false;
				}
				else
				{
					projectile.tileCollide = true;
				}

				if (target2 != -1 && playerDistance <= 848 && distanceP <= 848)
				{
					NPC toHit = Main.npc[target2];
					if(toHit.active == true)
					{
						dX = toHit.Center.X - projectile.Center.X;
						dY = toHit.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
						projectile.velocity *= 0.996f;
						projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
				else if(playerDistance <= 400)
				{
					if(playerDistance <= 120)
					{
						if (player.active == true)
						{
							speed /= playerDistance;
							projectile.velocity *= 0.8f;
							if (speed > playerDistance * 0.6f)
							{
								speed = playerDistance * 0.6f;
							}
							projectile.velocity += new Vector2(playerd.X * speed, playerd.Y * speed);
						}
					}
					else
					{
						if (player.active == true)
						{
							speed /= playerDistance;
							projectile.velocity *= 0.97f;
							projectile.velocity += new Vector2(playerd.X * speed, playerd.Y * speed) * 1.2f;
						}
					}
				}
				else if(playerDistance >= 200)
				{
					if(player.active == true)
					{
						speed /= playerDistance;
						projectile.velocity *= 0.94f;
						projectile.velocity += new Vector2(playerd.X * speed, playerd.Y * speed) * 1.2f;
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
		