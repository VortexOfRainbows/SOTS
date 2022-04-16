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
			Main.projFrames[projectile.type] = 5;
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 0;
			projectile.melee = true;
			projectile.friendly = true;
			projectile.width = 56;
			projectile.height = 30;
			projectile.timeLeft = 70;
			projectile.penetrate = 4;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.alpha = 40;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 15;
			projectile.extraUpdates = 1;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.ai[0] = 200;
			projectile.netUpdate = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float alphaMult = ((255f - projectile.alpha) / 255f);
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, projectile.height / 2);
			Color color;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i + SOTSWorld.GlobalCounter), false);
				color.A = 0;
				color *= 0.5f;
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color * alphaMult, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(220, 220, 220, 150);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color * alphaMult, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
        public override bool PreAI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 3)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 5;
			}
			return true;
        }
        public override void AI()
		{
			projectile.ai[0]++;
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.alpha += 3;
			int target2 = SOTSNPCs.FindTarget_Basic(projectile.Center, 640);
			if (target2 != -1)
			{
				NPC toHit = Main.npc[target2];
				if (toHit.active && projectile.ai[0] < 200)
				{
					Vector2 toNPC = toHit.Center - projectile.Center;
					projectile.velocity = Vector2.Lerp(projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * (projectile.velocity.Length() + 3), 0.07f);
				}
			}
			if(projectile.ai[0] > 6)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.05f;
				dust.velocity -= 1 * projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.scale = 1.3f;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 3 + projectile.whoAmI * 10), true);
				dust.alpha = 100;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 1.2f;
				dust.velocity += 5 * projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.scale *= 2;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 3 + projectile.whoAmI * 10), true);
				dust.alpha = 100;
			}
			base.Kill(timeLeft);
		}
	}
}
		