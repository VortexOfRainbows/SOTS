using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Prim.Trails;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Projectiles.Tide
{    
    public class Rainbolt : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rain Bolt");
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
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(140, 90, 160, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				Vector2 circular = new Vector2(0, 2).RotatedBy(k * 1.57f + projectile.rotation + MathHelper.ToRadians(counter * 5f));
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1) + new Vector2(0, projectile.gfxOffY) + circular,
				null, color * (1f - (projectile.alpha / 255f)), projectile.rotation + circular.ToRotation(), drawOrigin, 1.1f, SpriteEffects.None, 0f);
			}
			return false;
        }
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		int counter;
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				SOTS.primitives.CreateTrail(new WaterTrail(projectile));
				runOnce = false;
			}
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (player.dead)
			{
				projectile.Kill();
			}
			if ((modPlayer.aqueductDamage + 1) != projectile.damage)
			{
				projectile.Kill();
			}
			if (projectile.timeLeft > 100)
			{
				projectile.timeLeft = 300;
			}
			Vector2 toPlayer = player.Center - projectile.Center;
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
			initialLoop.X *= 0.5f;
			Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (ofTotal * 360f / total)));
			projectile.Center = player.Center + properLoop;
			projectile.velocity = new Vector2(-1, 0).RotatedBy(toPlayer.ToRotation());
			for (int i = -1; i <= 1; i += 2)
			{
				if(Main.rand.NextBool(3))
				{
					Vector2 circular = new Vector2((float)Math.Sin(MathHelper.ToRadians(counter * 2.2f)) * 8 * i, 0).RotatedBy(projectile.velocity.ToRotation());
					Dust dust = Dust.NewDustPerfect(projectile.Center + circular, 172);
					dust.scale = dust.scale * 0.3f + 1.0f;
					dust.noGravity = true;
					dust.velocity = dust.velocity * 0.5f + projectile.velocity * i;
				}
			}
			counter++;
		}
	}
}
		