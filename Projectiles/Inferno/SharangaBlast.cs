using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Inferno
{    
    public class SharangaBlast : ModProjectile 
    {	int expand = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharanga Crush");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 40;
			projectile.width = 40;
            Main.projFrames[projectile.type] = 4;
			projectile.penetrate = -1;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.timeLeft = 16;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
		}
		public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f);
			
			
            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 4;
            }
			if(expand == -1)
			{
				Main.PlaySound(SoundID.Item14, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				expand = 0;
				for(int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(-14, 0).RotatedBy(MathHelper.ToRadians(i));
					
					int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 6);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale = 1.75f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
				}
			}
        }
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			hitbox = new Rectangle((int)(projectile.position.X - projectile.width), (int)(projectile.position.Y - projectile.height), projectile.width * 3, projectile.height * 3);
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
            target.immune[projectile.owner] = 10;
			target.AddBuff(BuffID.OnFire, 180, false);
        }
	}
}