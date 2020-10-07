using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SOTS.Projectiles.Minions
{    
    public class Rainbolt : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Bolt");
		}
        public override void SetDefaults()
        {
			projectile.width = 12;
			projectile.height = 12;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 300;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.netImportant = true;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
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
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (projectile.type == proj.type && proj.active && projectile.active && proj.owner == projectile.owner)
					{
						if (proj == projectile)
						{
							found = true;
						}
						if (!found)
							ofTotal++;
						total++;
					}
				}
				Vector2 initialLoop = new Vector2(128, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 4));
				initialLoop.X /= 2.0f;
				Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (ofTotal * 360f / total)));
				projectile.position.X = properLoop.X + player.Center.X - projectile.width / 2;
				projectile.position.Y = properLoop.Y + player.Center.Y - projectile.height / 2;
			}
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 10, 10, 172, 0, 0, 100);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 1.0f;
			num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 10, 10, 172, 0, 0, 100, default, 2f);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
	}
}
		