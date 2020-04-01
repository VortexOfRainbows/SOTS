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

namespace SOTS.Projectiles.Crushers
{    
    public class ManaLock : ModProjectile 
    {	float distance = 30f;  
		int rotation = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Lock");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 43;
			projectile.width = 43;
			projectile.penetrate = 1;
			projectile.friendly = false;
			projectile.timeLeft = 60;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.hostile = false;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			Vector2 circularLocation = new Vector2(projectile.velocity.X -distance, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotation));
			rotation += 15;
			distance -= 0.5f;
			projectile.velocity *= 0.98f;
			projectile.scale *= 0.98f;
			projectile.alpha++;
			
			Player player  = Main.player[projectile.owner];
			
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 15);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			Main.dust[num1].scale = 1.5f;
		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("HealProj"), 1, 0, player.whoAmI, (int)projectile.ai[0], 3);	
			}
			for(int i = 5; i > 0; i --)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), projectile.width, projectile.height, 15);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.5f;
				Main.dust[num1].scale = 1.5f;
			}
		}
	}
}
		