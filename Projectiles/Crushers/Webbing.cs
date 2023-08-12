using Microsoft.Xna.Framework;
using SOTS.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class Webbing : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Webbing");
		}
        public override void SetDefaults()
        {
			Projectile.height = 78;
			Projectile.width = 76;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 120;
			Projectile.hide = true;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
			target.AddBuff(ModContent.BuffType<WebbedNPC>(), Projectile.timeLeft);
		}
		bool runOnce = true;
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = (int)(Projectile.width * Projectile.scale);
			int height = (int)(Projectile.width * Projectile.scale);
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - height / 2, width, height);
		}
		float rotation = 180;
		bool hasPassed = false;
        public override bool PreAI()
		{
			if (runOnce)
			{
				runOnce = false;
				if (Projectile.ai[0] == -1)
				{
					Projectile.extraUpdates = 2;
				}
				Projectile.scale = 0.5f;
				Projectile.hide = false;
			}
			return true;
        }
        public override void AI()
        {
			rotation *= 0.95f;
			Projectile.rotation = MathHelper.ToRadians(rotation);
			if (Projectile.scale < 1.0 && !hasPassed)
			{
				Projectile.scale += 0.01f;
				Projectile.scale *= 1.05f;
			}
			else if (Projectile.scale > 1.0)
			{
				hasPassed = true;
				Projectile.scale *= 0.98f;
				Projectile.scale -= 0.005f;
			}
			if(Projectile.timeLeft < 65)
			{
				hasPassed = true;
				Projectile.scale -= 0.0025f;
				Projectile.alpha += 4;
            }
        }
	}
}
		