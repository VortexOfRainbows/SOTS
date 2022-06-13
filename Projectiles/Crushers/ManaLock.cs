using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SOTS.Projectiles.Base;

namespace SOTS.Projectiles.Crushers
{    
    public class ManaLock : ModProjectile 
    {	float distance = 30f;  
		int rotation = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Lock");
			
		}
		
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(263);
            AIType = 263; 
			Projectile.height = 43;
			Projectile.width = 43;
			Projectile.penetrate = 1;
			Projectile.friendly = false;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.hostile = false;
			Projectile.alpha = 255;
		}
		public override void AI()
		{
			Vector2 circularLocation = new Vector2(Projectile.velocity.X -distance, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotation));
			rotation += 15;
			distance -= 0.5f;
			Projectile.velocity *= 0.98f;
			Projectile.scale *= 0.98f;
			Projectile.alpha++;
			
			Player player  = Main.player[Projectile.owner];
			
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 15);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			Main.dust[num1].scale = 1.5f;
		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			if(Projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<HealProj>(), 1, 0, player.whoAmI, (int)Projectile.ai[0], 3);	
			}
			for(int i = 5; i > 0; i --)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X , Projectile.position.Y), Projectile.width, Projectile.height, 15);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.5f;
				Main.dust[num1].scale = 1.5f;
			}
		}
	}
}
		