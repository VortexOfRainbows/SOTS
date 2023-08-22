using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Crushers
{    
    public class HellbreakerCrusher : CrusherProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hellbreaker Crusher");
		}
        public override void SafeSetDefaults()
        {
			Projectile.height = 26;
			Projectile.width = 26;
			maxDamage = 6;
			chargeTime = 270;
			minExplosions = 2;
			maxExplosions = 4;
			explosiveRange = 48;
			releaseTime = 240;
			accSpeed = 0.5f;
			initialExplosiveRange = 48;
			exponentReduction = 0.475f;
		}
        public override void PostAI()
        {
			Player player = Main.player[Projectile.owner];
			for(int i = 0; i < arms.Length; i++)
            {
				Vector2 arm = arms[i];
				arm += player.Center;
				if (Main.rand.NextBool(3))
				{
					int num1 = Dust.NewDust(new Vector2(arm.X - Projectile.width / 2, arm.Y - Projectile.height / 2) - new Vector2(5), Projectile.width, Projectile.height, DustID.Torch);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale *= 2.75f;
					Main.dust[num1].velocity *= 0.35f;
				}
			}
			if (Main.rand.NextBool(3))
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, DustID.Torch);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale *= 2.75f;
				Main.dust[num1].velocity *= 0.15f;
				Main.dust[num1].velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 4;
			}
			base.PostAI();
        }
        public override int ExplosionType()
		{
			return ModContent.ProjectileType<HellCrush>();
		}
		public override Texture2D ArmTexture(int handNum, int direction)
		{
			return Mod.Assets.Request<Texture2D>("Projectiles/Crushers/HellbreakerArm").Value;
		}
	}
}