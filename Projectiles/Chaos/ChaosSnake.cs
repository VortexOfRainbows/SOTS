using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{    
    public class ChaosSnake : ModProjectile 
    {
		bool end = false;
		int bounceCount = 0;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chaos Snake");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 current = Projectile.Center;
			bool lowfidel = SOTS.Config.lowFidelityMode;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(Projectile.ai[0] - k), true);
				color.A = 0;
				float yScale = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
				color = Projectile.GetAlpha(color) * yScale * 0.5f;
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Vector2 dist = Projectile.oldPos[k] + drawOrigin - current;
				float stretchScale = dist.Length() / texture.Width;
				yScale *= 0.5f + MathHelper.Clamp(0.5f * stretchScale, 0, 0.5f);
				if(!lowfidel)
				{
					for (int j = 0; j < 4; j++)
					{
						Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(90 * j));
						Main.spriteBatch.Draw(texture, drawPos + circular, null, color, dist.ToRotation(), drawOrigin, new Vector2(stretchScale, yScale), SpriteEffects.None, 0f);
					}
				}
				else
				{
					Main.spriteBatch.Draw(texture, drawPos, null, color, dist.ToRotation(), drawOrigin, new Vector2(stretchScale, yScale), SpriteEffects.None, 0f);
				}
				current = Projectile.oldPos[k] + drawOrigin;
			}
			return false;
		}
		public override void SetDefaults()
        {
			Projectile.height = 24;
			Projectile.width = 24;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 1200;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 80;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			bounceCount++;
			if(bounceCount > 5)
			{
				end = true;
				Projectile.velocity *= 0;
			}
			else
			{
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
        }
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i <10; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num2];
				dust.color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(Projectile.ai[0]), true);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 16;
			height = 16;
            return true;
        }
		bool runOnce = true;
        public override void AI()
		{
			if (end)
			{
				Projectile.velocity *= 0;
				if (Projectile.friendly)
				{
					Projectile.friendly = false;
					Projectile.netUpdate = true;
				}
				if (Projectile.timeLeft > 60)
					Projectile.timeLeft = 60;
			}
			if (Projectile.timeLeft < 60 && !end)
				end = true;
			if (runOnce)
			{
				for (int i = 0; i < 10; i++)
				{
					int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
					Dust dust = Main.dust[num2];
					dust.color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(Projectile.ai[0]), true);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 2f;
					dust.velocity += Projectile.velocity * 0.5f;
				}
				runOnce = false;
			}
			if((end && !Main.rand.NextBool(3)) || Main.rand.NextBool(10))
            {
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				dust.color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(Projectile.ai[0]), true);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				int alpha = 255 - (int)(255 * (Projectile.timeLeft / 60f));
				dust.alpha = (int)MathHelper.Clamp(alpha, 0, 255);
				return;
			}
			if (!end)
			{
				int target2 = SOTSNPCs.FindTarget_Basic(Projectile.Center, 900, this, true);
				if (target2 != -1)
				{
					NPC toHit = Main.npc[target2];
					if (toHit.active)
					{
						Vector2 toNPC = toHit.Center - Projectile.Center;
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * (Projectile.velocity.Length() + 5), 0.11f);
					}
				}
			}
			Projectile.ai[0]++;
			if(!end)
			{
				Projectile.velocity += new Vector2(0, (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[0] * 12)) * 0.4f).RotatedBy(Projectile.velocity.ToRotation());
				if (Projectile.velocity.Length() < 11)
					Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 11f;
			}
		}
	}
}
		