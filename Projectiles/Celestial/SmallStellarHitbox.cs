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
using SOTS.Void;

namespace SOTS.Projectiles.Celestial
{    
    public class SmallStellarHitbox : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starsplosion");
		}
        public override void SetDefaults()
        {
			projectile.height = 80;
			projectile.width = 80;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 5;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
		}
        public override bool? CanHitNPC(NPC target)
        {
            return target.whoAmI != projectile.ai[0];
        }
		bool runOnce = true;
        public override void AI()
		{
			if(runOnce)
			{
				float size = 32f;
				float starPosX = projectile.Center.X - size / 2f;
				float starPosY = projectile.Center.Y - size / 6f;
				float iterateBy = 2f;
				for (int i = 0; i < 5; i++)
				{
					float rads = MathHelper.ToRadians(144 * i);
					for (float j = 0; j < size; j += iterateBy)
					{
						Vector2 direction = -(projectile.Center - new Vector2(starPosX, starPosY)).SafeNormalize(Vector2.Zero);
						Dust dust = Dust.NewDustDirect(new Vector2(starPosX, starPosY), 0, 0, ModContent.DustType<CopyDust4>(), 50);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale = 1.3f;
						dust.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 60 + j / size * 60), true);
						dust.velocity = direction * 1.5f + Main.rand.NextVector2Circular(0.1f, 0.1f);
						Vector2 rotationDirection = new Vector2(iterateBy, 0).RotatedBy(rads);
						starPosX += rotationDirection.X;
						starPosY += rotationDirection.Y;
					}
				}
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 9, 1f, 0.2f);
				runOnce = false;
			}
		}
    }
}
		