using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Evil
{    
    public class EvilStrike : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare Strike");
		}
        public override void SetDefaults()
		{
			projectile.height = 48;
			projectile.width = 48;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 2;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
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
				Main.dust[num1].color = VoidPlayer.EvilColor;
				Main.dust[num1].fadeIn = 0.2f;
			}
		}
	}
}
		