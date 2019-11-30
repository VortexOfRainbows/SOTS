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

namespace SOTS.Projectiles.Minions
{    
    public class BlizzardProbe : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blizzard Probe");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;    
		}
		
        public override void SetDefaults()
        {
			projectile.width = 26;
			projectile.height = 26;
            Main.projFrames[projectile.type] = 1;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.timeLeft = 960;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.minion = true;
			projectile.alpha = 0;
            projectile.netImportant = true;
            projectile.minionSlots = 0f;
		}
		public int FindClosestEnemy()
		{
			Player player = Main.player[projectile.owner];
			float minDist = 600;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			
			for(int i = 0; i < Main.npc.Length - 1; i++)
			{
				NPC target = Main.npc[i];
				if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active)
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
			return target2;
		}
		int targetValue = 120;
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			
			Vector2 playerCursor;
			if (player.gravDir == 1f)
			{
			playerCursor.Y = (float)Main.mouseY + Main.screenPosition.Y;
			}
			else
			{
			playerCursor.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
			}
			playerCursor.X = (float)Main.mouseX + Main.screenPosition.X;
			
			float shootToX = playerCursor.X - projectile.Center.X;
			float shootToY = playerCursor.Y - projectile.Center.Y;
					
			projectile.tileCollide = true;
			if(FindClosestEnemy() == -1)
			{
				projectile.rotation = (float)Math.Atan2((double)shootToY, (double)shootToX) + MathHelper.ToRadians(45);
			}
			else if(player.whoAmI == Main.myPlayer)
			{
				projectile.ai[1] += 1;
				NPC target = Main.npc[FindClosestEnemy()];
				shootToX = target.Center.X - projectile.Center.X;
				shootToY = target.Center.Y - projectile.Center.Y;
				projectile.rotation = (float)Math.Atan2((double)shootToY, (double)shootToX) + MathHelper.ToRadians(45);
				if(projectile.ai[1] >= targetValue)
				{
					targetValue = Main.rand.Next(60,130);
					projectile.ai[1] = 0;
					LaunchLaser(target.Center);
				}
			}
			
        }
		public void LaunchLaser(Vector2 area)
		{
			Player player  = Main.player[projectile.owner];
			int laser = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BrightRedLaser"), projectile.damage, 0, projectile.owner);
			Main.projectile[laser].ai[0] = area.X;
			Main.projectile[laser].ai[1] = area.Y;
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
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X * 0.35f; 
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y * 0.35f;
				}
			return false;
		}
	}
}
		