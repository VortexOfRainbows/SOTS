using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace SOTS.Projectiles.Planetarium
{    
    public class CataclysmBulletDamage : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cataclysm Bullet");
		}
        public override void SetDefaults()
		{
			Projectile.height = 24;
			Projectile.width = 24;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 6;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 24;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
			//Main.NewText(Projectile.whoAmI + " " + Projectile.penetrate + " " + Projectile.timeLeft);
			if(Projectile.ai[1] <= 0)
            {
				return;
            }
			if (Projectile.owner == Main.myPlayer)
			{
				int npcIndex = -1;
				double distanceTB = 336;
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
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<CataclysmLightningZap>(), (int)(Projectile.damage * 0.33f) + 1, target.whoAmI, Projectile.owner, npc.whoAmI, Projectile.ai[1]);
					}
				}
			}
		}
		public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void OnKill(int timeLeft)
		{
			for (int h = 0; h < 20; h++)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 16, 16, 235);
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].velocity += Projectile.velocity * 0.01f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
		