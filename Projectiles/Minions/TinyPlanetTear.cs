using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{    
    public class TinyPlanetTear : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Planet Tear");
		}
        public override void SetDefaults()
        {
			projectile.height = 30;
			projectile.width = 30;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 300;
			projectile.tileCollide = false;
			projectile.melee = true;
			projectile.hostile = false;
			projectile.netImportant = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}
		public override void AI()
		{
			Player player  = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (player.dead)
			{
				projectile.Kill();
			}
			if (projectile.timeLeft > 100)
			{
				projectile.timeLeft = 300;
			}
			Vector2 toPlayer = player.Center - projectile.Center;
			float distance = toPlayer.Length();
			float speed = distance * 0.12f;
			if (speed < 12) speed = 12;

			projectile.velocity = new Vector2(-speed, 0).RotatedBy(Math.Atan2(projectile.Center.Y - player.Center.Y, projectile.Center.X - player.Center.X));
			if (distance < 256)
			{
				bool found = false;
				int ofTotal = 0;
				int total = 0;
				for(int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if(projectile.type == proj.type && proj.active && projectile.active && proj.owner == projectile.owner)
					{
						if(proj == projectile)
						{
							found = true;
						}
						if(!found)
							ofTotal++;
						total++;
					}
				}
				Vector2 rotateCenter = new Vector2(128, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (ofTotal * 360f / total)));
				rotateCenter += player.Center;
				Vector2 toRotate = rotateCenter - projectile.Center;
				float dist2 = toRotate.Length();
				if(dist2 > 30)
				{
					dist2 = 30;
				}
				projectile.velocity = new Vector2(-dist2, 0).RotatedBy(Math.Atan2(projectile.Center.Y - rotateCenter.Y, projectile.Center.X - rotateCenter.X));
			}
		}
	}
}
		