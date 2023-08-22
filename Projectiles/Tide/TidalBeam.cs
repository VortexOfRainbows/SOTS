using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Constructs;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Tide
{
	public class TidalBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tidal Beam");
		}
		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.hostile = true;
			Projectile.friendly = true;
			Projectile.timeLeft = 570;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.SourceDamage *= 2;
			if (target.type == ModContent.NPCType<TidalConstruct>())
				modifiers.SourceDamage *= 7;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item93, target.Center);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(end);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			end = reader.ReadBoolean();
        }
        List<Vector2>[] TrailPositions = new List<Vector2>[4];
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			//end = true;
			//Projectile.netUpdate = true;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return true;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < TrailPositions.Length; k++)
			{
				List<Vector2> trails = TrailPositions[k];
				Color color = new Color(60, 60, 90, 0);
				color = Projectile.GetAlpha(color);
				for (int j = 0; j < trails.Count; j++)
				{
					Vector2 drawPos = trails[j] - Main.screenPosition;
					if(!Projectile.oldPos[k].Equals(Projectile.position))
					{
						for(int i = 3; i > 0; i--)
						{
							Vector2 random = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
							Main.spriteBatch.Draw(texture, drawPos + random, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 0.20f, SpriteEffects.None, 0f);
						}
					}
				}
			}
			return false;
		}
        public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Color color = new Color(100, 100, 140, 0);
			color = Projectile.GetAlpha(color);
			for (int i = 3; i > 0; i--)
			{
				Vector2 random = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
				Main.spriteBatch.Draw(texture, drawPos + random, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 1f, SpriteEffects.None, 0f);
			}
		}
        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.FireworkFountain_Blue);
				Main.dust[dust].velocity *= 3f;
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].velocity += Projectile.velocity;
				Main.dust[dust].noGravity = true;
			}
		}
        public override bool? CanHitNPC(NPC target)
        {
            return target.Hitbox.Intersects(Projectile.Hitbox);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			for (int i = 0; i < TrailPositions.Length; i++)
			{
				List<Vector2> trail = TrailPositions[i];
				for(int j = 0; j < trail.Count; j++)
                {
					projHitbox = new Rectangle((int)trail[j].X - 4, (int)trail[j].Y - 4, 8, 8);
					if(projHitbox.Intersects(targetHitbox))
                    {
						return true;
                    }
                }
			}
			return base.Colliding(projHitbox, targetHitbox);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item93, target.Center);
			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.FireworkFountain_Blue);
				Main.dust[dust].velocity *= 3f;
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].velocity -= target.velocity;
				Main.dust[dust].noGravity = true;
			}
        }
        bool runOnce = true;
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		float stabalizeBendRatio = 1f;
		int count = 0;
		public void MoveTrailsTowardProjectile()
        {
			for(int i = 0; i < TrailPositions.Length; i++)
			{
				if (TrailPositions[i].Count > 0)
				{
					List<Vector2> trail = TrailPositions[i];
					Vector2 latestPos = trail[trail.Count - 1];
					Vector2 dynamicAddition = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i * 90 + counter * 2));
					Vector2 toProjectile = latestPos - (Projectile.Center + dynamicAddition);
					float speed = 2.25f * Projectile.velocity.Length();
					if (speed > toProjectile.Length())
					{
						speed = toProjectile.Length();
					}
					Vector2 additional = Vector2.Zero;
					if (trail.Count < 10)
						additional += Projectile.velocity * 0.5f;
					latestPos -= toProjectile.SafeNormalize(Vector2.Zero) * speed + additional;
					TrailPositions[i].Add(latestPos);
				}
            }
			count++;
			if(count % 4 == 0 || end)
			{
				for (int i = 0; i < TrailPositions.Length; i++)
				{
					for(int k = 0; k < (end ? 5 : 1); k++)
					{
						if(TrailPositions[i].Count > 0)
						{
							if(Main.rand.NextBool(2))
							{
								int dust = Dust.NewDust(TrailPositions[i][0] - new Vector2(5), 0, 0, DustID.FireworkFountain_Blue);
								Main.dust[dust].velocity *= 1.4f;
								Main.dust[dust].scale *= 0.9f;
								Main.dust[dust].noGravity = true;
							}
							TrailPositions[i].RemoveAt(0);
						}
						else
						{
							Projectile.Kill();
                        }
					}
				}
				if (end)
				{
					if (TrailPositions[0].Count > 0)
						SOTSUtils.PlaySound(SoundID.Item27, (int)TrailPositions[0][0].X, (int)TrailPositions[0][0].Y, 0.725f);
				}
			}
			for (int i = 0; i < TrailPositions.Length; i++)
			{
				List<Vector2> trail = TrailPositions[i];
				for(int j = -1; j < count / 30; j++)
				{
					if (Main.rand.NextBool(120))
					{
						if (TrailPositions[i].Count > 1)
						{ 
							int dust = Dust.NewDust(TrailPositions[i][Main.rand.Next(0, trail.Count - 1)] - new Vector2(5), 0, 0, DustID.FireworkFountain_Blue);
							Main.dust[dust].velocity *= 2f;
							Main.dust[dust].scale *= 0.7f;
							Main.dust[dust].noGravity = true;
						}
					}
				}
			}
		}
		bool end = false;
		int counter = 0;
		public override void AI()
		{
			counter++;
			Lighting.AddLight(Projectile.Center, 0.25f, 0.25f, 0.75f);
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item122, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f);
				for (int i = 0; i < TrailPositions.Length; i++)
				{
					TrailPositions[i] = new List<Vector2>();
				}
				if (Projectile.ai[1] != -1)
				{
					NPC npc = Main.npc[(int)Projectile.ai[1]];
					if (npc.active && npc.type == ModContent.NPCType<TidalConstruct>())
					{
						TidalConstruct tidalConstruct = (TidalConstruct)npc.ModNPC;
						for (int i = 0; i < tidalConstruct.projectiles.Length; i++)
						{
							Projectile tidalTrail = Main.projectile[tidalConstruct.projectiles[i]];
							if (tidalTrail.type == ModContent.ProjectileType<TidalConstructTrail>() && tidalTrail.active)
							{
								TrailPositions[i].Add(tidalTrail.Center + Projectile.velocity);
							}
						}
					}
				}
				Projectile.rotation = Projectile.velocity.ToRotation();
				runOnce = false;
			}
			NPC npc2 = Main.npc[(int)Projectile.ai[1]];
			if (Projectile.timeLeft < 30 || end || !(npc2.active && npc2.type == ModContent.NPCType<TidalConstruct>()))
            {
				Projectile.timeLeft = 29;
				end = true;
            }
			if(counter % 30 == 29 && !end)
            {
				SOTSUtils.PlaySound(SoundID.Item15, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.8f);
            }
			MoveTrailsTowardProjectile();
			//Vector2 varyingVelocity = new Vector2(1.5f, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] * 2));
			Projectile.position += Projectile.velocity; // + new Vector2(varyingVelocity.X, 0).RotatedBy(Projectile.rotation);
			Player target = Main.player[(int)Projectile.ai[0]];
			if (target.ZoneDungeon)
			{
				Projectile.tileCollide = false;
			}
			else
			{
				Projectile.tileCollide = true;
			}
			if (stabalizeBendRatio < 3)
				stabalizeBendRatio += 0.0005f;
			if(target.active && !target.dead)
            {
				float speed = Projectile.velocity.Length() + 0.001f;
				Vector2 normalizeVelo = Projectile.velocity.SafeNormalize(new Vector2(1, 0));
				Vector2 toPlayer = target.Center - Projectile.Center;
				toPlayer = toPlayer.SafeNormalize(new Vector2(1, 0));
				toPlayer *= 4 * stabalizeBendRatio;
				normalizeVelo *= 48 / stabalizeBendRatio;
				Projectile.velocity = toPlayer + normalizeVelo;
				Projectile.velocity = Projectile.velocity.SafeNormalize(new Vector2(1, 0)) * speed;
			}
		}
	}
}