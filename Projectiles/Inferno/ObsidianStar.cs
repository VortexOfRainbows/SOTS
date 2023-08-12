using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Inferno
{    
    public class ObsidianStar : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hellfury Star");
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(3);
            AIType = 3;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
		}
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
            {
				SOTSUtils.PlaySound(SoundID.Item20, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
				runOnce = false;
            }
			//Projectile.rotation += 1f;
			Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 32, 32, 6);
			if(Projectile.Center.Y < Projectile.ai[1])
			{
				Projectile.tileCollide = true;
			}
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			hitbox = new Rectangle((int)(Projectile.position.X - Projectile.width/2), (int)(Projectile.position.Y - Projectile.height/2), Projectile.width * 2, Projectile.height * 2);
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 360; i += 8)
			{
				Vector2 circularLocation = new Vector2(-20, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 6);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 2.25f;
				Main.dust[num1].velocity = circularLocation * 0.35f;
			}
			if(Projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<HellfuryCrush>(), Projectile.damage, 0, Main.myPlayer);
			}
		}
	}
}
		
			