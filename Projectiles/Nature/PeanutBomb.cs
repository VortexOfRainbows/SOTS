using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Nature
{    
    public class PeanutBomb : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Peanut Bomb");
		}
		public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
			projectile.ranged = true;
			projectile.friendly = true;
            projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.timeLeft = 360;
			projectile.alpha = 0;
			projectile.penetrate = 1;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 16;
			height = 16;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override bool PreAI()
		{
			projectile.rotation += projectile.velocity.Y * 0.03f;
			projectile.velocity.X *= 0.99f;
			projectile.velocity.Y += 0.125f;
			projectile.velocity.Y *= 1.005f;
			if (projectile.Center.Y >= projectile.ai[0] - 32)
				projectile.tileCollide = true;
			return true;
		}
		public override void AI()
        {
			for(int i = 0; i < 3; i++)
			{
				float mult = i /3f;
				Vector2 circularPos = new Vector2(6, -6).RotatedBy(projectile.rotation) * projectile.scale;
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) + projectile.velocity * mult + circularPos - new Vector2(5), 0, 0, DustID.Fire);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].scale = projectile.scale + 0.2f;
			}
		}
		public override void Kill(int timeLeft)
		{
			if (Main.myPlayer == projectile.owner)
			{
				Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<PeanutExplosion>(), projectile.damage, projectile.knockBack, Main.myPlayer);
			}
			float RandMod = Main.rand.NextFloat(4);
			for (int i = 0; i < 10 + RandMod * 2; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, DustID.Fire);
				Main.dust[num1].velocity *= 1.8f * (0.2f + 0.7f * projectile.scale);
				Main.dust[num1].velocity.Y -= 1.5f;
				Main.dust[num1].scale = projectile.scale + 0.6f;
			}
			for (int i = 0; i < 10 + RandMod * 2; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, DustID.Dirt);
				Main.dust[num1].velocity *= 1.6f * (0.2f + 0.7f * projectile.scale);
				Main.dust[num1].velocity.Y -= 1.5f;
				Main.dust[num1].scale = projectile.scale * 0.8f + 0.5f;
			}
			for (int i = 0; i < 5 + RandMod; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, 7);
				Main.dust[num1].velocity *= 1.4f * (0.2f + 0.6f * projectile.scale);
				Main.dust[num1].velocity.Y -= 1.5f;
				Main.dust[num1].scale = projectile.scale * 0.8f + 0.5f;
			}
        }
	}
	public class PeanutExplosion : ModProjectile
	{
		bool runOnce = true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Peanut Bomb");
		}
		public override void SetDefaults()
		{
			projectile.height = 40;
			projectile.width = 40;
			Main.projFrames[projectile.type] = 4;
			projectile.penetrate = -1;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.timeLeft = 4;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			if (runOnce)
			{
				Main.PlaySound(SoundID.Item14, (int)projectile.Center.X, (int)projectile.Center.Y);
				for (int i = 0; i < 360; i += 15)
				{
					Vector2 circularLocation = new Vector2(-Main.rand.NextFloat(8, 14), 0).RotatedBy(MathHelper.ToRadians(i));
					int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 6);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale *= 1.75f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
				}
				runOnce = false;
			}
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)(projectile.position.X - projectile.width/2), (int)(projectile.position.Y - projectile.height/2), projectile.width * 2, projectile.height * 2);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 5;
			target.AddBuff(BuffID.OnFire, 120, false);
		}
	}
}
			