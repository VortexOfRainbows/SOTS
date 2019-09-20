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

namespace SOTS.Projectiles
{    
    public class FlowerSeed : ModProjectile 
    {	
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flower Seed");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 1;
			projectile.width = 34;
			projectile.height = 38;
            Main.projFrames[projectile.type] = 2;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 3600;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.ranged = false;
			projectile.alpha = 0;
		}
		int damageCounter = 0;
		bool latch = false;
		int enemyIndex = -1;
		float diffPosX = 0;
		float diffPosY = 0;
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			
				
		
			if(!latch)
			{
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
				projectile.spriteDirection = 1;
				if(projectile.velocity.X < 0)
				{
					projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - MathHelper.ToRadians(180);
					projectile.spriteDirection = -1;
				}
			}
			if(latch && enemyIndex != -1)
			{
				NPC target = Main.npc[enemyIndex];
				if(target.active && !target.friendly)
				{
					projectile.aiStyle = 0;
					projectile.position.X = target.Center.X - projectile.width/2 - diffPosX;
					projectile.position.Y = target.Center.Y - projectile.height/2 - diffPosY;
				}
				else
				{
					enemyIndex = -1;
					projectile.aiStyle = 1;
					latch = true;
					projectile.tileCollide = true;
					projectile.friendly = false;
				}
			}
			if(!projectile.tileCollide)
			{
				projectile.velocity *= 0.9f;
			}				
			if(!projectile.friendly && latch || damageCounter >= 310)
			{
				damageCounter++;
				if(damageCounter % 11 == 0 && damageCounter < 300)
				{
					projectile.scale -= 0.02f;
				}
				if(damageCounter >= 300 && damageCounter < 315)
				{
					if(damageCounter == 301)
					projectile.frame = 1;
					projectile.scale += 0.035f;
					projectile.rotation += MathHelper.ToRadians(12);
				}
				if(damageCounter == 310)
				{
					projectile.friendly = true;
					for(int i = 0; i < 360; i += 15)
					{
						Vector2 circularLocation = new Vector2(22, 0).RotatedBy(MathHelper.ToRadians(i));
						
						int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 231);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 0.1f;
					}
				}
				if(damageCounter >= 315)
				{
					projectile.rotation += MathHelper.ToRadians(12);
					projectile.scale -= 0.047f;
					projectile.alpha += 2;
				}
				if(damageCounter >= 335)
				{
					projectile.Kill();
				}
			}
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			hitbox = new Rectangle((int)(projectile.Center.X - 11), (int)(projectile.Center.Y- 8), 22, 16);
			if(projectile.frame == 1)
			{
			hitbox = new Rectangle((int)(projectile.position.X), (int)(projectile.position.Y), projectile.width, projectile.height);
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			
			Player player = Main.player[projectile.owner];
			projectile.friendly = false;
            target.immune[projectile.owner] = 0;
			projectile.tileCollide = false;
			latch = true;
			projectile.damage = (int)(projectile.damage * projectile.ai[1]);
			projectile.velocity *= 0.1f;
			projectile.aiStyle = 0;
			for(int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				if(npc == target)
				{
					if(diffPosX == 0)
					diffPosX = npc.Center.X - projectile.Center.X;
				
					if(diffPosY == 0)
					diffPosY = npc.Center.Y - projectile.Center.Y;
				
					enemyIndex = i;
					break;
				}
			}
			if(target.life <= 0)
			{
					enemyIndex = -1;
					projectile.aiStyle = 1;
					latch = false;
					projectile.tileCollide = true;
			}
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough) 
		{
			width = 2;
			height = 2;
			fallThrough = true;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
				projectile.damage = (int)(projectile.damage * projectile.ai[1]);
				enemyIndex = -1;
				projectile.aiStyle = 0;
				latch = true;
				projectile.friendly = false;
				projectile.velocity *= 0.3f;
				projectile.tileCollide = false;
			return false;
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 15; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 2);
			Main.dust[num1].noGravity = true;
			}
		}
	}
}
		