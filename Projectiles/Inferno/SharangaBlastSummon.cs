using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Inferno
{    
    public class SharangaBlastSummon : ModProjectile 
    {	
		int expand = -1;
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
			Projectile.height = 40;
			Projectile.width = 40;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.friendly = true;
			Projectile.timeLeft = 16;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}
		public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f);
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 2)
			{
				Projectile.friendly = false;
			}
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
			if(expand == -1)
			{
				SOTSUtils.PlaySound(SoundID.Item14, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
				expand = 0;
				for(int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(-14, 0).RotatedBy(MathHelper.ToRadians(i));
					
					int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.Torch);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale = 1.75f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
				}
			}
        }
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			hitbox = new Rectangle((int)(Projectile.position.X - Projectile.width), (int)(Projectile.position.Y - Projectile.height), Projectile.width * 3, Projectile.height * 3);
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 5;
			target.AddBuff(BuffID.OnFire, 1200, false);
        }
	}
}