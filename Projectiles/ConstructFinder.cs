using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SOTS.Dusts;
using System.Linq;

namespace SOTS.Projectiles
{    
    public class ConstructFinder : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Construct Finder");
		}
        public override void SetDefaults()
        {
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.timeLeft = 7200;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 3;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.DisableCrit();
        }
        public override bool PreDraw(ref Color lightColor)
        {
			return false;
		}
		public override bool? CanHitNPC(NPC target)
        {
			return false;
		}
        public override void OnKill(int timeLeft)
        {
			for(int i = 0; i < 20; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.8f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(0, 192, 255, 100), new Color(0, 90, 136, 100), 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(Main.rand.NextFloat(360))));
				dust.fadeIn = 0.1f;
				dust.scale *= 1.2f;
				dust.alpha = Projectile.alpha;
			}
        }
        public override void AI()
		{
			Projectile.ai[0]++;
			Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
			dust.velocity *= 0.1f;
			dust.noGravity = true;
			dust.color = Color.Lerp(new Color(0, 192, 255, 100), new Color(0, 90, 136, 100), 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[0])));
			dust.fadeIn = 0.1f;
			dust.scale = 1.2f;
			dust.alpha = Projectile.alpha;
			if (Projectile.ai[0] > 20)
			{
				int npcId = -1;
				for (int i = 0; i < 200; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy() && Common.GlobalNPCs.DebuffNPC.Constructs.Contains(npc.type) && npc.Distance(Projectile.Center) > 64)
					{
						npcId = i;
						break;
					}
				}
				if (npcId == -1)
				{
					Projectile.Kill();
				}
				else
				{
					NPC npc = Main.npc[npcId];
					Vector2 toNPC = npc.Center - Projectile.Center;
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * 3, 0.03f);
				}
			}
			else
				Projectile.velocity *= 0.95f;
		}	
	}
}
		