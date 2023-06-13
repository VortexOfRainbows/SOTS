using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth.Glowmoth
{    
    public class WaveBall : ModProjectile 
    {
		public const int TrailLength = 10;
		public float Counter = 0;
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
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 1200;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 20;
			hitbox = new Rectangle((int)Projectile.Center.X - width/2, (int)Projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
		}
		Projectile partner = null;
		public Projectile PartnerProjectile()
        {
			int ID = (int)Projectile.ai[0];
			for (int i = 0; i < 1000; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.identity == ID && proj.active && proj.type == Projectile.type)
				{
					return proj;
				}
			}
			return null;
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			if (partner != null)
			{
				float collisionPoint = 0;
				Vector2 lineStart = Projectile.Center;
				Vector2 lineEnd = partner.Center;
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineStart, lineEnd, 10f, ref collisionPoint))
					return true;
			}
			return false;
        }
        public void DrawWebBetweenProjectile(bool partnerDraw)
        {
			if(partner != null)
            {
				Vector2 start = Projectile.Center;
				Vector2 end = partner.Center;
				Texture2D texture = ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/Glowmoth/MothMinionTrail").Value; //green trail
				if(partnerDraw)
                {
					start = end;
					end = Projectile.Center;
					texture = ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/Glowmoth/ArrowMothTrail").Value; //blue trail
				}
				Vector2 toPartner = end - start;
				Vector2 origin = new Vector2(0, texture.Height / 2);
				float dist = toPartner.Length();
				float xScale = dist / texture.Width;
				float alphaMult = 0.2f + 0.8f * Projectile.ai[1];
				for (int k = 0; k <= 1; k++)
				{
					Main.spriteBatch.Draw(texture, start - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, new Color(100, 100, 100, 0) * alphaMult * 0.7f, toPartner.ToRotation(), origin, new Vector2(xScale, 0.25f + 0.75f * alphaMult), SpriteEffects.None, 0f);
				}
			}
        }
		public void DrawTrail()
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float alphaMult = 0.05f + 0.95f * Projectile.ai[1];
			for (int k = 0; k < TrailLength; k++)
			{
				Color color2 = ColorHelpers.VibrantColorAttempt(Projectile.whoAmI * 18 + k * 6 + SOTSWorld.GlobalCounter);
				color2.A = 0;
				float scale = (TrailLength - k) / (float)TrailLength;
				Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition;
				Color color = Projectile.GetAlpha(color2) * scale;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.4f * alphaMult, Projectile.rotation, drawOrigin, Projectile.scale * scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = ColorHelpers.VibrantColorAttempt(Projectile.whoAmI * 18 + SOTSWorld.GlobalCounter);
			color.A = 0;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			DrawWebBetweenProjectile(false);
			DrawWebBetweenProjectile(true);
			DrawTrail();
			float alphaMult = 0.2f + 0.8f * Projectile.ai[1];
			for (int k = 0; k < 3; k++)
			{
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color) * alphaMult, Projectile.rotation + MathHelper.ToRadians(120f * k + Counter), drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		bool runOnce = true;
		public override void AI()
		{
			partner = PartnerProjectile();
			if (runOnce)
			{
                if (Projectile.ai[1] != 0)
                {
					Counter += Projectile.ai[1];
					Projectile.ai[1] = 0;
                }
				SOTSUtils.PlaySound(SoundID.Item98, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f, -0.4f);
				Projectile.scale = 0.1f;
				Projectile.alpha = 0;
				runOnce = false;
			}
			else if (Projectile.scale < 1f)
				Projectile.scale += 0.1f;
			else 
				Projectile.scale = 1f;
			if(Projectile.timeLeft < 9)
            {
				Projectile.alpha += 25;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			float sinusoid = (float)Math.Sin(Counter / 180f * MathHelper.Pi);
			sinusoid = (float)Math.Pow(sinusoid, 12);
			Counter += 0.9f;
			Projectile.ai[1] = sinusoid;
			Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.0025f;
			Projectile.position += Projectile.velocity * (0.3f + 0.7f * Projectile.ai[1]);
		}
        public override bool CanHitPlayer(Player target)
        {
			return Projectile.ai[1] > 0.5f;
        }
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void Kill(int timeLeft)
		{
			DustOut();
		}
		public void DustOut()
		{
			for (int i = 0; i < 360; i += 60)
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