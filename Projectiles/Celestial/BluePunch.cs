using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Dusts;

namespace SOTS.Projectiles.Celestial
{    
    public class BluePunch : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Fist");
		}
        public override void SetDefaults()
        {
			projectile.aiStyle = 0;
			projectile.melee = true;
			projectile.friendly = true;
			projectile.width = 56;
			projectile.height = 30;
			projectile.timeLeft = 96;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.alpha = 20;
            Main.projFrames[projectile.type] = 5;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 5;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, projectile.height / 2);
			Color color = Color.Black;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(100, 100, 255, 0);
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(100, 100, 200);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void AI()
		{ 
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1f / 255f, (255 - projectile.alpha) * 1f / 255f, (255 - projectile.alpha) * 2.4f / 255f);
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			projectile.velocity *= 0.99f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 5;
            }
			projectile.alpha += 2;	
			
			int rotation = 30 * (int)projectile.ai[1];
			
			if(projectile.timeLeft < 101 && projectile.timeLeft > 27)
			{
				Vector2 circularLocation = new Vector2(projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f).RotatedBy(MathHelper.ToRadians(rotation));
				
				Player player  = Main.player[projectile.owner];
				projectile.velocity *= 0.845f;
				projectile.velocity += circularLocation;
			}
			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.2f;
				dust.velocity -= 2 * projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.scale *= 2;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.color = new Color(150, 100, 200, 0);
				dust.alpha = 100;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 32; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 1.2f;
				dust.velocity += 5 * projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.scale *= 2;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.color = new Color(100, 100, 255, 0);
				dust.alpha = 100;
			}
			base.Kill(timeLeft);
		}
	}
}
		