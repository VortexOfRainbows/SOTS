using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SOTS.Projectiles.Otherworld
{    
    public class PlasmaLightningDamage : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Lightning");
		}
        public override void SetDefaults()
		{
			projectile.height = 24;
			projectile.width = 24;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.timeLeft = 6;
			projectile.tileCollide = false;
			projectile.alpha = 255;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(projectile.ai[1] <= 0)
            {
				return;
            }
			Player player = Main.player[projectile.owner];
			if (projectile.owner == Main.myPlayer)
			{
				int npcIndex = -1;
				double distanceTB = 400;
				for (int i = 0; i < 200; i++) //find first enemy
				{
					NPC npc = Main.npc[i];
					if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
					{
						if (npcIndex != i && target.whoAmI != i && npc.whoAmI != (int)projectile.ai[0])
						{
							float disX = projectile.Center.X - npc.Center.X;
							float disY = projectile.Center.Y - npc.Center.Y;
							double dis = Math.Sqrt(disX * disX + disY * disY);
							if (dis < distanceTB)
							{
								distanceTB = dis;
								npcIndex = i;
							}
						}
					}
				}
				if (npcIndex != -1)
				{
					NPC npc = Main.npc[npcIndex];
					if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
					{
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("PlasmaLightningZap"), (int)(projectile.damage * 0.7f), target.whoAmI, projectile.owner, npc.whoAmI, projectile.ai[1]);
					}
				}
			}
		}
		public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
			{
				var num371 = Dust.NewDust(projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, mod.DustType("CopyDust4"), 0, 0, 100, default, 1.6f);
				Dust dust = Main.dust[num371];
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(160, 200, 220, 70), new Color(120, 140, 180, 70), new Vector2(-0.5f, 0).RotatedBy(Main.rand.Next(360)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.alpha = 100;
			}
		}
	}
}
		