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
			DisplayName.SetDefault("Webbing");
		}
        public override void SetDefaults()
        {
			projectile.height = 78;
			projectile.width = 76;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 120;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 120;
			projectile.hide = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
			target.AddBuff(ModContent.BuffType<WebbedNPC>(), projectile.timeLeft);
		}
		bool runOnce = true;
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = (int)(projectile.width * projectile.scale);
			int height = (int)(projectile.width * projectile.scale);
			hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - height / 2, width, height);
		}
		float rotation = 180;
		bool hasPassed = false;
        public override bool PreAI()
		{
			if (runOnce)
			{
				runOnce = false;
				if (projectile.ai[0] == -1)
				{
					projectile.extraUpdates = 2;
				}
				projectile.scale = 0.5f;
				projectile.hide = false;
			}
			return true;
        }
        public override void AI()
        {
			rotation *= 0.95f;
			projectile.rotation = MathHelper.ToRadians(rotation);
			if (projectile.scale < 1.0 && !hasPassed)
			{
				projectile.scale += 0.01f;
				projectile.scale *= 1.05f;
			}
			else if (projectile.scale > 1.0)
			{
				hasPassed = true;
				projectile.scale *= 0.98f;
				projectile.scale -= 0.005f;
			}
			if(projectile.timeLeft < 65)
			{
				hasPassed = true;
				projectile.scale -= 0.0025f;
				projectile.alpha += 4;
            }
        }
	}
}
		