using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
	public class ChaosThorn : ModProjectile
	{
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(hasReleased);
			writer.Write(hasHit);
			writer.Write(cursorPos.X);
			writer.Write(cursorPos.Y);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			hasReleased = reader.ReadBoolean();
			hasHit = reader.ReadBoolean();
			cursorPos.X = reader.ReadSingle();
			cursorPos.Y = reader.ReadSingle();
		}
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Thorn");
		} 
		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.extraUpdates = 4;
			projectile.timeLeft = 900;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.penetrate = -1;
		}
		public const int trailLength = 34;
        Vector2[] trailPos = new Vector2[34];
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
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Chaos/SupernovaLaser");
			Vector2 drawOrigin = new Vector2(0, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailLength; k++)
			{
				float scale = projectile.scale * 0.8f;
				float scaleMultY = 0.1f + 0.9f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(projectile.ai[1] + k * 2), VoidPlayer.ChaosPink);
				color.A = 0;
				color = color * (float)Math.Sqrt(((trailPos.Length - k) / (float)trailPos.Length));
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float max = betweenPositions.Length() / texture.Width;
				if (trailPos[k] != projectile.Center)
					for (int i = -1; i <= 1; i++)
					{
						Vector2 offset = new Vector2(0, 1 * i).RotatedBy(betweenPositions.ToRotation());
						spriteBatch.Draw(texture, currentPos - Main.screenPosition + offset, null, color * 0.9f, betweenPositions.ToRotation(), drawOrigin, new Vector2(max, scale * scaleMultY), SpriteEffects.None, 0f);
					}
				previousPosition = currentPos;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			TrailPreDraw(spriteBatch);
			if(!hasHit)
			{
				Texture2D texture = Main.projectileTexture[projectile.type];
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(VoidPlayer.soulColorCounter * 6 + projectile.whoAmI * 18));
				color.A = 0;
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				for (int k = 0; k < 4; k++)
				{
					Vector2 offset = new Vector2(2, 0).RotatedBy(MathHelper.PiOver2 * k + projectile.velocity.ToRotation());
					Main.spriteBatch.Draw(texture, offset + projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, color * 0.7f, projectile.rotation + MathHelper.PiOver2, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
		Vector2 cursorPos = Vector2.Zero;
		bool hasHit = false;
		bool runOnce = true;
		bool hasReleased = false;
		int counter = 0;
		int timeLeftSetter = 900;
		public override void AI()
		{
			counter++;
			Player player = Main.player[projectile.owner];
			if (runOnce)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 30, 0.6f, 0.5f);
				if(timeLeftSetter == 900)
				{
					if (projectile.owner == Main.myPlayer)
					{
						timeLeftSetter = Main.rand.Next(721, 820);
						projectile.netUpdate = true;
					}
				}
			}
			if (projectile.owner == Main.myPlayer)
            {
				if (player.heldProj < 0 && !player.channel)
				{
					hasReleased = true;
				}
				cursorPos = Main.MouseWorld;
				projectile.netUpdate = true;
			}
			cataloguePos();

			if(hasReleased)
			{
				if (hasHit && Main.rand.NextBool(4))
				{
					int dust2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
					Dust dust = Main.dust[dust2];
					dust.color = VoidPlayer.ChaosPink;
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale = 1.5f;
					dust.velocity *= 2.0f;
					dust.alpha = 125;
				}
				if (hasHit)
				{
					if (projectile.timeLeft > trailLength)
						projectile.timeLeft = trailLength;
					projectile.friendly = false;
				}
				else
				{
					float sinMult = (projectile.timeLeft - 600) / 300f;
					sinMult = MathHelper.Clamp(sinMult, 0, 1);
					float sin = (float)Math.Sin(MathHelper.ToRadians(projectile.ai[1] * 1.0f)) * 0.016f * sinMult;
					projectile.velocity += new Vector2(0, sin).RotatedBy(projectile.velocity.ToRotation());
					projectile.ai[1]++;
					if(projectile.timeLeft < 670)
                    {
						projectile.velocity *= 0.9935f;
                    }
					if(projectile.timeLeft < 600)
					{
						projectile.friendly = true;
						Vector2 targetPos = cursorPos;
						int target = SOTSNPCs.FindTarget_Basic(projectile.Center, 640);
						if (target >= 0)
						{
							NPC npc = Main.npc[target];
							if (npc.CanBeChasedBy())
							{
								targetPos = npc.Center;
							}
							else
								triggerUpdate();
						}
						Vector2 toCursorPos = targetPos - projectile.Center;
						projectile.velocity = Vector2.Lerp(projectile.velocity, toCursorPos.SafeNormalize(Vector2.Zero) * (projectile.velocity.Length() + 2), 0.023f);
						if(projectile.velocity.Length() < 20)
							projectile.velocity *= 1.01f;
						if (targetPos == cursorPos && toCursorPos.Length() < 60 + (timeLeftSetter - 720) * 1.5f)
						{
							triggerUpdate();
						}
					}
					if (projectile.timeLeft <= trailLength)
						triggerUpdate();
				}
			}
			else
            {
				projectile.timeLeft = 900;
				projectile.friendly = false;
				if (cursorPos != Vector2.Zero)
                {
					Vector2 velo = cursorPos - player.Center;
					velo = velo.SafeNormalize(Vector2.Zero) * projectile.velocity.Length();
					projectile.velocity = velo;
				}
				Vector2 designatedPosition = player.Center;
				Vector2 offset = projectile.velocity.SafeNormalize(Vector2.Zero) * -projectile.ai[0];
				Vector2 normalSin = new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(projectile.ai[1] + SOTSPlayer.ModPlayer(player).orbitalCounter));
				Vector2 sinusoid = normalSin * (32 + 0.25f * projectile.ai[0]);
				sinusoid.X *= 0.2f;
				sinusoid = sinusoid.RotatedBy(projectile.velocity.ToRotation());
				projectile.Center = designatedPosition + offset + sinusoid;
				projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(normalSin.Y * 32.5f * (float)Math.Sin(MathHelper.ToRadians(SOTSPlayer.ModPlayer(player).orbitalCounter + (projectile.ai[0] - 60) * 3)))); //add some weird visual twistyness
				if(runOnce)
					for (int i = 0; i < 4; i++)
					{
						int dust2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = VoidPlayer.ChaosPink;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale = 1.5f;
						dust.velocity *= 1.5f;
					}
			}
			projectile.rotation = projectile.velocity.ToRotation();
			if (projectile.timeLeft < 60 && !hasHit) 
				triggerUpdate();
			runOnce = false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 0;
			triggerUpdate();
		}
		public void triggerUpdate()
		{
			hasHit = true;
			projectile.friendly = false;
			if (projectile.owner == Main.myPlayer)
			{
				projectile.netUpdate = true;
				Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<ChaosBloomExplosion>(), projectile.damage, projectile.knockBack * 0.5f, Main.myPlayer, Main.rand.NextFloat(-390, -30), Main.rand.NextFloat(-390, -30));
			}
			projectile.velocity *= 0;
		}
        public override bool ShouldUpdatePosition()
        {
			return hasReleased;
        }
    }
}