using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Lightning
{    
    public class PurpleThunderCluster : ModProjectile 
    {	
		float distance = 30f;  
		int rotation = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Cluster");
			
		}
		
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(263);
            AIType = 263; 
			Projectile.height = 43;
			Projectile.width = 43;
			Projectile.penetrate = 24;
			Projectile.friendly = true;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
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
			Player player = Main.player[Projectile.owner];
			Vector2 circularLocation = new Vector2(Projectile.velocity.X -distance, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotation));
			rotation += 15;
			distance -= 0.25f;
			Projectile.scale *= 0.99f;
			Projectile.alpha++;
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 173);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 15;
        }
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			
			Vector2 cursorArea = Main.MouseWorld;
			
			float shootToX = cursorArea.X - Projectile.Center.X;
			float shootToY = cursorArea.Y - Projectile.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
	
			distance = 1.45f / distance;
		
			shootToX *= distance * 5;
			shootToY *= distance * 5;
		
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item94, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
			if(Projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 3; i++)
					Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, shootToX, shootToY, Mod.Find<ModProjectile>("PurpleLightning").Type, Projectile.damage, Projectile.knockBack, Main.myPlayer, 4.25f, 0f);
			}
		}
	}
}
		