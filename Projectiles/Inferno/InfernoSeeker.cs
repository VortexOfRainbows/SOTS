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
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.ranged = true;
			Projectile.timeLeft = 150;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
		}
        public override bool PreDraw(ref Color lightColor)
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
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.7f;
				dust.noGravity = true;
				dust.scale += 0.1f;
				dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1f));
				dust.fadeIn = 0.1f;
				dust.scale *= 1.4f;
				dust.alpha = Projectile.alpha; 

				dust = Dust.NewDustDirect(Projectile.Center + new Vector2(-4, -4), 0, 0, DustID.Fire);
				dust.noGravity = true;
				dust.scale *= 1.1f;
				dust.velocity *= 1.2f;
				dust.velocity += Projectile.velocity * 0.1f;
				dust.alpha = Projectile.alpha;
			}
        }
        public override void AI()
		{
			if(Main.rand.NextBool(2))
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.1f;
				dust.noGravity = true;
				dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1f));
				dust.fadeIn = 0.1f;
				dust.scale = 1.2f;
				dust.alpha = Projectile.alpha;
			}
			else
            {
				Dust dust = Dust.NewDustDirect(Projectile.Center + new Vector2(-4, -4), 0, 0, DustID.Fire);
				dust.noGravity = true;
				dust.scale = 1.3f;
				dust.velocity *= 0.1f;
				dust.velocity += Projectile.velocity * 0.1f;
				dust.alpha = Projectile.alpha;
			}
			Projectile.velocity *= 0.97f;
			Projectile.ai[0]++;
			if (Projectile.ai[0] > 30)
			{
				int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 320, this);
				if (target >= 0)
				{
					NPC npc = Main.npc[target];
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (Projectile.velocity.Length() + 2), Projectile.ai[0] * 0.0025f);
					Projectile.friendly = true;
				}
			}
			Projectile.alpha++;
		}	
	}
}
		