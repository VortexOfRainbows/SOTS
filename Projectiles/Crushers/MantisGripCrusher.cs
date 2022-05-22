using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Nature;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class MantisGripCrusher : CrusherProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mantis Grip Crusher");
		}
        public override void SafeSetDefaults()
        {
			Projectile.height = 22;
			Projectile.width = 22;
			maxDamage = 5;
			minDamage = -0.2f;
			chargeTime = 240;
			minExplosions = 0;
			maxExplosions = 5;
			explosiveRange = 64;
			initialExplosiveRange = 44;
			releaseTime = 120;
			accSpeed = 0.5f;
			initialExplosiveRange = 48;
			exponentReduction = 0.5f;
			minTimeBeforeRelease = 14;
			armAngle = 60f;
		}
        public override void ExplosionSound()
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 92, 0.55f, 0.4f);
			base.ExplosionSound();
		}
        public override bool UseCustomExplosionEffect(float x, float y, float dist, float rotation, float chargePercent, int index)
		{
			float randRot = Main.rand.NextFloat(90f);
			for (int i = 0; i < 4; i++)
			{
				int direction = Main.rand.Next(2) * 2 - 1;
				float rand = Main.rand.NextFloat(5 + index * 6.5f, 45) * direction * (0.5f + chargePercent * 0.5f);
				float randMult = Main.rand.NextFloat(0.8f, 1.2f);
				randMult *= 0.5f + 0.5f * (float)Math.Cos(rand);
				Vector2 velo = new Vector2(-(i + index * 3.5f + chargePercent * 1.5f)* randMult, 0).RotatedBy(rotation + MathHelper.ToRadians(rand));
				Vector2 spawnAt = new Vector2(x, y) + Main.rand.NextVector2Circular(Main.rand.NextFloat(32 + index * 8) + i * 8 + index * 2, 0).RotatedBy(MathHelper.ToRadians(randRot + 90 * i + Main.rand.NextFloat(-15, 15))) + velo.SafeNormalize(Vector2.Zero) * 12 * (i - 1);
				Projectile.NewProjectileDirect(Projectile.Center, velo, ModContent.ProjectileType<NatureBoltFriendly>(), Projectile.damage, Projectile.knockBack, Projectile.owner, spawnAt.X, spawnAt.Y);
			}
			return true;
		}
		public override void PostAI()
        {
			//Player player = Main.player[Projectile.owner];
			Vector2 velo = Projectile.velocity.SafeNormalize(Vector2.Zero) * 4;
			Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4) + velo * 2, 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 100, default, 1.6f);
			dust.velocity *= 0.05f;
			dust.scale = 1.25f;
			dust.noGravity = true;
			dust.color = VoidPlayer.natureColor;
			dust.noGravity = true;
			dust.fadeIn = 0.2f;
			dust.velocity += velo;
			base.PostAI();
        }
		public override Texture2D ArmTexture(int handNum, int direction)
		{
			return Mod.Assets.Request<Texture2D>("Projectiles/Crushers/MantisGripArm").Value;
		}
	}
}