using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace SOTS.Projectiles.Otherworld
{    
    public class CataclysmBulletDamage : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cataclysm Bullet");
		}
        public override void SetDefaults()
		{
			projectile.height = 24;
			projectile.width = 24;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.timeLeft = 6;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 24;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
			//Main.NewText(projectile.whoAmI + " " + projectile.penetrate + " " + projectile.timeLeft);
			if(projectile.ai[1] <= 0)
            {
				return;
            }
			if (projectile.owner == Main.myPlayer)
			{
				int npcIndex = -1;
				double distanceTB = 336;
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
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("CataclysmLightningZap"), (int)(projectile.damage * 0.33f) + 1, target.whoAmI, projectile.owner, npc.whoAmI, projectile.ai[1]);
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
			for (int h = 0; h < 20; h++)
			{
				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 12, projectile.Center.Y - 12), 16, 16, 235);
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].velocity += projectile.velocity * 0.01f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
		