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
    public class PutridHook : ModProjectile 
    {
		int wait = 1;
		int hookTimer = 0;         
		float rotate;
				float oldVelocityY = 0;	
				float oldVelocityX = 0;
				bool collided = false;
				
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Hook");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 38;
			projectile.width = 38;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 360;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.magic = false;
			projectile.ranged = false;
		}
		public override bool PreAI()
		{
			if(projectile.knockBack == 10)
			{
				projectile.knockBack--;
			}
			if(projectile.knockBack == 2)
			{
				projectile.knockBack = 1;
				hookTimer = 0;
				wait = 1;
				collided = false;
				projectile.tileCollide = false;
			}
			return true;
		}
		public override void AI()
		{
			if(wait == 1)
			{
				wait = -1;
				oldVelocityY = projectile.velocity.Y;	
				oldVelocityX = projectile.velocity.X;
			}
			if(!collided)
			{
				projectile.rotation = MathHelper.ToRadians(Main.rand.Next(360));
				projectile.velocity.X += -oldVelocityX / 90f;
				projectile.velocity.Y += -oldVelocityY / 90f;
			}
			else
			{
				if(projectile.knockBack == 1)
				{
					projectile.velocity.X *= 0.93f;
					projectile.velocity.Y *= 0.93f;
				}
			}
			if(NPC.AnyNPCs(mod.NPCType("PutridPinkyPhase2")))
			{
				projectile.timeLeft = 6;
			}
		
			
			hookTimer++;
			if(hookTimer >= 240 && !collided)
			{
				projectile.tileCollide = true;
			}
			if(hookTimer >= 360 && !collided)
			{
				projectile.velocity.X = 0;
				projectile.velocity.Y = 0;
				collided = true;
			}
			for(int i = 0; i < 1000; i++)
			{
				Projectile friendlyProj = Main.projectile[i];
				
				if(friendlyProj.active && projectile.Center.X + 32 > friendlyProj.Center.X && projectile.Center.X - 32 < friendlyProj.Center.X && projectile.Center.Y + 32 > friendlyProj.Center.Y && projectile.Center.Y - 32 < friendlyProj.Center.Y && friendlyProj.friendly == true && friendlyProj.damage > 4 && !friendlyProj.sentry && !friendlyProj.minion)
				{	
					float newVelocityX = -friendlyProj.velocity.X;
					float newVelocityY = -friendlyProj.velocity.Y;
					friendlyProj.Kill();
				}
			}
			
		}
		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("PinkExplosion"), 33, 1, 0);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			projectile.tileCollide = false;
			projectile.velocity.X *= 0.93f;
			projectile.velocity.Y *= 0.93f;
			collided = true;
			return false;
		}
	}
}
		