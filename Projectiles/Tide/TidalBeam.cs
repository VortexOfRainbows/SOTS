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
			DisplayName.SetDefault("Tidal Beam");
		}
		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.hostile = true;
			projectile.friendly = true;
			projectile.timeLeft = 570;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			damage *= 2;
			if (target.type == ModContent.NPCType<TidalConstruct>())
				damage *= 7;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Main.PlaySound(SoundID.Item93, target.Center);
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
			//projectile.netUpdate = true;
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return true;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < TrailPositions.Length; k++)
			{
				List<Vector2> trails = TrailPositions[k];
				Color color = new Color(60, 60, 90, 0);
				color = projectile.GetAlpha(color);
				for (int j = 0; j < trails.Count; j++)
				{
					Vector2 drawPos = trails[j] - Main.screenPosition;
					if(!projectile.oldPos[k].Equals(projectile.position))
					{
						for(int i = 3; i > 0; i--)
						{
							Vector2 random = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
							Main.spriteBatch.Draw(texture, drawPos + random, null, color, projectile.rotation, drawOrigin, projectile.scale * 0.20f, SpriteEffects.None, 0f);
						}
					}
				}
			}
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Color color = new Color(100, 100, 140, 0);
			color = projectile.GetAlpha(color);
			for (int i = 3; i > 0; i--)
			{
				Vector2 random = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
				Main.spriteBatch.Draw(texture, drawPos + random, null, color, projectile.rotation, drawOrigin, projectile.scale * 1f, SpriteEffects.None, 0f);
			}
		}
        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 221);
				Main.dust[dust].velocity *= 3f;
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].velocity += projectile.velocity;
				Main.dust[dust].noGravity = true;
			}
		}
        public override bool? CanHitNPC(NPC target)
        {
            return target.Hitbox.Intersects(projectile.Hitbox);
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
        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			Main.PlaySound(SoundID.Item93, target.Center);
			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 221);
				Main.dust[dust].velocity *= 3f;
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].velocity -= target.velocity;
				Main.dust[dust].noGravity = true;
			}
			base.OnHitPlayer(target, damage, crit);
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
					Vector2 toProjectile = latestPos - (projectile.Center + dynamicAddition);
					float speed = 2.25f * projectile.velocity.Length();
					if (speed > toProjectile.Length())
					{
						speed = toProjectile.Length();
					}
					Vector2 additional = Vector2.Zero;
					if (trail.Count < 10)
						additional += projectile.velocity * 0.5f;
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
								int dust = Dust.NewDust(TrailPositions[i][0] - new Vector2(5), 0, 0, 221);
								Main.dust[dust].velocity *= 1.4f;
								Main.dust[dust].scale *= 0.9f;
								Main.dust[dust].noGravity = true;
							}
							TrailPositions[i].RemoveAt(0);
						}
						else
						{
							projectile.Kill();
                        }
					}
				}
				if (end)
				{
					if (TrailPositions[0].Count > 0)
						Main.PlaySound(2, (int)TrailPositions[0][0].X, (int)TrailPositions[0][0].Y, 27, 0.725f);
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
							int dust = Dust.NewDust(TrailPositions[i][Main.rand.Next(0, trail.Count - 1)] - new Vector2(5), 0, 0, 221);
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
			Lighting.AddLight(projectile.Center, 0.25f, 0.25f, 0.75f);
			if (runOnce)
			{
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 122, 1.2f);
				for (int i = 0; i < TrailPositions.Length; i++)
				{
					TrailPositions[i] = new List<Vector2>();
				}
				if (projectile.ai[1] != -1)
				{
					NPC npc = Main.npc[(int)projectile.ai[1]];
					if (npc.active && npc.type == ModContent.NPCType<TidalConstruct>())
					{
						TidalConstruct tidalConstruct = (TidalConstruct)npc.modNPC;
						for (int i = 0; i < tidalConstruct.projectiles.Length; i++)
						{
							Projectile tidalTrail = Main.projectile[tidalConstruct.projectiles[i]];
							if (tidalTrail.type == ModContent.ProjectileType<TidalConstructTrail>() && tidalTrail.active)
							{
								TrailPositions[i].Add(tidalTrail.Center + projectile.velocity);
							}
						}
					}
				}
				projectile.rotation = projectile.velocity.ToRotation();
				runOnce = false;
			}
			NPC npc2 = Main.npc[(int)projectile.ai[1]];
			if (projectile.timeLeft < 30 || end || !(npc2.active && npc2.type == ModContent.NPCType<TidalConstruct>()))
            {
				projectile.timeLeft = 29;
				end = true;
            }
			if(counter % 30 == 29 && !end)
            {
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 0.8f);
            }
			MoveTrailsTowardProjectile();
			//Vector2 varyingVelocity = new Vector2(1.5f, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[0] * 2));
			projectile.position += projectile.velocity; // + new Vector2(varyingVelocity.X, 0).RotatedBy(projectile.rotation);
			Player target = Main.player[(int)projectile.ai[0]];
			if(stabalizeBendRatio < 3)
				stabalizeBendRatio += 0.0005f;
			if(target.active && !target.dead)
            {
				float speed = projectile.velocity.Length() + 0.001f;
				Vector2 normalizeVelo = projectile.velocity.SafeNormalize(new Vector2(1, 0));
				Vector2 toPlayer = target.Center - projectile.Center;
				toPlayer = toPlayer.SafeNormalize(new Vector2(1, 0));
				toPlayer *= 4 * stabalizeBendRatio;
				normalizeVelo *= 48 / stabalizeBendRatio;
				projectile.velocity = toPlayer + normalizeVelo;
				projectile.velocity = projectile.velocity.SafeNormalize(new Vector2(1, 0)) * speed;
			}
		}
	}
}