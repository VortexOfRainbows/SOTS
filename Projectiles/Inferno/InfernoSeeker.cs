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
using SOTS.NPCs;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.Inferno
{    
    public class InfernoSeeker : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Inferno Seeker");
		}
        public override void SetDefaults()
        {
			projectile.height = 16;
			projectile.width = 16;
			projectile.ranged = true;
			projectile.timeLeft = 150;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.extraUpdates = 1;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.OnFire, 600, false);
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 6; i++)
            {
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.7f;
				dust.noGravity = true;
				dust.scale += 0.1f;
				dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1f));
				dust.fadeIn = 0.1f;
				dust.scale *= 1.4f;
				dust.alpha = projectile.alpha; 

				dust = Dust.NewDustDirect(projectile.Center + new Vector2(-4, -4), 0, 0, DustID.Fire);
				dust.noGravity = true;
				dust.scale *= 1.1f;
				dust.velocity *= 1.2f;
				dust.velocity += projectile.velocity * 0.1f;
				dust.alpha = projectile.alpha;
			}
        }
        public override void AI()
		{
			if(Main.rand.NextBool(2))
			{
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.1f;
				dust.noGravity = true;
				dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1f));
				dust.fadeIn = 0.1f;
				dust.scale = 1.2f;
				dust.alpha = projectile.alpha;
			}
			else
            {
				Dust dust = Dust.NewDustDirect(projectile.Center + new Vector2(-4, -4), 0, 0, DustID.Fire);
				dust.noGravity = true;
				dust.scale = 1.3f;
				dust.velocity *= 0.1f;
				dust.velocity += projectile.velocity * 0.1f;
				dust.alpha = projectile.alpha;
			}
			projectile.velocity *= 0.97f;
			projectile.ai[0]++;
			if (projectile.ai[0] > 30)
			{
				int target = SOTSNPCs.FindTarget_Basic(projectile.Center, 320, this);
				if (target >= 0)
				{
					NPC npc = Main.npc[target];
					projectile.velocity = Vector2.Lerp(projectile.velocity, (npc.Center - projectile.Center).SafeNormalize(Vector2.Zero) * (projectile.velocity.Length() + 2), projectile.ai[0] * 0.0025f);
					projectile.friendly = true;
				}
			}
			projectile.alpha++;
		}	
	}
}
		