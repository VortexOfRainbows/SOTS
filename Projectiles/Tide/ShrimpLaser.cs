using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Tide
{
	public class ShrimpLaser : ModProjectile
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(hasHit);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			hasHit = reader.ReadBoolean();
        }
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shrimp Laser");
		}
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.extraUpdates = 3;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
		}
        Vector2[] trailPos = new Vector2[40];
		public void cataloguePos()
		{
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			triggerUpdate();
			return false;
		}
		public static Color PurpleShrimpColorAttempt(float lerp)
		{
			return Color.Lerp(new Color(169, 117, 238), new Color(165, 68, 68), lerp);
		}
		public void TrailPreDraw()
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * 0.75f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = ColorHelper.ShrimpColorGradient((float)k / (trailPos.Length - 1)) * 1.2f;
                if (Projectile.ai[0] == -1)
				{
					color = PurpleShrimpColorAttempt((float)k / (trailPos.Length - 1)) * 1.0f;
					scale *= 0.8f;
				}
				color.A = 0;
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.33f;
				float max = betweenPositions.Length() / (texture.Width * scale * 0.1f);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					if (trailPos[k] != Projectile.Center)
						Main.spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation(), drawOrigin, new Vector2(scale, (float)Math.Sqrt(scale) * 0.9f), SpriteEffects.None, 0f);
				}
				previousPosition = currentPos;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			TrailPreDraw();
			return false;
		}
		bool hasHit = false;
		bool runOnce = true;
		float counter = 0;
		public override void AI()
		{
			cataloguePos();
			Lighting.AddLight(Projectile.Center, ColorHelper.TideColor.ToVector3());
			if (runOnce)
			{
				if (Projectile.ai[0] == -1)
				{
					for (int i = 0; i < 6; i++)
					{
						int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = PurpleShrimpColorAttempt(Main.rand.NextFloat(1)) * 1.0f;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 2.2f;
						dust.velocity *= 0.5f;
						dust.velocity += Projectile.velocity * 0.6f;
						dust.alpha = 125;
					}
					trailPos = new Vector2[20];
					SOTSUtils.PlaySound(SoundID.Item91, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.3f, 0.1f);
				}
				runOnce = false;
			}
			if(Main.rand.NextBool(40) || (hasHit && Main.rand.NextBool(8)))
            {
				int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = ColorHelper.ShrimpColorGradient(Main.rand.NextFloat(1)) * 1.2f;
				if (Projectile.ai[0] == -1)
				{
					dust.color = PurpleShrimpColorAttempt(Main.rand.NextFloat(1)) * 1.2f;
				}
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.velocity *= 0.8f;
				dust.alpha = 125;
			}
			if (!Projectile.velocity.Equals(new Vector2(0, 0)))
				Projectile.rotation = Projectile.velocity.ToRotation();

			if (hasHit || Projectile.timeLeft < 60)
			{
				if (Projectile.timeLeft > 60)
					Projectile.timeLeft = 60;
				triggerUpdate();
			}
			else
			{
				float sin = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] * 1.1f)) * 0.1f;
				if (Projectile.ai[0] == -1)
					sin = 1.9f * sin;
				Projectile.Center += new Vector2(0, sin).RotatedBy(Projectile.velocity.ToRotation());
				Projectile.ai[1]++;
				counter += 0.05f;
				float homingDist = 240;
				if (Projectile.ai[0] == -1)
					homingDist = 480;
				int target = Common.GlobalNPCs.SOTSNPCs.FindTarget_Basic(Projectile.Center, homingDist, needsLOS: true);
				if (target >= 0 && counter > 1)
                {
					NPC npc = Main.npc[target];
					if(npc.CanBeChasedBy())
                    {
						Vector2 toNPC = npc.Center - Projectile.Center;
						float homing = 0.002f + counter / 400f;
						if (Projectile.ai[0] == -1)
							homing *= 2;
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * 4f, homing);
						counter++;
					}
                }
				else
				{
					Projectile.velocity *= 0.99f;
				}
			}
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (Projectile.ai[0] != -1)
				target.AddBuff(BuffID.OnFire, 600, false);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
			triggerUpdate();
		}
		public void triggerUpdate()
		{
			hasHit = true;
			Projectile.velocity *= 0;
			Projectile.friendly = false;
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
				//Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<VibrantRing>(), Projectile.damage, Projectile.knockBack * 0.1f, Main.myPlayer);
			}
		}
	}
}