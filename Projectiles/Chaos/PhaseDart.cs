using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{    
    public class PhaseDart : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Dart");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 360;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 24;
			hitbox = new Rectangle((int)Projectile.Center.X - width/2, (int)Projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
		}
		public void DrawTelegraph()
		{
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Chaos/DartTelegraph");
			Vector2 from = Projectile.Center;
			for (int i = 1; i < 10; i++)
			{
				Color color2 = VoidPlayer.ChaosPink;
				color2.A = 0;
				float alphaMult = (counter / 90f);
				Vector2 to = Projectile.Center + Projectile.velocity * i * 10f * alphaMult;
				Vector2 toPos = from - to;
				int length = (int)toPos.Length() + 1;
				Main.spriteBatch.Draw(texture2, from - Main.screenPosition, new Rectangle(0, 0, 2, length), color2 * alphaMult * (1 - ((float)(i - 1) / 10f)), Projectile.velocity.ToRotation() - MathHelper.Pi / 2, new Vector2(1, 1), 1f, SpriteEffects.None, 0f);
				from = to;
			}
		}
		public void DrawTrail()
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color2 = VoidPlayer.ChaosPink;
				color2.A = 0;
				float scale = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(12, 12);
				Color color = Projectile.GetAlpha(color2) * scale;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale * scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			DrawTelegraph();
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = VoidPlayer.ChaosPink;
			color.A = 0;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			DrawTrail();
			for (int k = 0; k < 5; k++)
			{
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, Projectile.GetAlpha(color), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		bool runOnce = true;
		int counter = 0;
		public override void AI()
		{
			if(counter < 90)
				counter++;
			if (runOnce)
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, SoundLoader.GetSoundSlot(Mod, "Sounds/Items/StarLaser"), 0.6f, 0.2f + Main.rand.NextFloat(-0.1f, 0.1f));
				DustOut();
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
			Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.015f;
		}
        public override void Kill(int timeLeft)
		{
			DustOut();
		}
		public void DustOut()
        {
			for (int i = 0; i < 360; i += 45)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += Projectile.velocity * 0.9f;
				dust.color = VoidPlayer.ChaosPink;
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
			}
		}
	}
}