using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Projectiles.Inferno;

namespace SOTS.Projectiles.Minions
{    
    public class PenguinMissile : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Penguin Missile");
			Main.projFrames[Projectile.type] = 18;
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.RocketSnowmanI);
            AIType = ProjectileID.RocketSnowmanI;
			Projectile.alpha = 0;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.width = 24;
			Projectile.height = 44;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 1800;
			Projectile.friendly = true;
			Projectile.hostile = false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 3;
			target.AddBuff(BuffID.OnFire, 240, false);
		}
		public void genGore()
		{
			for (int i = 0; i < 4; i++)
			{
				int speedX = Main.rand.Next(7);
				if (Main.rand.Next(5) <= 1)
				{
					Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 22), new Vector2(speedX, 0).RotatedByRandom(MathHelper.ToRadians(360)) + Projectile.velocity * 0.12f, ModGores.GoreType("Gores/PenguinMissileGore" + (i + 1)), Projectile.scale);
					int goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 22), new Vector2(speedX, 0).RotatedByRandom(MathHelper.ToRadians(360)) + Projectile.velocity * 0.12f, Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 0.65f;
				}
				if (Main.rand.Next(5) <= 1)
				{
					if (i == 0 || i == 2)
						Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 22), new Vector2(speedX, 0).RotatedByRandom(MathHelper.ToRadians(360)) + Projectile.velocity * 0.12f, ModGores.GoreType("Gores/PenguinMissileGore" + (i + 1)), Projectile.scale);
				}
			}
			SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.45f);
		}
		public override void Kill(int timeLeft)
		{
			genGore();
			if(Projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<SharangaBlastSummon>(), Projectile.damage, 0, Main.myPlayer);
			}
		}
		public override void AI()
		{
			Projectile.frameCounter++;
			if(Projectile.frameCounter % 2 == 0)
			{
				Projectile.frame = (Projectile.frame + 1) % 18;
			}
		}		
	}
}
		