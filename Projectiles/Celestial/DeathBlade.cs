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
			DisplayName.SetDefault("Death's Touch");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;    
		}
        public override void SetDefaults()
        {
			projectile.magic = true;
			projectile.friendly = true;
			projectile.width = 66;
			projectile.height = 46;
			projectile.timeLeft = 240;
			projectile.penetrate = 5;
			projectile.alpha = 100;
			projectile.tileCollide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 15;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 0;
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			projectile.velocity *= 0.25f;
			projectile.netUpdate = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, projectile.height / 2);
			DrawTrail(spriteBatch, lightColor);
			Color color = Color.Black;
			for (int i = 0; i < 360; i += 35)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(2f, 3f), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(100, 255, 100, 0);
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(0, 0, 0);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public void DrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(new Color(33, 100, 33)) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
		}
		public override void AI()
		{ 
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
			projectile.rotation += 0.3f;
			if(player.whoAmI == Main.myPlayer)
			{
				projectile.netUpdate = true;
				Vector2 cursorArea = Main.MouseWorld;
				if(projectile.timeLeft > 192 && projectile.timeLeft < 224 && projectile.penetrate >= 5)
				{
					float dX = cursorArea.X - projectile.Center.X;
					float dY = cursorArea.Y - projectile.Center.Y;
					float distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					float speed = 1f / distance;
					projectile.velocity *= 0.95f;
					projectile.velocity += new Vector2(dX * speed, dY * speed);
				}
			}
			if(projectile.timeLeft <= 30)
			{
				projectile.alpha += 7;
				projectile.velocity *= 0.915f;
				if (projectile.alpha > 255)
					projectile.Kill();
			}
			
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 360; i += 10)
			{
				Vector2 circularLocation = new Vector2(-15, 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 107);
				dust.noGravity = true;
				dust.velocity *= 0.1f;
				dust.velocity += circularLocation * 0.3f;
				dust.scale *= 1.25f;
			}
			for (int i = 0; i < 360; i += 10)
			{
				Vector2 circularLocation = new Vector2(-15, 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
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
		