using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
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
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.extraUpdates = 4;
			Projectile.timeLeft = 900;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
		}
		public const int trailLength = 34;
        Vector2[] trailPos = new Vector2[34];
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
		public void TrailPreDraw()
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Chaos/SupernovaLaser");
			Vector2 drawOrigin = new Vector2(0, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailLength; k++)
			{
				float scale = Projectile.scale * 0.8f;
				float scaleMultY = 0.1f + 0.9f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(Projectile.ai[1] + k * 2), VoidPlayer.ChaosPink);
				color.A = 0;
				color = color * (float)Math.Sqrt(((trailPos.Length - k) / (float)trailPos.Length));
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float max = betweenPositions.Length() / texture.Width;
				if (trailPos[k] != Projectile.Center)
					for (int i = -1; i <= 1; i++)
					{
						Vector2 offset = new Vector2(0, 1 * i).RotatedBy(betweenPositions.ToRotation());
						Main.spriteBatch.Draw(texture, currentPos - Main.screenPosition + offset, null, color * 0.9f, betweenPositions.ToRotation(), drawOrigin, new Vector2(max, scale * scaleMultY), SpriteEffects.None, 0f);
					}
				previousPosition = currentPos;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			TrailPreDraw();
			if(!hasHit)
			{
				Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(VoidPlayer.soulColorCounter * 6 + Projectile.whoAmI * 18));
				color.A = 0;
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				for (int k = 0; k < 4; k++)
				{
					Vector2 offset = new Vector2(2, 0).RotatedBy(MathHelper.PiOver2 * k + Projectile.velocity.ToRotation());
					Main.spriteBatch.Draw(texture, offset + Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, color * 0.7f, Projectile.rotation + MathHelper.PiOver2, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
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
			Player player = Main.player[Projectile.owner];
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f, 0.5f);
				if(timeLeftSetter == 900)
				{
					if (Projectile.owner == Main.myPlayer)
					{
						timeLeftSetter = Main.rand.Next(721, 820);
						Projectile.netUpdate = true;
					}
				}
			}
			if (Projectile.owner == Main.myPlayer)
            {
				if (player.heldProj < 0 && !player.channel)
				{
					hasReleased = true;
				}
				cursorPos = Main.MouseWorld;
				Projectile.netUpdate = true;
			}
			cataloguePos();

			if(hasReleased)
			{
				if (hasHit && Main.rand.NextBool(4))
				{
					int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
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
					if (Projectile.timeLeft > trailLength)
						Projectile.timeLeft = trailLength;
					Projectile.friendly = false;
				}
				else
				{
					float sinMult = (Projectile.timeLeft - 600) / 300f;
					sinMult = MathHelper.Clamp(sinMult, 0, 1);
					float sin = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] * 1.0f)) * 0.016f * sinMult;
					Projectile.velocity += new Vector2(0, sin).RotatedBy(Projectile.velocity.ToRotation());
					Projectile.ai[1]++;
					if(Projectile.timeLeft < 670)
                    {
						Projectile.velocity *= 0.9935f;
                    }
					if(Projectile.timeLeft < 600)
					{
						Projectile.friendly = true;
						Vector2 targetPos = cursorPos;
						int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 640);
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
						Vector2 toCursorPos = targetPos - Projectile.Center;
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, toCursorPos.SafeNormalize(Vector2.Zero) * (Projectile.velocity.Length() + 2), 0.023f);
						if(Projectile.velocity.Length() < 20)
							Projectile.velocity *= 1.01f;
						if (targetPos == cursorPos && toCursorPos.Length() < 60 + (timeLeftSetter - 720) * 1.5f)
						{
							triggerUpdate();
						}
					}
					if (Projectile.timeLeft <= trailLength)
						triggerUpdate();
				}
			}
			else
            {
				Projectile.timeLeft = 900;
				Projectile.friendly = false;
				if (cursorPos != Vector2.Zero)
                {
					Vector2 velo = cursorPos - player.Center;
					velo = velo.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
					Projectile.velocity = velo;
				}
				Vector2 designatedPosition = player.Center;
				Vector2 offset = Projectile.velocity.SafeNormalize(Vector2.Zero) * -Projectile.ai[0];
				Vector2 normalSin = new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(Projectile.ai[1] + SOTSPlayer.ModPlayer(player).orbitalCounter));
				Vector2 sinusoid = normalSin * (32 + 0.25f * Projectile.ai[0]);
				sinusoid.X *= 0.2f;
				sinusoid = sinusoid.RotatedBy(Projectile.velocity.ToRotation());
				Projectile.Center = designatedPosition + offset + sinusoid;
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(normalSin.Y * 32.5f * (float)Math.Sin(MathHelper.ToRadians(SOTSPlayer.ModPlayer(player).orbitalCounter + (Projectile.ai[0] - 60) * 3)))); //add some weird visual twistyness
				if(runOnce)
					for (int i = 0; i < 4; i++)
					{
						int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = VoidPlayer.ChaosPink;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale = 1.5f;
						dust.velocity *= 1.5f;
					}
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.timeLeft < 60 && !hasHit) 
				triggerUpdate();
			runOnce = false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 0;
			triggerUpdate();
		}
		public void triggerUpdate()
		{
			hasHit = true;
			Projectile.friendly = false;
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<ChaosBloomExplosion>(), Projectile.damage, Projectile.knockBack * 0.5f, Main.myPlayer, Main.rand.NextFloat(-390, -30), Main.rand.NextFloat(-390, -30));
			}
			Projectile.velocity *= 0;
		}
        public override bool ShouldUpdatePosition()
        {
			return hasReleased;
        }
    }
}