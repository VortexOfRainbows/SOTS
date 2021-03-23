using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class CurseSingularity : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Singularity");
			Main.projFrames[projectile.type] = 11;
		}
        public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 44;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.timeLeft = 960;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.ranged = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, projectile.height / 2);
			Color color = Color.Black;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(150, 100, 200, 0);
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(48 + 100, 0 + 100, 108 + 100);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<DarkLight>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.NextFloat(1000));
			}
		}
		int counter = 0;
		public override void AI()
		{
			counter++;
			if (Main.rand.NextBool(3) && counter > 12)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.7f;
				dust.noGravity = true;
				dust.color = new Color(150, 100, 200, 0);
				dust.fadeIn = 0.1f;
				dust.scale = 1.5f;
				dust.alpha = 40;
			}			
			if(projectile.timeLeft % 6 == 0)
			{
				projectile.alpha++;
			}
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
				projectile.friendly = true;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 11;
            }
			
			float minDist = 560;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 1.25f;
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
							bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height);
							if(lineOfSight)
							{
								minDist = distance;
								target2 = i;
							}
						}
					}
				}
				
				if(target2 != -1)
				{
					NPC toHit = Main.npc[target2];
					if(toHit.active == true)
					{
						dX = toHit.Center.X - projectile.Center.X;
						dY = toHit.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
						projectile.velocity *= 0.95f;
						projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
        }
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 30; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num1];
				dust.velocity += 0.15f * projectile.velocity;
				dust.noGravity = true;
				dust.color = new Color(150, 100, 200, 0);
				dust.fadeIn = 0.1f;
				dust.scale *= 2.5f;
				dust.alpha = 40;
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
	}
}
		