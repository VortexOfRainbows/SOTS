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

namespace SOTS.Projectiles.Margrit
{    
    public class MargritToxin : ModProjectile 
    {	int bounce = 6;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Toxin");
			
		}
        public override void SetDefaults()
        {
			projectile.aiStyle = 2;
			projectile.height = 38;
			projectile.width = 38;
			projectile.thrown = true;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.timeLeft = 7200;
			projectile.tileCollide = true;
		}
		public override void AI()
		{ 
			for(int i = 0; i < 5; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 38, 38, 197);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
		}
public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			//If collide with tile, reduce the penetrate.
			//So the projectile can reflect at most 5 times
			bounce--;
			if (bounce <= 0)
			{
				projectile.Kill();
			}
			else
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				Main.PlaySound(SoundID.Item10, projectile.position);
			
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
				target.AddBuff(mod.BuffType("MargritToxin"), projectile.damage * 60, false);
			
		}
	}
}
		