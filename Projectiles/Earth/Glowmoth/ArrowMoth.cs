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
using Terraria.GameContent;

namespace SOTS.Projectiles.Earth.Glowmoth
{    
    public class ArrowMoth : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 3;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			Projectile.arrow = true;
			Projectile.friendly = false;
			Projectile.alpha = 100;
			Projectile.width = 24;
			Projectile.height = 20;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 150;
		}
		int firstBounce = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if(firstBounce <= 1)
			{
				if (Projectile.velocity.X != oldVelocity.X)
					Projectile.velocity.X = -oldVelocity.X;
				if (Projectile.velocity.Y != oldVelocity.Y)
					Projectile.velocity.Y = -oldVelocity.Y;
				firstBounce++;
				return false;
            }
			return true;
        }
        public override void AI()
		{
			if (Projectile.timeLeft == 75 || Projectile.timeLeft == 50 || Projectile.timeLeft == 25)
				SummonArrow();
			Projectile.ai[0] += 0.5f;
			Visuals();
			int target = Common.GlobalNPCs.SOTSNPCs.FindTarget_Basic(Projectile.Center, 300f, Projectile);
			if (target != -1)
			{
				NPC npc = Main.npc[target];
				Vector2 destination = npc.Center + new Vector2(0, -npc.height * 0.6f - 32);
				float distance = (destination - Projectile.Center).Length();
				if (distance > 8)
				{
					var normal = (destination - Projectile.Center).SafeNormalize(Vector2.Zero);
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, normal * (Projectile.velocity.Length() * 1.014f + 1.6f), 0.0925f);
				}
				else if (distance < 28)
				{
					Projectile.velocity *= 1.004f;
				}
				else if (distance < 56)
				{
					Projectile.velocity *= 0.995f;
				}
			}
		}
        public override void Kill(int timeLeft)
        {
			SummonArrow();
        }
        public void SummonArrow()
        {
			if(Main.myPlayer == Projectile.owner)
            {
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Projectile.velocity.X * 0.825f, 7.5f), (int)Projectile.ai[1], Projectile.damage, Projectile.knockBack, Main.myPlayer);
            }
			for(int i = 0; i < 13; i++)
            {
				Color color2 = ColorHelpers.VibrantColorAttempt(180 + Projectile.ai[0] % 180, true);
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 0.55f * dust.scale + 0.6f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 2f;
			}
			for (int i = 0; i < 6; i++)
			{
				Color color2 = ColorHelpers.VibrantColorAttempt(180 + Projectile.ai[0] % 180, true);
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.color = Color.Lerp(Color.White, color2, 0.3f);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 0.55f * dust.scale + 0.6f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 1.25f;
				dust.velocity += new Vector2(0, 2);
			}
		}
		int graduallyBringInTrail = 0;
		float frameCounter = 0;
		private void Visuals()
		{
			Projectile.rotation = Projectile.velocity.X * 0.05f;
			frameCounter += 1 + Projectile.identity % 10 * 0.1f;
			SpinCounter += Projectile.velocity.Length() * Math.Sign(Projectile.velocity.X);
			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 5;
			if (frameCounter >= frameSpeed)
			{
				frameCounter -= 5;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
			if (Main.rand.NextBool(4))
			{
				Color color2 = ColorHelpers.VibrantColorAttempt(180 + Projectile.ai[0] % 180, true);
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 0.35f * dust.scale + 0.5f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.4f;
				dust.velocity -= new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
			}
			if (graduallyBringInTrail < Projectile.oldPos.Length)
				graduallyBringInTrail++;
			Lighting.AddLight(Projectile.Center, new Color(172, 173, 252).ToVector3() * 0.225f);
		}
		public float SpinCounter = 0; //since this variable is only used for visuals, it doesn't need to be synced in multiplayer
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D texture2 = ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/Glowmoth/ArrowMothTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 trailOrigin = new Vector2(texture2.Width - 6, texture2.Height * 0.5f);
			Rectangle yFrame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			float scaleOffSpeed = MathHelper.Clamp(Projectile.velocity.Length() / 15f, 0, 1);
			for (int i = 0; i < 4; i++)
			{
				Vector2 circular = new Vector2(2 + 12 * scaleOffSpeed * scaleOffSpeed * Projectile.timeLeft / 150f, 0).RotatedBy(MathHelper.ToRadians(i * 90 + SpinCounter * 0.3f));
				Color color = new Color(80, 85, 100, 0) * (1.0f - scaleOffSpeed * 0.6f);
				Main.EntitySpriteDraw(texture, Projectile.Center + circular - Main.screenPosition, yFrame, color, Projectile.rotation, drawOrigin, Projectile.scale * (0.7f + 0.3f * scaleOffSpeed), Projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			}
			for (int k = 1; k < graduallyBringInTrail - 1; k++)
			{
				for (int j = 0; j < 3; j++)
				{
					float scaleMult = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * (1.0f - j * 0.2f);
					Vector2 toNextPosition = Projectile.oldPos[k] - Projectile.oldPos[k + 1];
					Vector2 drawPos = Projectile.oldPos[k] + drawOrigin - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
					Color color = new Color(60, 60, 70, 0) * scaleMult * (0.5f + scaleOffSpeed * 0.5f);
					Main.EntitySpriteDraw(texture2, drawPos + toNextPosition.SafeNormalize(Vector2.Zero) * 3 * j, null, color, toNextPosition.ToRotation(), trailOrigin, new Vector2(toNextPosition.Length() / texture2.Width * 2, Projectile.scale * (1.5f - 0.5f * scaleOffSpeed) * scaleMult), SpriteEffects.None, 0);
				}
			}
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, yFrame, Color.White, Projectile.rotation, drawOrigin, Projectile.scale * 0.95f, Projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			return false;
		}
	}
}
		
			