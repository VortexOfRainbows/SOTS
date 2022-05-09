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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = 70;
			Projectile.height = 70;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 80;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.alpha = 0;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = 28 * Projectile.scale;
			hitbox = new Rectangle((int)(Projectile.Center.X - width/2), (int)(Projectile.Center.Y - width / 2), (int)width, (int)width);
		}
        public override bool CanHitPlayer(Player target)
        {
            return Projectile.timeLeft > 14;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Chaos/ChaosCircle").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if(counter <= 40)
				for (int k = 0; k < 90; k++)
				{
					float sin = (float)Math.Sin(MathHelper.ToRadians(k * 24 + 720f * (counter / 60f) * (counter / 60f)));
					Vector2 circularPos = new Vector2((32 + sin * 8) * (1 - Projectile.scale), 0).RotatedBy(MathHelper.ToRadians(k * 4) + Projectile.rotation);
					Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 3));
					color.A = 0;
					Vector2 drawPos = Projectile.Center + circularPos - Main.screenPosition;
					Main.spriteBatch.Draw(texture, drawPos, null, Projectile.GetAlpha(color), Projectile.rotation, drawOrigin, (1 - Projectile.scale), SpriteEffects.None, 0f);
				}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Color color = new Color(100, 100, 100, 0);
				Vector2 circular = new Vector2(4 * Projectile.scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
				if (k != 0)
				{
					color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
					color.A = 0;
				}
				else
					circular *= 0f;
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, null, Projectile.GetAlpha(color) * 0.8f, 0f, drawOrigin, Projectile.scale * 0.4f, SpriteEffects.None, 0f);
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
					Projectile.NewProjectile(Projectile.Center, Projectile.velocity, ModContent.ProjectileType<ChaosHelixLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 0.2f);
				}
				runOnce = false;
				Projectile.scale = 0;
				Projectile.alpha = 0;
			}
			float alphaMult = counter / 40f;
			if(alphaMult > 1)
            {
				alphaMult = 1 - ((counter - 40f) / 40f);
            }
			Projectile.alpha = (int)(255 * (1 - alphaMult));
			float scaleMult = counter / 40f;
			if (scaleMult > 1)
			{
				Projectile.scale = 1 - ((counter - 40f) / 40f);
			}
			else
				Projectile.scale = scaleMult * 0.5f + 0.5f * scaleMult * scaleMult;
			counter++; 
		}
    }
}