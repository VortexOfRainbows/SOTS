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

namespace SOTS.Projectiles.Tide
{    
    public class PhantarayBall : ModProjectile 
    {	       
        public override void SetDefaults()
        {
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.netImportant = true;
			Projectile.alpha = 255;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.X -= 8;
            hitbox.Y -= 8;
            hitbox.Width += 16;
            hitbox.Height += 16;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = texture.Size() / 2;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            for (int i = 0; i < 6; i++)
            {
                Vector2 circular = new Vector2(6, 0).RotatedBy(MathHelper.TwoPi * i / 6f + SOTSWorld.GlobalCounter * MathHelper.TwoPi / 120f);
                Main.spriteBatch.Draw(texture, drawPos + circular, null, new Color(200, 100, 100, 0), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, drawPos, null, new Color(200, 150, 160, 0), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            DustExplosion();
		}
        public void DustExplosion()
        {
            for (int i = 0; i < 360; i += 20)
            {
                Vector2 circularLocation = new Vector2(7, 0).RotatedBy(MathHelper.ToRadians(i));
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(10, 10), 10, 10, ModContent.DustType<CopyDust4>());
                dust.scale *= 0.1f;
                dust.scale += 1.5f;
                dust.color = new Color(186, 92, 92);
                dust.alpha = 50;
                dust.noGravity = true;
                dust.fadeIn = 0.5f;
                dust.velocity *= 0.1f;
                dust.velocity += circularLocation * 0.35f;
            }
        }
		bool runOnce = true;
		public override void AI()
		{
            if (runOnce)
            {
				runOnce = false;
                DustExplosion();
                SOTSUtils.PlaySound(SoundID.Item99, Projectile.Center, 0.75f, -0.1f);
            }
            Projectile.velocity *= 1.0175f;
            for(float i = 0.0f; i < 1.0f; i += 0.34f)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center + Projectile.velocity * i - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
                dust.scale *= 0.1f;
                dust.scale += 1.2f;
                dust.color = new Color(186, 92, 92);
                dust.alpha = Projectile.alpha;
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
                dust.velocity *= 0.1f;
            }
			Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.alpha -= 20;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;
        }
	}
}
		