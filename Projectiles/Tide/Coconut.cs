using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Tide
{    
    public class Coconut : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut");
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(3))
			{
				target.AddBuff(BuffID.OnFire, 600); //fire for 10 seconds
			}
		}
		public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18; 
			projectile.friendly = true;
            projectile.hostile = false; 
			projectile.timeLeft = 60;
			projectile.alpha = 0;
			projectile.penetrate = 1;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 14;
			height = 14;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			return false;
        }
		bool runOnce = true;
        public override bool PreAI()
		{
			if(runOnce)
            {
				if (projectile.ai[0] >= 3)
				{
					projectile.friendly = true;
				}
				else
                {
					projectile.friendly = false;
                }
				runOnce = false;
				projectile.ai[1] = Main.rand.Next(24);
				projectile.netUpdate = true;
            }
			float scaleMod = projectile.ai[0];
			if (scaleMod < 0) scaleMod = 0;

			projectile.scale = 0.6f + 0.5f * scaleMod / 3f;
			projectile.rotation += projectile.velocity.X * 0.1f;
			if(projectile.timeLeft < 30 + projectile.ai[1] - (scaleMod / 3 * 20))
            {
				projectile.Kill();
            }
			projectile.velocity.X *= 0.99f;
			projectile.velocity.Y += 0.09f * projectile.scale;
			return true;
		}
		public override void AI()
        {
			for(int i = 0; i < 2; i++)
			{
				Vector2 circularPos = new Vector2(7, -7).RotatedBy(projectile.rotation) * projectile.scale;
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) + circularPos - new Vector2(5), 0, 0, DustID.Fire);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].scale = projectile.scale + 0.2f;
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item14, (int)projectile.Center.X, (int)projectile.Center.Y);
			Player owner = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)owner.GetModPlayer(mod, "SOTSPlayer");
			int RandMod = (int)projectile.ai[0];
			for (int i = 0; i < 10 + RandMod * 2; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, DustID.Fire);
				Main.dust[num1].velocity *= 1.8f * (0.2f + 0.7f * projectile.scale);
				Main.dust[num1].scale = projectile.scale + 0.6f;
			}
			for (int i = 0; i < 10 + RandMod * 2; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, 212);
				Main.dust[num1].velocity *= 1.6f * (0.2f + 0.7f * projectile.scale);
				Main.dust[num1].scale = projectile.scale * 0.8f + 0.5f;
			}
			for (int i = 0; i < 5 + RandMod; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, DustID.t_BorealWood);
				Main.dust[num1].velocity *= 1.4f * (0.2f + 0.6f * projectile.scale);
				Main.dust[num1].scale = projectile.scale * 0.8f + 0.5f;
			}
			if (projectile.owner == Main.myPlayer)
			{
				Vector2 randVelo = new Vector2(Main.rand.NextFloat(1, 3 * projectile.scale), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
				randVelo += projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(-10, 3) * 0.2f;
				randVelo.Y -= 1.4f;
				if (projectile.ai[0] > 0 || Main.rand.NextBool(20 - RandMod))
				{
					Projectile.NewProjectile(projectile.Center, randVelo, projectile.type, projectile.damage, projectile.knockBack * 0.8f, projectile.owner, projectile.ai[0] - 1f);
					bool triple = false;
					if ((projectile.ai[0] > 2) || Main.rand.NextBool(40 - RandMod))
					{
						triple = true;
						Projectile.NewProjectile(projectile.Center, randVelo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-90, 90) + 240)) * Main.rand.NextFloat(0.8f, 1.2f), projectile.type, projectile.damage, projectile.knockBack * 0.6f, projectile.owner, projectile.ai[0] - 1f);
					}
					if ((projectile.ai[0] > 1 && Main.rand.NextBool(3)) || Main.rand.NextBool(30 - RandMod))
					{
						float num = 180;
						if (!triple)
							num = 120;
						Projectile.NewProjectile(projectile.Center, randVelo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-90, 90) + num)) * Main.rand.NextFloat(0.8f, 1.2f), projectile.type, projectile.damage, projectile.knockBack * 0.7f, projectile.owner, projectile.ai[0] - 1);
					}
				}
				for(int i = 0; i < Main.rand.Next(1, 4); i++)
				{
					Projectile.NewProjectile(projectile.Center, new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360))) * Main.rand.NextFloat(0.8f, 1.2f) + new Vector2(0, -3), ModContent.ProjectileType<CoconutShrapnel>(), projectile.damage, projectile.knockBack * 0.7f, projectile.owner);
				}
			}		
        }
	}
}
			