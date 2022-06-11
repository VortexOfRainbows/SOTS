using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Inferno
{    
    public class SharangaBlast : ModProjectile 
    {
		bool runOnce = true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharanga Crush");
		}
        public override void SetDefaults()
        {
			Projectile.height = 40;
			Projectile.width = 40;
            Main.projFrames[Projectile.type] = 4;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.timeLeft = 16;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
		}
		public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
			if(runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item14, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
				for(int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(-14, 0).RotatedBy(MathHelper.ToRadians(i));
					int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 6);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale = 1.75f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
				}
				runOnce = false;
			}
        }
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			hitbox = new Rectangle((int)(Projectile.position.X - Projectile.width), (int)(Projectile.position.Y - Projectile.height), Projectile.width * 3, Projectile.height * 3);
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 10;
			target.AddBuff(BuffID.OnFire, 180, false);
        }
	}
}