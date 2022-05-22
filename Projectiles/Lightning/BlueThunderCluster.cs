using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Lightning
{    
    public class BlueThunderCluster : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Cluster");
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(263);
            AIType = 263; 
			Projectile.height = 58;
			Projectile.width = 58;
			Projectile.penetrate = 24;
			Projectile.friendly = true;
			Projectile.timeLeft = 150;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
		}
		public override void AI()
		{
			Projectile.alpha++;
			for(int i = 0; i < 360; i += 15)
			{
				Vector2 circularLocation = new Vector2(-28, 0).RotatedBy(MathHelper.ToRadians(i));
			
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 56);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].alpha = Projectile.alpha;
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 15;
        }
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item94, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
			if(Projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 8; i++)
                {
					Vector2 circularVelocity = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(i * 45));
					Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, circularVelocity.X, circularVelocity.Y, Mod.Find<ModProjectile>("BlueLightning").Type, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f);
				}
			}
		}
	}
}
		