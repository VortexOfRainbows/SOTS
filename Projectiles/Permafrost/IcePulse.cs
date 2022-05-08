using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Permafrost
{    
    public class IcePulse : ModProjectile 
    {
		bool runOnce = true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Pulse");
		}
        public override void SetDefaults()
        {
			projectile.height = 40;
			projectile.width = 40;
            Main.projFrames[projectile.type] = 4;
			projectile.penetrate = -1;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.timeLeft = 16;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = alt ? Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/IcePulseAlt").Value : Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), lightColor, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		bool alt = false;
        public override bool PreAI()
		{
			if (projectile.ai[0] == -1)
				alt = true;
			return base.PreAI();	
        }
        public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f);
			if (runOnce)
			{
				SoundEngine.PlaySound(SoundID.Item50, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				runOnce = false;
				for (int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(-10, 0).RotatedBy(MathHelper.ToRadians(i));
					int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, alt ? ModContent.DustType<ModIceDust>() : ModContent.DustType<ModSnowDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale = 1.45f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
				}
			}
			projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 4;
            }
        }
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			hitbox = new Rectangle((int)(projectile.position.X - projectile.width/2), (int)(projectile.position.Y - projectile.height/2), projectile.width * 2, projectile.height * 2);
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
            target.immune[projectile.owner] = 10;
			if(Main.rand.NextBool(10))
				target.AddBuff(BuffID.Frostburn, 300, false);
        }
	}
}