using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth.Glowmoth
{    
    public class GlowBombOrb : ModProjectile 
    {
		public const int TrailLength = 20;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = TrailLength;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 900;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int width = 40;
			float scaleMult = Projectile.ai[1] / 5f + 0.5f;
			if (Projectile.ai[0] < 0)
			{
				width = (int)(width * scaleMult);
			}
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
			base.ModifyDamageHitbox(ref hitbox);
		}
		public void DrawTrail()
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float scaleMult = Projectile.ai[1] / 5f + 0.5f;
			float alphaMult = 1.2f;
			for (int k = 0; k < TrailLength; k++)
			{
				float scale = (TrailLength - k) / (float)TrailLength;
				Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition;
				Color color = Projectile.GetAlpha(new Color(100, 100, 110, 0)) * scale;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.3f * alphaMult, Projectile.rotation, drawOrigin, Projectile.scale * scaleMult * scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 100, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float scaleMult = Projectile.ai[1] / 5f + 0.5f;
			DrawTrail();
			float alphaMult = 1f;
			for (int k = 0; k < 2; k++)
			{
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color) * alphaMult, Projectile.rotation, drawOrigin, scaleMult, SpriteEffects.None, 0f);
			}
			return false;
		}
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item98, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f, -0.4f);
				Projectile.scale = 0.05f;
				Projectile.alpha = 0;
				runOnce = false;
			}
			else if (Projectile.scale < 1f)
				Projectile.scale += 0.05f;
			else 
				Projectile.scale = 1f;
			Projectile.rotation += Projectile.velocity.Length() * 0.05f;
			Projectile.velocity.Y += 0.1f;
			if(Projectile.Center.Y > Projectile.ai[0] && Projectile.timeLeft < 870)
            {
				Projectile.tileCollide = true;
            }
		}
        public override bool ShouldUpdatePosition()
        {
			return true;
        }
        public override void Kill(int timeLeft)
		{
			if(Main.netMode != NetmodeID.MultiplayerClient && Projectile.ai[1] > 0)
			{
				int direction = Math.Sign(Projectile.velocity.X);
				int start = direction;
				int end = direction;
				if (Projectile.velocity.X == 0)
                {
					start = -1;
					end = 1;
                }
				for (int i = start; i <= end; i += 2)
				{
					Vector2 shootUp = new Vector2(2 * i, -Projectile.ai[1] * 1.2f - 4);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootUp, ModContent.ProjectileType<GlowBombOrb>(), Projectile.damage, 1f, Main.myPlayer, Projectile.position.Y, Projectile.ai[1] - 1);
				}
			}
			DustOut();
		}
		public void DustOut()
		{
			for (int i = 0; i < 360; i += 40)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(5), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.7f;
				dust.velocity += Projectile.velocity * 0.7f;
				dust.color = ColorHelpers.VibrantColorAttempt(Main.rand.NextFloat(360));
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.5f;
			}
		}
	}
}