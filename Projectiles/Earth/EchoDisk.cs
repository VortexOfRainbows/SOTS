using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Earth
{    
    public class EchoDisk : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Echo Disc");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Main.projFrames[Projectile.type] = 2;
		}
        public override void SetDefaults()
        {
			Projectile.height = 26;
			Projectile.width = 26;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 720;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.extraUpdates = 2;
			Projectile.alpha = 0;
		}
		public void DustRing()
		{
			for (int i = 0; i < 360; i += 10)
			{
				Vector2 circularLocation = new Vector2(-9, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				dust.color = color;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.75f;
				dust.alpha = 70;
				dust.velocity *= 0.1f;
				dust.velocity += circularLocation * 0.25f;
			}
		}
		public override void Kill(int timeLeft)
		{
			color = new Color(80, 120, 220, 0);
			DustRing();
		}
		Color color = new Color(180, 230, 100, 0);
		public override void AI()
		{
			Projectile.rotation += 0.47f;
			if(Projectile.ai[0] == 1)
			{
				DustRing();
				Projectile.timeLeft = 90;
				Projectile.frame = 0;
				Projectile.ai[0] = -1;
				Projectile.alpha = 255;
				Projectile.position -= Projectile.velocity * 40;
				Projectile.penetrate = 2;
				Projectile.tileCollide = false;
				if (Main.myPlayer == Projectile.owner)
					Projectile.netUpdate = true;
			}
			if (Projectile.ai[0] != -1)
			{
				Projectile.frame = 1;
			}
			else
			{
				if(Projectile.timeLeft > 45)
				{
					Projectile.alpha -= 6;
				}
				else
				{
					Projectile.alpha += 6;
				}
				if (Projectile.timeLeft <= 52)
				{
					Projectile.tileCollide = true;
				}
			}
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B * 1.75f / 255f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Projectile.ai[0] != -1)
				Projectile.ai[0] = 1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			DrawTrail(spriteBatch, lightColor);
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			color = Projectile.GetAlpha(color);
			for (int j = 0; j < 5; j++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public void DrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.ai[0] != -1)
				Projectile.ai[0] = 1;
			Projectile.velocity = oldVelocity;
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			return Projectile.ai[0] == -1;
		}
	}
}
		