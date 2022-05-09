using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class VisionFlare : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vision Flare");
		}
        public override void SetDefaults()
        {
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.magic = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 16;
			Projectile.width = 200;
			Projectile.height = 60;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
		}
		bool runOnce = true;
        public override bool PreAI()
        {
			if(runOnce)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 117, 1.0f, 0.2f);
				for (int i = 0; i < 30; i++)
				{
					float mult = 0.2f + 0.8f * i / 30f;
					Vector2 circular = new Vector2(32, 0).RotatedBy(MathHelper.ToRadians(i * 12));
					Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5) + circular, 0, 0, ModContent.DustType<CopyDust4>());
					dust.color = new Color(220, 80, 80, 40);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.alpha = 40;
					dust.velocity *= 0.35f;
					dust.velocity += circular * 0.05f;
					dust.scale *= 1.5f;

					if(i % 2 == 0)
					{
						dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
						dust.color = new Color(220, 80, 80, 40);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.alpha = 40;
						dust.velocity *= 0.3f;
						dust.scale *= 1.8f;
					}

					for (int j = 0; j < 2; j++)
					{
						int direction = j * 2 - 1;
						dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
						dust.color = new Color(220, 80, 80, 40);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.alpha = 40;
						dust.velocity.Y *= 0.75f + 0.5f * mult;
						dust.velocity.X += 16f * direction * mult;
						dust.scale *= 2.1f - 0.4f * mult;
					}
				}
				runOnce = false;
            }
            return base.PreAI();
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}
		
			