using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class CataclysmDamage : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cataclysm Collapse");
		}
        public override void SetDefaults()
        {
            projectile.width = 196;
            projectile.height = 196;
			projectile.timeLeft = 12;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.tileCollide = false;
			projectile.melee = true;
            projectile.aiStyle = 0; 
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 60;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(2))
				target.AddBuff(BuffID.OnFire, 1200, false);
		}
		int randseed = -1;
		public override void AI() //The projectile's AI/ what the projectile does
		{
			if(randseed == -1)
			{
				Main.PlaySound(SoundID.Item14, projectile.Center);
				randseed = Main.rand.Next(360);
				for (int i = 0; i < 360; i += 5)
				{
					Vector2 circularLocation = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(i));
					int num = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, mod.DustType("CopyDust4"));
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
					int num = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, mod.DustType("CopyDust4"));
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
					int num = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, mod.DustType("CopyDust4"));
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