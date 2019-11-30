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
using SOTS.Void;

namespace SOTS.Projectiles.Celestial
{    
    public class CeremonialSlash : ModProjectile 
    {	int ai1 = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ceremonial Slash");
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(595);
            aiType = 595;
			Main.projFrames[projectile.type] = Main.projFrames[595];  
			projectile.width = 96;
		}
		public override void AI()
        {
			Player player  = Main.player[projectile.owner];
			Vector2 cursorArea;
			if (player.gravDir == 1f)
			{
				cursorArea.Y = (float)Main.mouseY + Main.screenPosition.Y;
			}
			else
			{
				cursorArea.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
			}
			cursorArea.X = (float)Main.mouseX + Main.screenPosition.X;
			ai1++;
			
			float difX = cursorArea.X - projectile.Center.X;
			float difY = cursorArea.Y - projectile.Center.Y;
			float dir = (float)Math.Atan2((double)difY,(double)difX);
			
			if(ai1 % 6 == 0)
			{
				Vector2 area = new Vector2(1048f, 0).RotatedBy(dir);
				area.X = projectile.Center.X + area.X;
				area.Y = projectile.Center.Y + area.Y;
				
				if(player.whoAmI == Main.myPlayer)
				LaunchLaser(area);
			}
			if(ai1 % 4 == 0)
			{
			VoidPlayer.ModPlayer(player).voidMeter--;
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 2;
        }
		public void LaunchLaser(Vector2 area)
		{
			Player player  = Main.player[projectile.owner];
			int Probe = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("RedLaser"), projectile.damage, 0, projectile.owner);
			Main.projectile[Probe].ai[0] = area.X;
			Main.projectile[Probe].ai[1] = area.Y;
		}
	}
}
		
			