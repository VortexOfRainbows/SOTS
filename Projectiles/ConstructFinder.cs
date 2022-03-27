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
using SOTS.NPCs.ArtificialDebuffs;
using System.Linq;

namespace SOTS.Projectiles
{    
    public class ConstructFinder : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Construct Finder");
		}
        public override void SetDefaults()
        {
			projectile.height = 16;
			projectile.width = 16;
			projectile.timeLeft = 7200;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.extraUpdates = 3;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			return false;
		}
		public override bool? CanHitNPC(NPC target)
        {
			return false;
		}
        public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 20; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.8f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(0, 192, 255, 100), new Color(0, 90, 136, 100), 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(Main.rand.NextFloat(360))));
				dust.fadeIn = 0.1f;
				dust.scale *= 1.2f;
				dust.alpha = projectile.alpha;
			}
        }
        public override void AI()
		{
			projectile.ai[0]++;
			Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
			dust.velocity *= 0.1f;
			dust.noGravity = true;
			dust.color = Color.Lerp(new Color(0, 192, 255, 100), new Color(0, 90, 136, 100), 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(projectile.ai[0])));
			dust.fadeIn = 0.1f;
			dust.scale = 1.2f;
			dust.alpha = projectile.alpha;
			if (projectile.ai[0] > 20)
			{
				int npcId = -1;
				for (int i = 0; i < 200; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy() && DebuffNPC.constructs.Contains(npc.type) && npc.Distance(projectile.Center) > 64)
					{
						npcId = i;
						break;
					}
				}
				if (npcId == -1)
				{
					projectile.Kill();
				}
				else
				{
					NPC npc = Main.npc[npcId];
					Vector2 toNPC = npc.Center - projectile.Center;
					projectile.velocity = Vector2.Lerp(projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * 3, 0.03f);
				}
			}
			else
				projectile.velocity *= 0.95f;
		}	
	}
}
		