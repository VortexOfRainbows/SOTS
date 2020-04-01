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

namespace SOTS.Void
{    
    public class VoidHealEffect : ModProjectile 
    {	float distance = 50f;  
		int rotation = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Heal Effect");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 2;
			projectile.width = 2;
			projectile.penetrate = 1;
			projectile.friendly = false;
			projectile.timeLeft = 36;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
		}
		/**
		* The reason I use a projectile to execute server syncing is because I don't know enough to do it a different way
		* It also allows me to generate dust in a special way, so it isn't just a workaround, but also useful in other ways
		*/
		public override void AI()
		{
			Player player  = Main.player[projectile.owner];
			if(rotation == 0)
			{
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(110, 90, 125, 255), string.Concat((int)projectile.ai[1]), false, false);
			}
			else
			{
				int counter = 0;
				for(int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if(proj.active)
					{
						counter++;
						if(proj.type == projectile.type)
						{
							counter += 9;
						}
					}
				}
				if(counter > 500) //kill this effect if there are too many projectiles on screen, or when the amount of animations already active is too great
				{
					projectile.Kill();
				}
			}
			rotation += 1;
			distance -= 0.45f;
			projectile.scale *= 0.98f;
			projectile.position = player.Center - new Vector2(1,1);
			for(int i = (int)((projectile.ai[1] * 0.2f) + 1); i > 0; i--)
			{
				Vector2 circularLocation = new Vector2(projectile.velocity.X -distance, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotation + projectile.ai[0] + (i * (360/((projectile.ai[1] * 0.2f) + 1)))));
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 37);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Lighting.AddLight(circularLocation, 110, 90, 125); //adds game light at the area
			}
		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			for(int i = 5; i > 0; i --)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), projectile.width, projectile.height, 37);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.5f;
			}
		}
	}
}
		