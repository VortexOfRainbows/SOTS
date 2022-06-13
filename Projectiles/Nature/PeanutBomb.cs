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
            Projectile.width = 20;
            Projectile.height = 20;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
            Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 360;
			Projectile.alpha = 0;
			Projectile.penetrate = 1;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 16;
			height = 16;
            return true;
        }
        public override bool PreAI()
		{
			Projectile.rotation += Projectile.velocity.Y * 0.03f;
			Projectile.velocity.X *= 0.99f;
			Projectile.velocity.Y += 0.125f;
			Projectile.velocity.Y *= 1.005f;
			if (Projectile.Center.Y >= Projectile.ai[0] - 32)
				Projectile.tileCollide = true;
			return true;
		}
		public override void AI()
        {
			for(int i = 0; i < 3; i++)
			{
				float mult = i /3f;
				Vector2 circularPos = new Vector2(6, -6).RotatedBy(Projectile.rotation) * Projectile.scale;
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + Projectile.velocity * mult + circularPos - new Vector2(5), 0, 0, DustID.Torch);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].scale = Projectile.scale + 0.2f;
			}
		}
		public override void Kill(int timeLeft)
		{
			if (Main.myPlayer == Projectile.owner)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PeanutExplosion>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
			}
			float RandMod = Main.rand.NextFloat(4);
			for (int i = 0; i < 10 + RandMod * 2; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, DustID.Torch);
				Main.dust[num1].velocity *= 1.8f * (0.2f + 0.7f * Projectile.scale);
				Main.dust[num1].velocity.Y -= 1.5f;
				Main.dust[num1].scale = Projectile.scale + 0.6f;
			}
			for (int i = 0; i < 10 + RandMod * 2; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, DustID.Dirt);
				Main.dust[num1].velocity *= 1.6f * (0.2f + 0.7f * Projectile.scale);
				Main.dust[num1].velocity.Y -= 1.5f;
				Main.dust[num1].scale = Projectile.scale * 0.8f + 0.5f;
			}
			for (int i = 0; i < 5 + RandMod; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, 7);
				Main.dust[num1].velocity *= 1.4f * (0.2f + 0.6f * Projectile.scale);
				Main.dust[num1].velocity.Y -= 1.5f;
				Main.dust[num1].scale = Projectile.scale * 0.8f + 0.5f;
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
			Projectile.height = 40;
			Projectile.width = 40;
			Main.projFrames[Projectile.type] = 4;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.timeLeft = 4;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
		}
		public override void AI()
		{
			if (runOnce)
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
				for (int i = 0; i < 360; i += 15)
				{
					Vector2 circularLocation = new Vector2(-Main.rand.NextFloat(8, 14), 0).RotatedBy(MathHelper.ToRadians(i));
					int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 6);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale *= 1.75f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
				}
				runOnce = false;
			}
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)(Projectile.position.X - Projectile.width/2), (int)(Projectile.position.Y - Projectile.height/2), Projectile.width * 2, Projectile.height * 2);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 5;
			target.AddBuff(BuffID.OnFire, 120, false);
		}
	}
}
			