using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.Void;

namespace SOTS.Projectiles.Chaos
{
	public class ChaosPunch : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bizarre Adventure Fist");
			Main.projFrames[Projectile.type] = 5;
		}
		public override void SetDefaults()
		{
			Projectile.aiStyle = 0;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.width = 56;
			Projectile.height = 30;
			Projectile.timeLeft = 70;
			Projectile.penetrate = 4;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.alpha = 40;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			Projectile.extraUpdates = 1;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.ai[0] = 200;
			Projectile.netUpdate = true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			float alphaMult = ((255f - Projectile.alpha) / 255f);
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, Projectile.height / 2);
			Color color;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i + SOTSWorld.GlobalCounter), false);
				color.A = 0;
				color *= 0.5f;
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color * alphaMult, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(220, 220, 220, 150);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color * alphaMult, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
        public override bool PreAI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 3)
			{
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 5;
			}
			return true;
        }
        public override void AI()
		{
			Projectile.ai[0]++;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.alpha += 3;
			int target2 = SOTSNPCs.FindTarget_Basic(Projectile.Center, 640);
			if (target2 != -1)
			{
				NPC toHit = Main.npc[target2];
				if (toHit.active && Projectile.ai[0] < 200)
				{
					Vector2 toNPC = toHit.Center - Projectile.Center;
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * (Projectile.velocity.Length() + 3), 0.07f);
				}
			}
			if(Projectile.ai[0] > 6)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.05f;
				dust.velocity -= 1 * Projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.scale = 1.3f;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 3 + Projectile.whoAmI * 10), true);
				dust.alpha = 100;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 1.2f;
				dust.velocity += 5 * Projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.scale *= 2;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 3 + Projectile.whoAmI * 10), true);
				dust.alpha = 100;
			}
			base.Kill(timeLeft);
		}
	}
}
		