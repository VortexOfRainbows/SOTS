using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;

namespace SOTS.Projectiles.Lightning
{    
    public class GreenThunderCluster : ModProjectile 
    {	float distance = 30f;  
		int rotation = 0;
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Thunder Cluster");
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(263);
            AIType = 263; 
			Projectile.height = 43;
			Projectile.width = 43;
			Projectile.penetrate = 24;
			Projectile.friendly = true;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.hostile = false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
		public override void AI()
		{
			Vector2 circularLocation = new Vector2(Projectile.velocity.X -distance, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotation));
			rotation += 15;
			distance -= 0.5f;
			Projectile.scale *= 0.98f;
			Projectile.alpha++;
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 107);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 15;
        }
		public override void Kill(int timeLeft)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item94, Projectile.Center);
			if (Projectile.owner == Main.myPlayer)
			{
				Player player = Main.player[Projectile.owner];
				Vector2 cursorArea = Main.MouseWorld;
				float shootToX = cursorArea.X - Projectile.Center.X;
				float shootToY = cursorArea.Y - Projectile.Center.Y;
				float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
				distance = 6.25f / distance;
				shootToX *= distance * 5;
				shootToY *= distance * 5;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, shootToX, shootToY, ModContent.ProjectileType<GreenLightning>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 5f);
			}
		}
	}
}
		