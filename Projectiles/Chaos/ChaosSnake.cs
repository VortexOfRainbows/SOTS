using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			DisplayName.SetDefault("Chaos Snake");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 current = projectile.Center;
			bool lowfidel = SOTS.Config.lowFidelityMode;
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(projectile.ai[0] - k), true);
				color.A = 0;
				float yScale = (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length;
				color = projectile.GetAlpha(color) * yScale * 0.5f;
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Vector2 dist = projectile.oldPos[k] + drawOrigin - current;
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
				current = projectile.oldPos[k] + drawOrigin;
			}
			return false;
		}
		public override void SetDefaults()
        {
			projectile.height = 24;
			projectile.width = 24;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 1500;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.extraUpdates = 1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 80;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			bounceCount++;
			if(bounceCount > 5)
			{
				end = true;
				projectile.velocity *= 0;
			}
			else
			{
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
        }
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i <10; i++)
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num2];
				dust.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(projectile.ai[0]), true);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 16;
			height = 16;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
		bool runOnce = true;
        public override void AI()
		{
			if (end)
			{
				projectile.velocity *= 0;
				if (projectile.friendly)
				{
					projectile.friendly = false;
					projectile.netUpdate = true;
				}
				if (projectile.timeLeft > 60)
					projectile.timeLeft = 60;
			}
			if (projectile.timeLeft < 60 && !end)
				end = true;
			if (runOnce)
			{
				for (int i = 0; i < 10; i++)
				{
					int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
					Dust dust = Main.dust[num2];
					dust.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(projectile.ai[0]), true);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 2f;
					dust.velocity += projectile.velocity * 0.5f;
				}
				runOnce = false;
			}
			if((end && !Main.rand.NextBool(3)) || Main.rand.NextBool(10))
            {
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
				dust.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(projectile.ai[0]), true);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				int alpha = 255 - (int)(255 * (projectile.timeLeft / 60f));
				dust.alpha = (int)MathHelper.Clamp(alpha, 0, 255);
				return;
			}
			int target2 = SOTSNPCs.FindTarget_Basic(projectile.Center, 900, this, true);
			if (target2 != -1)
			{
				NPC toHit = Main.npc[target2];
				if (toHit.active)
				{
					Vector2 toNPC = toHit.Center - projectile.Center;
					projectile.velocity = Vector2.Lerp(projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * (projectile.velocity.Length() + 5), 0.11f);
				}
			}
			projectile.ai[0]++;
			projectile.velocity += new Vector2(0, (float)Math.Sin(MathHelper.ToRadians(projectile.ai[0] * 12)) * 0.4f).RotatedBy(projectile.velocity.ToRotation());
			if (projectile.velocity.Length() < 11)
				projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * 11f;
		}
	}
}
		