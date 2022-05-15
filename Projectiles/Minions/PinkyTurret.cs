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
            Projectile.width = 30; 
            Projectile.height = 30; 
            Projectile.hostile = false; 
            Projectile.friendly = false;
            Projectile.ignoreWater = true;  
            Main.projFrames[Projectile.type] = 1; 
            Projectile.penetrate = -1;
            Projectile.tileCollide = false; 
            Projectile.sentry = true;
			Projectile.netImportant = true;
			Projectile.timeLeft = Projectile.SentryLifeTime;
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
					distance = Vector2.Distance(target.Center, Projectile.Center);
					bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
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
					float dX = toHit.Center.X - Projectile.Center.X;
					float dY = toHit.Center.Y - Projectile.Center.Y;
					float distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					speed /= distance;

					Vector2 shootTo = new Vector2(dX * speed, dY * speed);
					if (Projectile.owner == Main.myPlayer)
					{
						Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, shootTo.X, shootTo.Y, Mod.Find<ModProjectile>("PinkBubble").Type, Projectile.damage, 0, Main.myPlayer);
					}
					SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 21);
				}
			}
		}
        public override void AI()
        {
			Main.player[Projectile.owner].UpdateMaxTurrets();
			Projectile.rotation += 0.11f;
			int[] foundNpcList = {-1,-1,-1,-1};

			int target1 = findTargets(foundNpcList);
			foundNpcList[0] = target1;
			int target2 = findTargets(foundNpcList);
			foundNpcList[1] = target2;
			int target3 = findTargets(foundNpcList);
			foundNpcList[2] = target3;
			int target4 = findTargets(foundNpcList);
			foundNpcList[3] = target4;

			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 50)
			{
				fireProj(target1);
				fireProj(target2);
				fireProj(target3);
				fireProj(target4);
				Projectile.ai[0] = 0f;
			}
        }
    }
}