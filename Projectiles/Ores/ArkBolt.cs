using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Laser;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Ores
{    
    public class ArkBolt : ModProjectile 
    {	
		int helixRot = 0;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ark Bolt");
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 1;
			Projectile.width = 12;
			Projectile.height = 12;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
			Projectile.timeLeft = 600;
		}
		public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.85f / 215f, (255 - Projectile.alpha) * 0.1f / 215f, (255 - Projectile.alpha) * 0.1f / 215f);
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(135);
			Projectile.spriteDirection = 1;

			if(Projectile.timeLeft < 597)
            {
                for (float j = 0; j < 1.0f; j += 0.4f)
                {
                    float curve = 8f * (float)Math.Sin(MathHelper.ToRadians(helixRot * 4f));
                    helixRot++;
                    float radianDir = Projectile.velocity.ToRotation();
                    for (int i = -1; i <= 1; i += 2)
                    {
                        Vector2 helixPos1 = Projectile.Center + Projectile.velocity * j + new Vector2(0, curve * i).RotatedBy(radianDir);
                        Dust dust = Dust.NewDustDirect(helixPos1 - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.0f);
                        dust.scale *= 0.1f;
                        dust.scale += 1.0f;
                        dust.velocity *= 0.1f;
                        dust.noGravity = true;
                        dust.color = Color.Lerp(new Color(255, 10, 10, 0), Color.White, Main.rand.NextFloat(0.5f));
                        dust.fadeIn = 0.2f;
                    }
                }
            }
			else
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.0f);
                    dust.scale *= 0.4f;
                    dust.scale += 1.1f;
                    dust.velocity *= 0.6f;
                    dust.velocity += Main.rand.NextFloat(0.5f) * Projectile.velocity;
                    dust.noGravity = true;
                    dust.color = Color.Lerp(new Color(255, 10, 10, 0), Color.White, Main.rand.NextFloat(0.5f));
                    dust.fadeIn = 0.2f;
                }
            }
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Player player = Main.player[Projectile.owner];
            target.immune[Projectile.owner] = 10;
			if(Projectile.owner == Main.myPlayer)
			{
				int npcIndex = -1;
				int npcIndex1 = -1;
				for(int j = 0; j < 2; j++)
				{
					double distanceTB = 216;
					for(int i = 0; i < 200; i++) //find first enemy
					{
						NPC npc = Main.npc[i];
						if(!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
						{
							if(npcIndex != i && npcIndex1 != i && target.whoAmI != i)
							{
								float disX = Projectile.Center.X - npc.Center.X;
								float disY = Projectile.Center.Y - npc.Center.Y;
								double dis = Math.Sqrt(disX * disX + disY * disY);
								if(dis < distanceTB && j == 0)
								{
									distanceTB = dis;
									npcIndex = i;
								}
								if(dis < distanceTB && j == 1)
								{
									distanceTB = dis;
									npcIndex1 = i;
								}
							}
						}
					}
				}
				for(int projNum = 0; projNum < 1; projNum++)
				{
					if(npcIndex != -1)
					{
						NPC npc = Main.npc[npcIndex];
						if(!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
						{
							LaunchLaser(npc.Center);
						}
					}
					if(npcIndex1 != -1)
					{
						NPC npc = Main.npc[npcIndex1];
						if(!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
						{
							LaunchLaser(npc.Center);
						}
					}
				}
            }
		}
		public override void OnKill(int timeLeft)
		{
			for(int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.0f);
                dust.scale *= 0.5f;
                dust.scale += 1.2f;
                dust.velocity *= 0.9f;
                dust.velocity += Main.rand.NextFloat(0.2f) * Projectile.velocity;
                dust.noGravity = true;
                dust.color = Color.Lerp(new Color(255, 10, 10, 0), Color.White, Main.rand.NextFloat(0.5f));
                dust.fadeIn = 0.2f;
            }
		}
		public void LaunchLaser(Vector2 area)
		{
			int Probe = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<BrightRedLaser>(), (int)(Projectile.damage * 1.5f) + 1, 0, Projectile.owner, area.X, area.Y);
			Main.projectile[Probe].DamageType = DamageClass.Magic;
		}
	}
}