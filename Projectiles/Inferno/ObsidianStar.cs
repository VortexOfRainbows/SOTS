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
			DisplayName.SetDefault("Hellfury Star");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(3);
            aiType = 3;
			projectile.width = 32;
			projectile.height = 32;
			projectile.penetrate = 1;
			
			projectile.tileCollide = false;
		}
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
            {
				Main.PlaySound(SoundID.Item20, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				runOnce = false;
            }
			//projectile.rotation += 1f;
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 32, 32, 6);
			
			if(projectile.Center.Y < projectile.ai[1])
			{
				projectile.tileCollide = true;
			}
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			hitbox = new Rectangle((int)(projectile.position.X - projectile.width/2), (int)(projectile.position.Y - projectile.height/2), projectile.width * 2, projectile.height * 2);
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 360; i += 8)
			{
				Vector2 circularLocation = new Vector2(-20, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 6);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 2.25f;
				Main.dust[num1].velocity = circularLocation * 0.35f;
			}
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("HellfuryCrush"), projectile.damage, 0, Main.myPlayer);
			}
		}
	}
}
		
			