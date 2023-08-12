using Microsoft.Xna.Framework;
using SOTS.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Nature
{    
    public class FriendlyFlowerBolt : ModProjectile 
    {
        public override string Texture => "SOTS/Projectiles/Nature/NatureBolt";
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Flower Bolt");
		}
        public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Summon;
			Projectile.height = 8;
			Projectile.width = 8;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 720;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.extraUpdates = 4;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		public override void Kill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item105, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.45f, -0.2f);
			DustHelper.DrawStar(Projectile.Center, 231, 5f, 4f, 1.5f, 1.75f, 0.7f, 0.7f, true, 10, 0);
		}
		public override void AI()
		{
			Dust dust = Dust.NewDustPerfect(Projectile.Center, 231);
			dust.velocity *= 0.05f;
			dust.noGravity = true;
			dust.scale = 1.7f;

			Projectile.ai[0]++;
			float sin = (float)Math.Sin(Projectile.ai[0] * MathHelper.Pi / 45f);
			Vector2 circular = new Vector2(0, sin * 8).RotatedBy(Projectile.velocity.ToRotation());
			dust = Dust.NewDustPerfect(Projectile.Center + circular, 231, Main.rand.NextVector2Circular(0.2f, 0.2f));
			dust.noGravity = true;
			dust.scale = 1.2f;
		}
	}
}
		