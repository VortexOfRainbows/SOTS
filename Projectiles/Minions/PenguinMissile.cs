using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Minions
{    
    public class PenguinMissile : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Penguin Missile");
			Main.projFrames[projectile.type] = 18;
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(ProjectileID.RocketSnowmanI);
            aiType = ProjectileID.RocketSnowmanI;
			projectile.alpha = 0;
			projectile.penetrate = 1; 
			projectile.ranged = false;
			projectile.minion = true;
			projectile.width = 24;
			projectile.height = 44;
			projectile.tileCollide = false;
			projectile.timeLeft = 1800;
			projectile.friendly = true;
			projectile.hostile = false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 3;
			target.AddBuff(BuffID.OnFire, 240, false);
		}
		public void genGore()
		{
			for (int i = 0; i < 4; i++)
			{
				int speedX = Main.rand.Next(7);
				if (Main.rand.Next(5) <= 1)
				{
					Gore.NewGore(new Vector2(projectile.Center.X - 12, projectile.Center.Y - 22), new Vector2(speedX, 0).RotatedByRandom(MathHelper.ToRadians(360)) + projectile.velocity * 0.12f, mod.GetGoreSlot("Gores/PenguinMissileGore" + (i + 1)), projectile.scale);
					int goreIndex = Gore.NewGore(new Vector2(projectile.Center.X - 12, projectile.Center.Y - 22), new Vector2(speedX, 0).RotatedByRandom(MathHelper.ToRadians(360)) + projectile.velocity * 0.12f, Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 0.65f;
				}
				if (Main.rand.Next(5) <= 1)
				{
					if (i == 0 || i == 2)
						Gore.NewGore(new Vector2(projectile.Center.X - 12, projectile.Center.Y - 22), new Vector2(speedX, 0).RotatedByRandom(MathHelper.ToRadians(360)) + projectile.velocity * 0.12f, mod.GetGoreSlot("Gores/PenguinMissileGore" + (i + 1)), projectile.scale);
				}
			}
			Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 14, 0.45f);
		}
		public override void Kill(int timeLeft)
		{
			genGore();
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("SharangaBlastSummon"), projectile.damage, 0, Main.myPlayer);
			}
		}
		public override void AI()
		{
			projectile.frameCounter++;
			if(projectile.frameCounter % 2 == 0)
			{
				projectile.frame = (projectile.frame + 1) % 18;
			}
		}		
	}
}
		