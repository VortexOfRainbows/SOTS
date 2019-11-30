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

namespace SOTS.Projectiles.Ores
{    
    public class PlatinumCurse : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Curse");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18; 
            projectile.timeLeft = 360;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.melee = true; 
            projectile.aiStyle = 0; 
			projectile.alpha = 0;
			projectile.timeLeft = 100;
		}
		public override void AI()
		{
			projectile.ai[1]++;
			int count = 0;
			Player player  = Main.player[projectile.owner];
			if((int)projectile.ai[0] != -1)
			{
				NPC npc = Main.npc[(int)projectile.ai[0]];
				if(npc.active && !npc.friendly && npc.CanBeChasedBy())
				{
					for(int i = 0; i < Main.maxProjectiles; i++)
					{
						Projectile proj = Main.projectile[i];
						if(projectile.type == proj.type && proj.active && projectile.active && proj.ai[0] == projectile.ai[0])
						{
							proj.ai[1] = projectile.ai[1]; //syncing up attack 
							count++;
							if(proj.whoAmI == projectile.whoAmI) break;
						}
					}
					if(count >= 10)
					{
						projectile.Kill();
					}
					else
					{
						Vector2 circularPos = new Vector2(0, npc.height - 12f).RotatedBy(MathHelper.ToRadians((count -1)* 40));
						projectile.position.X = circularPos.X + npc.Center.X - projectile.width/2;
						projectile.position.Y = circularPos.Y + npc.Center.Y - projectile.height/2;
						projectile.timeLeft = 6;
						if(projectile.ai[1] % 90 == 10 * (count - 1))
						{
							LaunchLaser(npc.Center);
						}
					}
				}
				else
				{
					projectile.ai[0] = -1;
				}
			}
			else
			{
				projectile.Kill();
			}
		}
		public void LaunchLaser(Vector2 area)
		{
			Player player  = Main.player[projectile.owner];
			int Probe = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BrightRedLaser"), (int)(projectile.damage * 1f), 0, projectile.owner);
			Main.projectile[Probe].melee = true;
			Main.projectile[Probe].minion = false;
			Main.projectile[Probe].ai[0] = area.X;
			Main.projectile[Probe].ai[1] = area.Y;
		}
	}
}
		
			