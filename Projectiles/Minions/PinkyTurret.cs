using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Minions
 
{
    public class PinkyTurret : ModProjectile
    {	
        public override void SetDefaults()
        {
            Projectile.width = 30; 
            Projectile.height = 30; 
            Projectile.hostile = false; 
            Projectile.friendly = false;
            Projectile.ignoreWater = true;  
            Projectile.penetrate = -1;
            Projectile.tileCollide = false; 
            Projectile.sentry = true;
			Projectile.netImportant = true;
			Projectile.timeLeft = Projectile.SentryLifeTime;
		}
		public int FindTargets(int[] npcList)
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
		public void FireProj(int target)
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
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, shootTo.X, shootTo.Y, ModContent.ProjectileType<PinkBubble>(), Projectile.damage, 0, Main.myPlayer);
					}
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item21, Projectile.Center);
				}
			}
		}
        public override void OnSpawn(IEntitySource source)
        {
			Main.player[Projectile.owner].UpdateMaxTurrets();
        }
        public override void AI()
        {
			Projectile.rotation += 0.11f;
			int[] foundNpcList = {-1,-1,-1,-1};

			for(int i = 0; i < 4; i++)
				foundNpcList[i] = FindTargets(foundNpcList);

			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 50)
            {
                for (int i = 0; i < 4; i++)
                    FireProj(foundNpcList[i]);
				Projectile.ai[0] = 0f;
			}
        }
    }
}