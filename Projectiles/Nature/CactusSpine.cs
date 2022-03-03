using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace SOTS.Projectiles.Nature
{    
    public class CactusSpine : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus Spine");
		}
        public override void SetDefaults()
        {
			projectile.friendly = true;
			projectile.height = 10;
			projectile.width = 18;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 20;
			projectile.timeLeft = 50;
			projectile.extraUpdates = 2;
		}
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 17, 0.8f, 0.1f);
				runOnce = false;
            }
			projectile.alpha = (int)(255 * (1 - projectile.timeLeft / 50f));
			if(Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(4, 4), 0, 0, 2, 0, 0, 50 + (int)(205 * (1 - Math.Sin((1 - projectile.timeLeft / 50f) * MathHelper.Pi)))); //DustID.Grass
				dust.noGravity = true;
				dust.velocity *= 0.3f;
			}
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 720, false);
		}
	}
}
		