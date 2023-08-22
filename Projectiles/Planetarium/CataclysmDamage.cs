using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Planetarium
{    
    public class CataclysmDamage : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cataclysm Collapse");
		}
        public override void SetDefaults()
        {
            Projectile.width = 196;
            Projectile.height = 196;
			Projectile.timeLeft = 12;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = 0; 
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(2))
				target.AddBuff(BuffID.OnFire, 1200, false);
		}
		int randseed = -1;
		public override void AI() //The projectile's AI/ what the projectile does
		{
			if(randseed == -1)
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
				randseed = Main.rand.Next(360);
				for (int i = 0; i < 360; i += 5)
				{
					Vector2 circularLocation = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(i));
					int num = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
					Dust dust = Main.dust[num];
					dust.velocity = circularLocation;
					dust.color = new Color(220, 60, 10, 40);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 3f;
				}
				for (int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(14, 0).RotatedBy(MathHelper.ToRadians(i));
					circularLocation.X /= 2;
					circularLocation = circularLocation.RotatedBy(MathHelper.ToRadians(randseed));
					int num = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
					Dust dust = Main.dust[num];
					dust.velocity = circularLocation;
					dust.color = new Color(220, 60, 10, 40);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 3f;
				}
				for (int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(14, 0).RotatedBy(MathHelper.ToRadians(i));
					circularLocation.X /= 2;
					circularLocation = circularLocation.RotatedBy(MathHelper.ToRadians(randseed + 90));
					int num = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
					Dust dust = Main.dust[num];
					dust.velocity = circularLocation;
					dust.color = new Color(220, 60, 10, 40);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 3f;
				}
			}
		}
	}
}