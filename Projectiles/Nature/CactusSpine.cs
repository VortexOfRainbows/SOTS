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
			Projectile.friendly = true;
			Projectile.height = 10;
			Projectile.width = 18;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
			Projectile.timeLeft = 50;
			Projectile.extraUpdates = 2;
		}
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 17, 0.8f, 0.1f);
				runOnce = false;
            }
			Projectile.alpha = (int)(255 * (1 - Projectile.timeLeft / 50f));
			if(Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, 2, 0, 0, 50 + (int)(205 * (1 - Math.Sin((1 - Projectile.timeLeft / 50f) * MathHelper.Pi)))); //DustID.Grass
				dust.noGravity = true;
				dust.velocity *= 0.3f;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 720, false);
		}
	}
}
		