using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;

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
			Projectile.aiStyle = 2;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.width = 46;
			Projectile.height = 36;
			Projectile.timeLeft = 6000;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;
			fallThrough = true;
			return true;
		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			Projectile.alpha = 0;		
			float minDist = 500;
			int target2 = -1;
			float dX;
			float dY;
			float distance;
			float speed = 0.4f;
			if(Projectile.friendly == true && Projectile.hostile == false)
			{
				for(int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.CanBeChasedBy())
					{
						dX = target.Center.X - Projectile.Center.X;
						dY = target.Center.Y - Projectile.Center.Y;
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
					{						dX = toHit.Center.X - Projectile.Center.X;
						dY = toHit.Center.Y - Projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
				   
						Projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Projectile.timeLeft -= 1000;
        }
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 3; i++)
			{
				int goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61,64), 1f);	
				Main.gore[goreIndex].scale = 0.65f;
				Main.gore[goreIndex].velocity.Y *= 0.25f;
				Main.gore[goreIndex].velocity.X *= 0.25f;
			}
            SoundEngine.PlaySound(2, (int)(Projectile.Center.X), (int)(Projectile.Center.Y), 14, 0.4f);
		}
	}
}
		