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
			Projectile.height = 30;
			Projectile.width = 30;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.netImportant = true;
			Projectile.ignoreWater = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		public override void AI()
		{
			Player player  = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (player.dead)
			{
				Projectile.Kill();
			}
			if ((modPlayer.tPlanetDamage + 1) != Projectile.damage)
			{
				Projectile.Kill();
			}
			if (Projectile.timeLeft > 100)
			{
				Projectile.timeLeft = 300;
			}
			Vector2 toPlayer = player.Center - Projectile.Center;
			float distance = toPlayer.Length();
			float speed = distance * 0.12f;
			if (speed < 12) speed = 12;

			Projectile.velocity = new Vector2(-speed, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - player.Center.Y, Projectile.Center.X - player.Center.X));
			if (distance < 256)
			{
				bool found = false;
				int ofTotal = 0;
				int total = 0;
				for(int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if(Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner)
					{
						if(proj == Projectile)
						{
							found = true;
						}
						if(!found)
							ofTotal++;
						total++;
					}
				}
				Vector2 rotateCenter = new Vector2(128, 0).RotatedBy(MathHelper.ToRadians(-modPlayer.orbitalCounter + (ofTotal * 360f / total)));
				rotateCenter += player.Center;
				Vector2 toRotate = rotateCenter - Projectile.Center;
				float dist2 = toRotate.Length();
				if(dist2 > 30)
				{
					dist2 = 30;
				}
				Projectile.velocity = new Vector2(-dist2, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - rotateCenter.Y, Projectile.Center.X - rotateCenter.X));
			}
		}
	}
}
		