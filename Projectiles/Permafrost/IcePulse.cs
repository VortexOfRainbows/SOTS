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
			// DisplayName.SetDefault("Ice Pulse");
		}
        public override void SetDefaults()
        {
			Projectile.height = 40;
			Projectile.width = 40;
            Main.projFrames[Projectile.type] = 4;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.timeLeft = 16;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = alt ? Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/IcePulseAlt").Value : Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		bool alt = false;
        public override bool PreAI()
		{
			if (Projectile.ai[0] == -1)
				alt = true;
			return base.PreAI();	
        }
        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f);
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item50, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
				runOnce = false;
				for (int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(-10, 0).RotatedBy(MathHelper.ToRadians(i));
					int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, alt ? ModContent.DustType<ModIceDust>() : ModContent.DustType<ModSnowDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale = 1.45f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
				}
			}
			Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
        }
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			hitbox = new Rectangle((int)(Projectile.position.X - Projectile.width/2), (int)(Projectile.position.Y - Projectile.height/2), Projectile.width * 2, Projectile.height * 2);
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Player player = Main.player[Projectile.owner];
            target.immune[Projectile.owner] = 10;
			if(Main.rand.NextBool(10))
				target.AddBuff(BuffID.Frostburn, 300, false);
        }
	}
}