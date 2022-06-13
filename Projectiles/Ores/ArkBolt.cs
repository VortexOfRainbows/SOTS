using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Laser;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Ores
{    
    public class ArkBolt : ModProjectile 
    {	
		int helixRot = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ark Bolt");
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 1;
			Projectile.width = 12;
			Projectile.height = 12;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			// Projectile.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
		}
		public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.85f / 215f, (255 - Projectile.alpha) * 0.1f / 215f, (255 - Projectile.alpha) * 0.1f / 215f);
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) - MathHelper.ToRadians(135);
			Projectile.spriteDirection = 1;
			
			Vector2 curve = new Vector2(7f,0).RotatedBy(MathHelper.ToRadians(helixRot * 5f));
			helixRot ++;
			
			float radianDir = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			Vector2 helixPos1 = Projectile.Center + new Vector2(curve.X, 0).RotatedBy(radianDir + MathHelper.ToRadians(90));
			int num1 = Dust.NewDust(new Vector2(helixPos1.X - 4, helixPos1.Y - 4), 4, 4, 235);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.2f;
			
			Vector2 helixPos2 = Projectile.Center + new Vector2(curve.X, 0).RotatedBy(radianDir - MathHelper.ToRadians(90));
			num1 = Dust.NewDust(new Vector2(helixPos2.X - 4, helixPos2.Y - 4), 4, 4, 235);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.2f;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
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
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 15; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 235);
				Main.dust[num1].noGravity = true;
			}
		}
		public void LaunchLaser(Vector2 area)
		{
			int Probe = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<BrightRedLaser>(), (int)(Projectile.damage * 1.5f) + 1, 0, Projectile.owner, area.X, area.Y);
			Main.projectile[Probe].DamageType = DamageClass.Magic;
			//Main.projectile[Probe].minion = false;
		}
	}
}