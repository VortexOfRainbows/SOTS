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
			// DisplayName.SetDefault("Flower Strike"); //do you enjoy how all of my netsyncing is done via projectiles?
		}
        public override void SetDefaults()
		{
			Projectile.height = 48;
			Projectile.width = 48;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		public override bool? CanHitNPC(NPC target)
        {
            return Projectile.ai[0] == target.whoAmI;
        }
        public override void AI()
		{
			for(int i = 0; i < 360; i += 30)
			{
				Vector2 circularLocation = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
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
		