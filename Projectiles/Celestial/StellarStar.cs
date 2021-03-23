using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Celestial
{    
    public class StellarStar : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Stariest of Starry Stars");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;    
		}
		
        public override void SetDefaults()
        {
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.width = 14;
			projectile.height = 14;
			projectile.timeLeft = 1200;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void AI()
		{ 
			projectile.rotation = Main.rand.Next(38);
			float minDist = 560;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 1.2f;
			float slowdown = 0.95f;
			if(projectile.ai[1] == 1)
			{
				projectile.magic = true;
				projectile.ranged = false;
				minDist = 240;
				speed = 5f;
				slowdown = 0.825f;
			}
			if(projectile.friendly == true && projectile.hostile == false)
			{
				for(int i = 0; i < Main.npc.Length - 1; i++)
				{
					NPC target = Main.npc[i];
					if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
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
				
				if(target2 != -1 && projectile.timeLeft > 40)
				{
					NPC toHit = Main.npc[target2];
					if(toHit.active == true)
					{
						
					dX = toHit.Center.X - projectile.Center.X;
					dY = toHit.Center.Y - projectile.Center.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					speed /= distance;
				    projectile.velocity *= slowdown;
					projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
			if(projectile.timeLeft <= 40)
			{
				projectile.alpha += 7;
				projectile.velocity *= 0.945f;
			}
			if(projectile.ai[0] == 1)
			{
				Main.PlaySound(SoundID.Item9, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				projectile.ai[0] = 0;
				int color = Main.rand.Next(2);
				float size = 120f;
				float starPosX = projectile.Center.X - size/2f;
				float starPosY = projectile.Center.Y - size/6f;
				for(int i = 0; i < 5; i ++)
				{
					float rads = MathHelper.ToRadians(144 * i);
					for(float j = 0; j < size; j += 8f)
					{
						int num1 = Dust.NewDust(new Vector2(starPosX, starPosY), 0, 0, color % 2 == 0 ? 88 : 21);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 0.1f;
						
						Vector2 rotationDirection = new Vector2(8f, 0).RotatedBy(rads);
						starPosX += rotationDirection.X;
						starPosY += rotationDirection.Y;
					}
				}
				projectile.timeLeft = 40;
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
            target.immune[projectile.owner] = 11 - (int)(projectile.ai[1] * 5);
			projectile.ai[0] = 1;
			projectile.velocity *= 0.8f;
			
			if(projectile.owner == Main.myPlayer)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("StellarHitbox"), projectile.damage, projectile.knockBack * 1.5f, player.whoAmI, projectile.ai[1]);
		
			projectile.timeLeft = 40;
			projectile.friendly = false;
			projectile.netUpdate = true;
		}
	}
}
		