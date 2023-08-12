using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Celestial
{    
    public class DeathBlade : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Death's Touch");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;    
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.width = 66;
			Projectile.height = 46;
			Projectile.timeLeft = 240;
			Projectile.penetrate = 5;
			Projectile.alpha = 100;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			Projectile.velocity *= 0.25f;
			Projectile.netUpdate = true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, Projectile.height / 2);
			DrawTrail(Main.spriteBatch, lightColor);
			Color color;
			for (int i = 0; i < 360; i += 45)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(2f, 3f), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(100, 255, 100, 0);
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(0, 0, 0);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public void DrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(new Color(33, 100, 33)) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
		}
		public override void AI()
		{ 
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 0.9f / 255f, (255 - Projectile.alpha) * 0.3f / 255f);
			Projectile.rotation += 0.3f;
			if(player.whoAmI == Main.myPlayer)
			{
				Projectile.netUpdate = true;
				Vector2 cursorArea = Main.MouseWorld;
				if(Projectile.timeLeft > 192 && Projectile.timeLeft < 224 && Projectile.penetrate >= 5)
				{
					float dX = cursorArea.X - Projectile.Center.X;
					float dY = cursorArea.Y - Projectile.Center.Y;
					float distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					float speed = 1f / distance;
					Projectile.velocity *= 0.95f;
					Projectile.velocity += new Vector2(dX * speed, dY * speed);
				}
			}
			if(Projectile.timeLeft <= 30)
			{
				Projectile.alpha += 7;
				Projectile.velocity *= 0.915f;
				if (Projectile.alpha > 255)
					Projectile.Kill();
			}
			
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 360; i += 10)
			{
				Vector2 circularLocation = new Vector2(-15, 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 107);
				dust.noGravity = true;
				dust.velocity *= 0.1f;
				dust.velocity += circularLocation * 0.3f;
				dust.scale *= 1.25f;
			}
			for (int i = 0; i < 360; i += 10)
			{
				Vector2 circularLocation = new Vector2(-15, 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 0.5f;
				dust.velocity += circularLocation * 0.15f;
				dust.scale *= 2.5f;
				dust.fadeIn = 0.1f;
				dust.color = new Color(33, 100, 33);
			}
		}
	}
}
		