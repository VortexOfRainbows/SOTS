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

namespace SOTS.Projectiles.Ores
{    
    public class SoulLock : ModProjectile 
    {	float distance = 30f;  
		int rotation = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Lock");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 2;
			projectile.width = 2;
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
			if((int)projectile.ai[0] != -1)
			{
				NPC npc = Main.npc[(int)projectile.ai[0]];
				if(!npc.friendly && npc.lifeMax > 5 && npc.active)
				{
					projectile.position.X = npc.Center.X - 1;
					projectile.position.Y = npc.Center.Y - 1;
				}
				else
				{
					projectile.Kill();
				}
			}
			Vector2 circularLocation = new Vector2(projectile.velocity.X -distance, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotation));
			rotation += 15;
			distance -= 0.5f;
			projectile.scale *= 0.98f;
			projectile.alpha++;
			
			Player player  = Main.player[projectile.owner];
			
			
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 16);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			if(projectile.timeLeft <= 2)
			{
				projectile.friendly = true;
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
            target.immune[projectile.owner] = 0;
			int heal = 1;
			if(Main.rand.Next(4) == 0) heal = 2;
			
			if(Main.rand.Next(16) == 0) heal = 3;
			
			if(player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("HealProj"), 0, 0, player.whoAmI, heal, 0);	
			}
        }
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			for(int i = 5; i > 0; i --)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), projectile.width, projectile.height, 16);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.5f;
			}
		}
	}
}
		