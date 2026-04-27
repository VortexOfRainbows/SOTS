using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Minions
{    
    public class VoidspaceExplosion : ModProjectile 
    {
        public override void SetDefaults()
        {
			Projectile.height = 24;
			Projectile.width = 24;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 2;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.DamageType = DamageClass.Summon;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.DisableCrit();
        }
        public override bool? CanHitNPC(NPC target)
        {
            return target.whoAmI == (int)Projectile.ai[0];
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void OnKill(int timeLeft)
		{
            int type = (int)Projectile.ai[1];
			bool AncientSteel = type == ModContent.ProjectileType<AncientSteelLantern>();
			int secondDust = AncientSteel ? DustID.Torch : DustID.TerraBlade;
            Color c = AncientSteel ? Helpers.ColorHelper.InfernoColorGradient(0.25f) : new Color(75, 255, 30);
			c.A = 0;
			Vector2 safe = Projectile.velocity.SNormalize();
			float length = Projectile.velocity.Length();
			for(int i = 32; i < length - 12; i += 4)
			{
				float percent = i / length;
				Vector2 position = Projectile.Center - Projectile.velocity * percent;
				PixelDust.Spawn(position, 0, 0, safe * Main.rand.NextFloat(1, 2) + Main.rand.NextVector2Circular(.5f, .5f) * (0.5f + 0.5f * percent), c * (0.5f + 0.3f * Main.rand.NextFloat(percent)), 7).scale = 1f + 0.5f * percent;
			}
			for (int i = 0; i < 12; i++)
            {
                Vector2 circularLocation = new Vector2(12, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                Dust dust = Dust.NewDustDirect(Projectile.Center + circularLocation - new Vector2(4), 0, 0, secondDust);
				dust.noGravity = true;
				dust.velocity *= 0.1f;
				dust.velocity += circularLocation * Main.rand.NextFloat(0.25f);
				dust.scale = 1.0f + dust.scale * 0.25f;
			}
			for (int i = 0; i < 8; i++)
			{
				Vector2 circularLocation = new Vector2(12, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
				Dust dust = Dust.NewDustDirect(Projectile.Center + circularLocation - new Vector2(4), 0, 0, SOTSUtils.TypeHelper.CopyDust4Type);
				dust.noGravity = true;
				dust.velocity *= 0.5f;
				dust.velocity += circularLocation * 0.125f;
				dust.scale = 1.0f;
				dust.fadeIn = 0.1f;
				dust.color = c;
			}
		}
	}
}
		