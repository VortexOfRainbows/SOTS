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
			Projectile.width = 50;
			Projectile.height = 26;
            Main.projFrames[Projectile.type] = 4;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 960;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.alpha = 0;
            Projectile.minionSlots = 0f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 16;
			height = 16;
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		int counter = 0;
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(Projectile.rotation);
			writer.Write(Projectile.spriteDirection);
			writer.Write(Projectile.frame);
			writer.Write(Projectile.timeLeft);
			writer.Write(counter);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			Projectile.rotation = reader.ReadSingle();
			Projectile.spriteDirection = reader.ReadInt32();
			Projectile.frame = reader.ReadInt32();
			Projectile.timeLeft = reader.ReadInt32();
			counter = reader.ReadInt32();
		}
		public override void AI()
        {
			Player player = Main.player[Projectile.owner];
			if(player.whoAmI != Main.myPlayer)
            {
				Projectile.timeLeft = 20;
            }
			Projectile.tileCollide = true;
            Projectile.frameCounter++;
			if(!Projectile.active)
			{
				Projectile.Kill();
			}
            if (Projectile.frameCounter >= 9)
            {
				Projectile.friendly = true;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.spriteDirection = 1;
			if(Projectile.velocity.X < 0)
			{
				Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
				Projectile.spriteDirection = -1;
			}
			float minDist = 480;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float dXP = 0f;
			float dYP = 0f;
			float distanceP = 0;
			float speed = 0.8f + Projectile.ai[0] * 0.15f; 
			bool foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				int i = player.MinionAttackTargetNPC;
				NPC target = Main.npc[i];
				if (target.CanBeChasedBy())
				{
					bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
					dX = target.Center.X - Projectile.Center.X;
					dY = target.Center.Y - Projectile.Center.Y;
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
			if (Projectile.friendly == true && Projectile.hostile == false)
			{
				for(int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if(target.CanBeChasedBy() && !foundTarget)
					{
						bool lineOfSight = Collision.CanHitLine(Projectile.Center, 4, 4, target.position, target.width, target.height);
						dX = target.Center.X - Projectile.Center.X;
						dY = target.Center.Y - Projectile.Center.Y;
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
				int unique = (int)Projectile.ai[0] % 3;

				if(Main.myPlayer == Projectile.owner)
				{
					if(counter % 2 == 0)
						Projectile.netUpdate = true;
				}
				Vector2 circular = new Vector2(0, -48).RotatedBy(MathHelper.ToRadians((unique * 120) + counter * 2));
				circular.Y *= 0.5f;
				circular.Y -= 96f;
				Vector2 playerd = player.Center + circular;
				float playerDistance = Vector2.Distance(player.Center, Projectile.Center);
				
				if (playerDistance > 2100f && !foundTarget)
				{
					if (player.active == true)
					{
						Projectile.position.X = player.position.X;
						Projectile.position.Y = player.position.Y;
					}
				}
				if (!foundTarget)
				{
					Projectile.tileCollide = false;
				}
				else
				{
					Projectile.tileCollide = true;
				}

				if (target2 != -1 && playerDistance <= 1200 && distanceP <= 1200)
				{
					NPC toHit = Main.npc[target2];
					if(toHit.active == true)
					{
						dX = toHit.Center.X - Projectile.Center.X;
						dY = toHit.Center.Y - Projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
						Projectile.velocity *= 0.994f;
						Projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
				else
				{
					Vector2 toPlayer = playerd - Projectile.Center;
					Projectile.rotation = toPlayer.ToRotation();
					if (toPlayer.X < 0)
					{
						Projectile.rotation = toPlayer.ToRotation() - MathHelper.ToRadians(180);
						Projectile.spriteDirection = -1;
					}
					float dist = toPlayer.Length();
					toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
					float speed2 = speed * 5 + dist * 0.005f;
					if (speed2 > dist)
					{
						speed2 = dist;
					}
					Projectile.velocity = toPlayer * speed2;
				}
			}
        }
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X * 0.35f; 
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y * 0.35f;
			}
			return false;
		}
	}
}
		