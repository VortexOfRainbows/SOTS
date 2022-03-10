using log4net.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Base;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
	public class ThunderBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Thunder Ball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			projectile.width = 70;
			projectile.height = 70;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 80;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.scale = 1f;
			projectile.alpha = 0;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = 28 * projectile.scale;
			hitbox = new Rectangle((int)(projectile.Center.X - width/2), (int)(projectile.Center.Y - width / 2), (int)width, (int)width);
		}
        public override bool CanHitPlayer(Player target)
        {
            return projectile.timeLeft > 14;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Chaos/ChaosCircle");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if(counter <= 40)
				for (int k = 0; k < 90; k++)
				{
					float sin = (float)Math.Sin(MathHelper.ToRadians(k * 24 + 720f * (counter / 60f) * (counter / 60f)));
					Vector2 circularPos = new Vector2((32 + sin * 8) * (1 - projectile.scale), 0).RotatedBy(MathHelper.ToRadians(k * 4) + projectile.rotation);
					Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 3));
					color.A = 0;
					Vector2 drawPos = projectile.Center + circularPos - Main.screenPosition;
					Main.spriteBatch.Draw(texture, drawPos, null, projectile.GetAlpha(color), projectile.rotation, drawOrigin, (1 - projectile.scale), SpriteEffects.None, 0f);
				}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Color color = new Color(100, 100, 100, 0);
				Vector2 circular = new Vector2(4 * projectile.scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
				if (k != 0)
				{
					color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
					color.A = 0;
				}
				else
					circular *= 0f;
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, null, projectile.GetAlpha(color) * 0.8f, 0f, drawOrigin, projectile.scale * 0.4f, SpriteEffects.None, 0f);
			}
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		float counter = 0;
		bool runOnce = true;
        public override void AI()
		{
			if(runOnce)
			{
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<ChaosHelixLaser>(), projectile.damage, projectile.knockBack, Main.myPlayer, 0, 0.2f);
				}
				runOnce = false;
				projectile.scale = 0;
				projectile.alpha = 0;
			}
			float alphaMult = counter / 40f;
			if(alphaMult > 1)
            {
				alphaMult = 1 - ((counter - 40f) / 40f);
            }
			projectile.alpha = (int)(255 * (1 - alphaMult));
			float scaleMult = counter / 40f;
			if (scaleMult > 1)
			{
				projectile.scale = 1 - ((counter - 40f) / 40f);
			}
			else
				projectile.scale = scaleMult * 0.5f + 0.5f * scaleMult * scaleMult;
			counter++; 
		}
    }
}