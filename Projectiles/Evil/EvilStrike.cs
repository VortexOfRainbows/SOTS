using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Evil
{    
    public class EvilStrike : ModProjectile 
    {	
        public override void SetDefaults()
		{
			Projectile.height = 48;
			Projectile.width = 48;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 2;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.DisableCrit();
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
				Main.dust[num1].color = ColorHelpers.EvilColor;
				Main.dust[num1].fadeIn = 0.2f;
			}
		}
	}
}
		