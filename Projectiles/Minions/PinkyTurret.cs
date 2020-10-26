using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Minions
 
{
    public class PinkyTurret : ModProjectile
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Turret");
			
		}
        public override void SetDefaults()
        {
            projectile.width = 30; 
            projectile.height = 30; 
            projectile.hostile = false; 
            projectile.friendly = false;
            projectile.ignoreWater = true;  
            Main.projFrames[projectile.type] = 1; 
            projectile.penetrate = -1;
            projectile.tileCollide = false; 
            projectile.sentry = true;
			projectile.netImportant = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
		}
		public int findTargets(int[] npcList)
		{
			float minDist = 640;
			int target2 = -1;
			float distance = 0;
			for (int i = 0; i < Main.npc.Length - 1; i++)
			{
				NPC target = Main.npc[i];
				if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.CanBeChasedBy() && !npcList.Contains(target.whoAmI))
				{
					distance = Vector2.Distance(target.Center, projectile.Center);
					bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height);
					bool closeThroughWall = distance < 32f;
					if (distance < minDist && (lineOfSight || closeThroughWall))
					{
						minDist = distance;
						target2 = i;
					}
				}
			}
			return target2;
		}
		public void fireProj(int target)
		{
			float speed = 8f;
			if (target != -1)
			{
				NPC toHit = Main.npc[target];
				if (toHit.active)
				{
					float dX = toHit.Center.X - projectile.Center.X;
					float dY = toHit.Center.Y - projectile.Center.Y;
					float distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					speed /= distance;

					Vector2 shootTo = new Vector2(dX * speed, dY * speed);
					if (projectile.owner == Main.myPlayer)
					{
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootTo.X, shootTo.Y, mod.ProjectileType("PinkBubble"), projectile.damage, 0, Main.myPlayer);
					}
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 21);
				}
			}
		}
        public override void AI()
        {
			Main.player[projectile.owner].UpdateMaxTurrets();
			projectile.rotation += 0.11f;
			int[] foundNpcList = {-1,-1,-1,-1};

			int target1 = findTargets(foundNpcList);
			foundNpcList[0] = target1;
			int target2 = findTargets(foundNpcList);
			foundNpcList[1] = target2;
			int target3 = findTargets(foundNpcList);
			foundNpcList[2] = target3;
			int target4 = findTargets(foundNpcList);
			foundNpcList[3] = target4;

			projectile.ai[0] += 1f;
			if (projectile.ai[0] >= 50)
			{
				fireProj(target1);
				fireProj(target2);
				fireProj(target3);
				fireProj(target4);
				projectile.ai[0] = 0f;
			}
        }
    }
}