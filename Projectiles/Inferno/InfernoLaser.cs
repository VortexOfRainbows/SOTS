using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Inferno
{
	public class InfernoLaser : ModProjectile
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
			DisplayName.SetDefault("Lava Laser");
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.extraUpdates = 4;
			projectile.timeLeft = 900;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
		}
        Vector2[] trailPos = new Vector2[60];
		public void cataloguePos()
		{
			Vector2 current = projectile.Center;
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
		public void TrailPreDraw(SpriteBatch spriteBatch)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * 0.75f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = VoidPlayer.InfernoColorAttemptDegrees(k);
				color.A = 0;
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.33f;
				float max = betweenPositions.Length() / (texture.Width * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					if (trailPos[k] != projectile.Center)
						spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
				}
				previousPosition = currentPos;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			TrailPreDraw(spriteBatch);
			return false;
		}
		bool hasHit = false;
		bool runOnce = true;
		float counter = 0;
		public override void AI()
		{
			cataloguePos();
			Lighting.AddLight(projectile.Center, VoidPlayer.Inferno1.ToVector3());
			if (runOnce)
			{
				runOnce = false;
				//Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 60, 0.8f, -0.1f);
			}
			if(Main.rand.NextBool(40) || (hasHit && Main.rand.NextBool(8)))
            {
				int dust2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1));
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.velocity *= 0.8f;
				dust.alpha = 125;
			}
			if (!projectile.velocity.Equals(new Vector2(0, 0)))
				projectile.rotation = projectile.velocity.ToRotation();

			if (hasHit)
			{
				if (projectile.timeLeft > 60)
					projectile.timeLeft = 60;
			}
			else
			{
				float sin = (float)Math.Sin(MathHelper.ToRadians(projectile.ai[1] * 1.1f)) * 0.25f;
				projectile.Center += new Vector2(0, sin).RotatedBy(projectile.velocity.ToRotation());
				projectile.ai[1]++;
				int target = (int)projectile.ai[0];
				if (target >= 0)
                {
					NPC npc = Main.npc[target];
					if(npc.CanBeChasedBy())
                    {
						Vector2 toNPC = npc.Center - projectile.Center;
						projectile.velocity = Vector2.Lerp(projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * 3.75f, 0.001f + counter / 4000f);
						counter++;
					}
                }
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 600, false);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 0;
			triggerUpdate();
		}
		public void triggerUpdate()
		{
			hasHit = true;
			projectile.velocity *= 0;
			projectile.friendly = false;
			if (projectile.owner == Main.myPlayer)
			{
				projectile.netUpdate = true;
				//Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<VibrantRing>(), projectile.damage, projectile.knockBack * 0.1f, Main.myPlayer);
			}
		}
	}
}