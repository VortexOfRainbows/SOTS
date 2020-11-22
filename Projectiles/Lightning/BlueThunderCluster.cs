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
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 58;
			projectile.width = 58;
			projectile.penetrate = 24;
			projectile.friendly = true;
			projectile.timeLeft = 150;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.magic = true;
		}
		public override void AI()
		{
			projectile.alpha++;
			for(int i = 0; i < 360; i += 15)
			{
				Vector2 circularLocation = new Vector2(-28, 0).RotatedBy(MathHelper.ToRadians(i));
			
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 56);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].alpha = projectile.alpha;
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 15;
        }
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			Main.PlaySound(SoundID.Item94, (int)(projectile.Center.X), (int)(projectile.Center.Y));
			if(projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 8; i++)
                {
					Vector2 circularVelocity = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(i * 45));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, circularVelocity.X, circularVelocity.Y, mod.ProjectileType("BlueLightning"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
				}
			}
		}
	}
}
		