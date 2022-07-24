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
			Main.projFrames[Projectile.type] = 11;
		}
        public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 44;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 960;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.DamageType = DamageClass.Ranged;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, Projectile.height / 2);
			Color color = Color.Black;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(150, 100, 200, 0);
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(48 + 100, 0 + 100, 108 + 100);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[Projectile.owner];
			if(Projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<DarkLight>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Main.rand.NextFloat(1000));
			}
		}
		int counter = 0;
		public override void AI()
		{
			counter++;
			if (Main.rand.NextBool(3) && counter > 12)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.7f;
				dust.noGravity = true;
				dust.color = new Color(150, 100, 200, 0);
				dust.fadeIn = 0.1f;
				dust.scale = 1.5f;
				dust.alpha = 40;
			}			
			if(Projectile.timeLeft % 6 == 0)
			{
				Projectile.alpha++;
			}
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
				Projectile.friendly = true;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 11;
            }
			
			float minDist = 560;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 1.25f;
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
							bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
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
						dX = toHit.Center.X - Projectile.Center.X;
						dY = toHit.Center.Y - Projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
						Projectile.velocity *= 0.95f;
						Projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
        }
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 30; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[num1];
				dust.velocity += 0.15f * Projectile.velocity;
				dust.noGravity = true;
				dust.color = new Color(150, 100, 200, 0);
				dust.fadeIn = 0.1f;
				dust.scale *= 2.5f;
				dust.alpha = 40;
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
	}
}
		