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
			projectile.width = 50;
			projectile.height = 26;
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
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 16;
			height = 16;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
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
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.spriteDirection = 1;
			if(projectile.velocity.X < 0)
			{
				projectile.rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
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
						bool lineOfSight = Collision.CanHitLine(projectile.Center, 4, 4, target.position, target.width, target.height);
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
				Vector2 circular = new Vector2(0, -48).RotatedBy(MathHelper.ToRadians((unique * 120) + counter * 2));
				circular.Y *= 0.5f;
				circular.Y -= 96f;
				Vector2 playerd = player.Center + circular;
				float playerDistance = Vector2.Distance(player.Center, projectile.Center);
				
				if (playerDistance > 2100f && !foundTarget)
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

				if (target2 != -1 && playerDistance <= 1200 && distanceP <= 1200)
				{
					NPC toHit = Main.npc[target2];
					if(toHit.active == true)
					{
						dX = toHit.Center.X - projectile.Center.X;
						dY = toHit.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
						projectile.velocity *= 0.994f;
						projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
				else
				{
					Vector2 toPlayer = playerd - projectile.Center;
					projectile.rotation = toPlayer.ToRotation();
					if (toPlayer.X < 0)
					{
						projectile.rotation = toPlayer.ToRotation() - MathHelper.ToRadians(180);
						projectile.spriteDirection = -1;
					}
					float dist = toPlayer.Length();
					toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
					float speed2 = speed * 5 + dist * 0.005f;
					if (speed2 > dist)
					{
						speed2 = dist;
					}
					projectile.velocity = toPlayer * speed2;
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
		