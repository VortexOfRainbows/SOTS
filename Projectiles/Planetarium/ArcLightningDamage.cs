using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SOTS.Projectiles.Planetarium
{    
    public class ArcLightningDamage : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Arc Lightning");
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
				double distanceTB = 216;
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
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<ArcLightningZap>(), (int)(Projectile.damage * 0.8f) + 1, target.whoAmI, Projectile.owner, npc.whoAmI, Projectile.ai[1]);
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
			for (int i = 0; i < 8; i++)
			{
				var num371 = Dust.NewDust(Projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
				Dust dust = Main.dust[num371];
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(160, 200, 220, 100), new Color(120, 140, 180, 100), new Vector2(-0.5f, 0).RotatedBy(Main.rand.Next(360)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.alpha = 100;
			}
		}
	}
}
		