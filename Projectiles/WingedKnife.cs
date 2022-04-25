using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles
{    
    public class WingedKnife : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winged Knife");
		}
        public override void SetDefaults()
        {
			projectile.aiStyle = 2;
			projectile.thrown = true;
			projectile.friendly = true;
			projectile.width = 46;
			projectile.height = 36;
			projectile.timeLeft = 6000;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 10;
			height = 10;
			fallThrough = true;
			return true;
		}
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			projectile.alpha = 0;		
			float minDist = 500;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 0.4f;
			if(projectile.friendly == true && projectile.hostile == false)
			{
				for(int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.CanBeChasedBy())
					{
						dX = target.Center.X - projectile.Center.X;
						dY = target.Center.Y - projectile.Center.Y;
						distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
						if(distance < minDist)
						{
							minDist = distance;
							target2 = i;
						}
					}
				}
				if(target2 != -1)
				{
					NPC toHit = Main.npc[target2];
					if(toHit.active == true)
					{						dX = toHit.Center.X - projectile.Center.X;
						dY = toHit.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
				   
						projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			projectile.timeLeft -= 1000;
        }
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 3; i++)
			{
				int goreIndex = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61,64), 1f);	
				Main.gore[goreIndex].scale = 0.65f;
				Main.gore[goreIndex].velocity.Y *= 0.25f;
				Main.gore[goreIndex].velocity.X *= 0.25f;
			}
            Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 14, 0.4f);
		}
	}
}
		