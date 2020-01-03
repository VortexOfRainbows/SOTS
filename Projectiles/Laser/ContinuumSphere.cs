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
namespace SOTS.Projectiles.Laser
{    
    public class ContinuumSphere : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Continuum Sphere");
		}
		
        public override void SetDefaults()
        {
			projectile.height = 30;
			projectile.width = 30;
			projectile.penetrate = 24;
			projectile.friendly = false;
			projectile.timeLeft = 6004;
			projectile.tileCollide = false;
			projectile.hostile = false;
		}
		public override bool PreAI()
        {
			if(projectile.active)
			return true;
		
			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			float newAi = projectile.ai[1] / 13f;
			double frequency = 0.3;
			double center = 130;
			double width = 125;
			double red = Math.Sin(frequency * (double)newAi) * width + center;
			double grn = Math.Sin(frequency * (double)newAi + 2.0) * width + center;
			double blu = Math.Sin(frequency * (double)newAi + 4.0) * width + center;
			Color color = new Color((int)red, (int)grn, (int)blu);
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		int ai1 = 2;
		public override bool ShouldUpdatePosition() 
		{
			return false;
		}
		public override void AI()
		{
			Player player  = Main.player[projectile.owner];
			if(projectile.ai[1] == 0f)
			{
				projectile.ai[0] = Main.rand.Next(1020);
			}
			projectile.ai[1] ++;
			ai1++;			
			Vector2 cursorArea = Main.MouseWorld;
			if(Main.myPlayer == player.whoAmI)
			{
				projectile.netUpdate = true;
				float shootToX = cursorArea.X - player.Center.X;
				float shootToY = cursorArea.Y - player.Center.Y;
				float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				distance = 7.25f / distance;
			   
				shootToX *= distance * 5f;
				shootToY *= distance * 5f;
						   
				double startingDirection = Math.Atan2((double)-shootToY, (double)-shootToX);
				startingDirection *= 180/Math.PI;
				
				if(player.channel || projectile.timeLeft > 6)
				{
					projectile.timeLeft = 6;
					projectile.alpha = 0;
					if(Main.myPlayer == projectile.owner && ai1 % 3 == 0)
					{
						int projID = Projectile.NewProjectile(player.Center.X + shootToX, player.Center.Y + shootToY, shootToX, shootToY, mod.ProjectileType("CollapseLaser"), projectile.damage, 1f, projectile.owner, projectile.ai[1], 0f);
						Main.projectile[projID].ai[1] = projectile.ai[1];
					}						
				}
				projectile.ai[0] = (float)startingDirection;
				double deg = (double) projectile.ai[0]; 
				double rad = deg * (Math.PI / 180);
				double dist = 32;
				projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
				projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
			}
		}
	}
}