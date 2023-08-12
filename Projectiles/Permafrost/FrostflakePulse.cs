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
using SOTS.Dusts;
using SOTS.Projectiles.Celestial;

namespace SOTS.Projectiles.Permafrost
{    
    public class FrostflakePulse : ModProjectile
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(hasHit);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			hasHit = reader.ReadBoolean();
        }
        public Color blue = new Color(116, 125, 238, 0);
		public Color blue2 = new Color(84, 89, 197, 0);
		List<FireParticle> particleList = new List<FireParticle>();
		int removedCounter = 0;
		Vector2 trueVelocity = Vector2.Zero;
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--;
					removedCounter++;
				}
				else
					particle.position += trueVelocity * 0.8f;
			}
		}
		bool runOnce = true;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Frostflake Pulse");
		}
        public override bool ShouldUpdatePosition()
        {
            return (int)Projectile.ai[0] == -1;
        }
        public override void SetDefaults()
        {
			Projectile.height = 16;
			Projectile.width = 16;
            Main.projFrames[Projectile.type] = 4;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = false;
			Projectile.timeLeft = 90;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 15;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = Color.Lerp(blue, blue2, ((removedCounter + i) % 10) / 10f);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					Main.spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(1, 1), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.15f, SpriteEffects.None, 0f);
				}
			}
			int frostFlake = (int)Projectile.ai[0];
			if (counter > 20 && counter < 60 && frostFlake == 2)
            {
				float alphaMult = (counter - 20) / 10f;
				if (alphaMult > 1)
					alphaMult = 1;
				float otherMult = (counter - 30) / 30f;
				otherMult = (float)Math.Pow(MathHelper.Clamp(otherMult, 0, 1), 2f);
				alphaMult -= otherMult;
				float scale = 0.5f + expandAmt / 40f;
				float dist1 = 10 * scale;
				float dist2 = 8 * scale;
				SOTSProjectile.DrawStar(Projectile.Center, alphaMult * 1.75f, 0, MathHelper.ToRadians(30), 6, dist1, dist2, 0.9f, 270);
				SOTSProjectile.DrawStar(Projectile.Center, alphaMult * 1.0f, 0, MathHelper.ToRadians(30), 6, dist1 * 0.4f, dist2 * 0.4f, 0.9f);
			}
			return false;
		}
        public override bool PreAI()
		{
			if (runOnce)
			{
				int frostFlake = (int)Projectile.ai[0];
				float spinCounter = Projectile.ai[1];
				Vector2 manipulateVelo = Projectile.velocity;
				if (frostFlake == 1)
				{
					SOTSUtils.PlaySound(SoundID.Item50, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.1f, 0.1f); //mine ice
					for (int k = 0; k < 30; k++)
					{
						if (!SOTS.Config.lowFidelityMode || Main.rand.NextBool(4))
						{
							Vector2 circularLocation = new Vector2(0, 8).RotatedBy(MathHelper.ToRadians(k * 12));
							circularLocation = circularLocation.RotatedBy(manipulateVelo.ToRotation());
							Dust dust = Dust.NewDustDirect(Projectile.Center + circularLocation + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
							dust.noGravity = true;
							dust.scale = dust.scale * 0.5f + 1f;
							dust.velocity = dust.velocity * 0.3f + circularLocation * 0.2f + manipulateVelo * 0.08f;
							dust.fadeIn = 0.1f;
						}
					}
				}
				else if(frostFlake == 2)
				{
					SOTSUtils.PlaySound(SoundID.Item105, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.1f, -0.4f); //starfury
					for (int i = 0; i < 3; i++)
					{
						Vector2 spawnPos = Vector2.Lerp(Projectile.Center + manipulateVelo.SafeNormalize(Vector2.Zero) * 8, Projectile.Center + manipulateVelo.SafeNormalize(Vector2.Zero) * -120f, i * 0.3f);
						float percent = (1 - 0.3f * i);
						float dist1 = 16 * percent;
						float dist2 = 12 * percent;
						SOTSProjectile.DustStar(spawnPos, manipulateVelo * 0.4f, manipulateVelo.ToRotation(), 40 - i * 5, spinCounter + MathHelper.ToRadians(30 * i), 6, dist1, dist2, 0.6f, 1.2f - i * 0.15f);
					}
				}
				runOnce = false;
			}
			if(hasHit && (int)Projectile.ai[0] == -1)
            {
				Projectile.friendly = false;
				trueVelocity = Vector2.Zero;
				return false;
            }
			return base.PreAI();	
        }
		float expandVelocity = 0;
		float expandAmt = 0;
		private int counter = 0;
        public override void AI()
		{
			int frostFlake = (int)Projectile.ai[0];
			Projectile.oldPosition = Projectile.position - trueVelocity;
			if(counter < 20 && frostFlake == -2)
            {
				counter = 20;
			}
			if (counter < 10 && frostFlake == 2)
			{
				counter = 10;
			}
			if (frostFlake == -1)
            {
				if(counter >= 17)
					Projectile.tileCollide = true;
				Projectile.friendly = true;
            }
			counter++;
			if (counter > 20 && frostFlake != -1)
			{
				trueVelocity *= 0.0f;
				if (counter == 21)
				{
					if (frostFlake != -2)
						SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f, -0.2f);
					else
						SOTSUtils.PlaySound(SoundID.Item50, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.75f, 0.35f);
					for (int i = 0; i < 30; i++)
					{
						Vector2 circular = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(i * 12));
						Vector2 rotational = new Vector2(0, -1.5f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						if (!SOTS.Config.lowFidelityMode || Main.rand.NextBool(3))
						{
							if (frostFlake == 1 || (frostFlake == -2 && Main.rand.NextBool(2)))
							{
								Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
								dust.noGravity = true;
								dust.velocity *= 0.2f;
								dust.velocity += circular * 0.5f;
								dust.scale *= 1.75f;
								dust.fadeIn = 0.1f;
								dust.color = Color.Lerp(blue, blue2, Main.rand.NextFloat(1));
							}
							else
							{
								circular *= 1.5f;
							}
							if (frostFlake != -2 || Main.rand.NextBool(2))
								particleList.Add(new FireParticle(Projectile.Center + circular - rotational * 2, rotational + circular * 0.05f, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.8f, 0.9f)));
						}
					}
					if (frostFlake == 2)
					{
						DustStar(180, MathHelper.ToRadians(30));
						expandVelocity = 3;
						if(Main.myPlayer == Projectile.owner)
                        {
							for(int i = -4; i <= 4; i++)
							{
								Vector2 velocity = new Vector2(i, 9 - Math.Abs(i) * 0.4f) * 0.3f;
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<FrostflakePulse>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, -1, 0);
							}
                        }
					}
				}
				Projectile.friendly = true;
				if (counter > 30)
					Projectile.friendly = false;
			}
			else
			{
				float min = 0;
				float max = 20f;
				if (frostFlake == 2)
				{
					max = 10f;
					min = 10;
				}
				float mult = (counter - min) / max;
				if (frostFlake == 2)
				{
					trueVelocity += new Vector2(0, -(float)Math.Sin(mult * MathHelper.Pi) * 6f);
					for (int i = 0; i < 5; i++)
					{
						if(!SOTS.Config.lowFidelityMode || Main.rand.NextBool(4))
						{
							Vector2 spawnPos = Vector2.Lerp(Projectile.Center, Projectile.oldPosition + Projectile.Size / 2, i * 0.2f);
							Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, Color.Lerp(blue, blue2, Main.rand.NextFloat(1)));
							dust.noGravity = true;
							dust.scale = 1.2f;
							dust.velocity = Vector2.Zero;
							dust.fadeIn = 0.1f;
						}
					}
				}
				if(frostFlake == -1)
				{
					if(counter < 16)
						Projectile.velocity = Projectile.velocity * 1.16f;
					trueVelocity = Projectile.velocity;
					for (int i = 0; i < 2; i++)
					{
						if (!SOTS.Config.lowFidelityMode || Main.rand.NextBool(3))
						{
							Vector2 spawnPos = Vector2.Lerp(Projectile.Center, Projectile.oldPosition + Projectile.Size / 2, i * 0.5f);
							Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, Color.Lerp(blue, blue2, Main.rand.NextFloat(1)));
							dust.noGravity = true;
							dust.scale = 1.0f;
							dust.velocity = Vector2.Zero;
							dust.fadeIn = 0.1f;
						}
					}
				}
				Vector2 rotational = new Vector2(0, -2.0f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30f, 30f)));
				if(frostFlake != -1)
				{
					rotational.X *= 0.25f;
					rotational.Y *= 0.75f;
					rotational = rotational.SafeNormalize(Vector2.Zero) * 3f + new Vector2(0, -1);
				}
				else
                {
					rotational = Projectile.velocity * 0.1f;
					mult = 0.5f;
                }
				particleList.Add(new FireParticle(Projectile.Center - rotational * 0.5f, rotational, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), mult * Main.rand.NextFloat(0.9f, 1.3f)));
				if(frostFlake == 1 && Main.rand.NextBool(2))
				{
					if (!SOTS.Config.lowFidelityMode || Main.rand.NextBool(2))
					{
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.3f + mult * 0.8f;
						dust.velocity += trueVelocity * 0.3f;
						dust.scale *= 1.1f;
						dust.fadeIn = 0.1f;
						dust.color = Color.Lerp(blue, blue2, Main.rand.NextFloat(1));
					}
				}
			}
			expandAmt += expandVelocity;
			expandVelocity *= 0.96f;
		}
        public override void PostAI()
		{
			if((int)Projectile.ai[0] != -1)
				Projectile.Center += trueVelocity;
			cataloguePos();
        }
        public void DustStar(int total = 30, float spin = 30, int pointAmount = 6, float innerDistAdd = 10, float innerDistMin = 8, float xCompress = 0.95f)
		{
			for (float k = 0; k < total; k++)
			{
				float rad = MathHelper.ToRadians(k * 360f / total);
				float x = (float)Math.Cos(rad);
				float y = (float)Math.Sin(rad);
				float mult = (Math.Abs((rad * (pointAmount / 2) % (float)Math.PI) - (float)Math.PI / 2) * innerDistAdd) + innerDistMin;//triangle wave function
				Vector2 circular = new Vector2(x, y).RotatedBy(spin) * mult;
				circular.X *= xCompress;
				Dust dust = Dust.NewDustDirect(circular + Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
				dust.noGravity = true;
				dust.scale = dust.scale * 0.4f + 1.2f;
				dust.velocity = dust.velocity * 0.1f + trueVelocity + circular * 0.24f;
				dust.fadeIn = 0.1f;
			}
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			if((int)Projectile.ai[0] != -1)
			{
				int width = 80;
				hitbox = new Rectangle((int)(Projectile.Center.X - width / 2), (int)(Projectile.Center.Y - width / 2), width, width);
			}
		}
		bool hasHit = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if(!hasHit)
			{
				hasHit = true;
				if (Main.myPlayer == Projectile.owner && (int)Projectile.ai[0] == -1)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FrostflakePulse>(), Projectile.damage, 0, Main.myPlayer, -2, 0);
				Projectile.netUpdate = true;
			}
			return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (!hasHit)
			{
				hasHit = true;
				target.AddBuff(BuffID.Frostburn, 120, false);
				if (Main.myPlayer == Projectile.owner && (int)Projectile.ai[0] == -1)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FrostflakePulse>(), Projectile.damage, 0, Main.myPlayer, -2, 0);
				Projectile.netUpdate = true;
			}
		}
	}
}