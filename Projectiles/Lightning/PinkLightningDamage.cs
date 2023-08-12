using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SOTS.Projectiles.Lightning
{    
    public class PinkLightningDamage : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Pink Lightning");
		}
        public override void SetDefaults()
		{
			Projectile.height = 24;
			Projectile.width = 24;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 6;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if(Projectile.ai[1] <= 0)
            {
				return;
            }
			Player player = Main.player[Projectile.owner];
			target.immune[Projectile.owner] = 0;
			if (Projectile.owner == Main.myPlayer)
			{
				int npcIndex = -1;
				double distanceTB = 196;
				for (int i = 0; i < 200; i++) //find first enemy
				{
					NPC npc = Main.npc[i];
					if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
					{
						if (npcIndex != i && target.whoAmI != i && npc.whoAmI != (int)Projectile.ai[0])
						{
							float disX = Projectile.Center.X - npc.Center.X;
							float disY = Projectile.Center.Y - npc.Center.Y;
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
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<PinkLightningZap>(), (int)(Projectile.damage * 0.6f) + 1, target.whoAmI, Projectile.owner, npc.whoAmI, Projectile.ai[1]);
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
				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 16, 16, 255);
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].velocity += Projectile.velocity * 0.01f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
		