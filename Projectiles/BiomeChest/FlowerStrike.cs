using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.BiomeChest
{    
    public class FlowerStrike : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flower Strike"); //do you enjoy how all of my netsyncing is done via projectiles?
		}
        public override void SetDefaults()
		{
			projectile.height = 48;
			projectile.width = 48;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 40;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		public override bool? CanHitNPC(NPC target)
        {
            return projectile.ai[0] == target.whoAmI;
        }
        public override void AI()
		{
			for(int i = 0; i < 360; i += 30)
			{
				Vector2 circularLocation = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.75f;
				Main.dust[num1].scale *= 1.45f;
				Main.dust[num1].alpha = 100;
				Main.dust[num1].color = new Color(120, 30, 50, 0);
				Main.dust[num1].fadeIn = 0.2f;
			}
		}
	}
}
		