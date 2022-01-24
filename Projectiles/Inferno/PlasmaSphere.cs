using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Inferno
{    
    public class PlasmaSphere : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Sphere");
		}
        public override void SetDefaults()
        {
			projectile.height = 32;
			projectile.width = 32;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 6004;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.localNPCHitCooldown = 5;
			projectile.usesLocalNPCImmunity = true;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int timeLeft = projectile.timeLeft - 2;
			if (timeLeft < 0)
				timeLeft = 0;
			float timeBeforeEnd = (float)Math.Sqrt(timeLeft / 50f);
			if (timeBeforeEnd > 1)
				timeBeforeEnd = 1;
			int width = (int)(48 * projectile.scale * timeBeforeEnd);
			hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			int timeLeft = projectile.timeLeft - 2;
			if (timeLeft < 0)
				timeLeft = 0;
			float timeBeforeEnd = (float)Math.Sqrt(timeLeft / 50f);
			if (timeBeforeEnd > 1)
				timeBeforeEnd = 1;
			Texture2D texture = mod.GetTexture("Effects/Masks/Extra_49");
			float sin = (float)Math.Sin(MathHelper.ToRadians(120 - projectile.timeLeft));
			sin = 0.8f + sin * 0.3f;
			Color color = VoidPlayer.InfernoColorAttempt(0.3f + 0.3f * (float)Math.Sin(MathHelper.ToRadians(counter)));
			color.A = 0;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			SOTS.GodrayShader.Parameters["distance"].SetValue(6);
			SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
			SOTS.GodrayShader.Parameters["noise"].SetValue(mod.GetTexture("TrailTextures/noise"));
			SOTS.GodrayShader.Parameters["rotation"].SetValue(MathHelper.ToRadians(counter));
			SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f * sin);
			SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale * sin * timeBeforeEnd, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix); 
			color = VoidPlayer.Inferno1 * 1.1f;
			color.A = 0;
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale / 2f * sin * timeBeforeEnd, SpriteEffects.None, 0f);
			color = VoidPlayer.Inferno2 * 1.1f;
			color.A = 0;
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale / 2f * sin * timeBeforeEnd, SpriteEffects.None, 0f);
			return false;
		}
		public override bool ShouldUpdatePosition() 
		{
			return projectile.ai[0] < 0;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			damage = (int)(damage * (0.6f + totalCharges * 0.4f));
        }
        int counter = 0;
		int totalCharges = 1;
		bool runOnce = true;
		bool ended = false;
		float ogVelo = 0;
		float veloScale = 0;
        public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 62, 0.9f, -0.5f);
			for (int i = 24 + totalCharges * 2; i > 0; i--)
			{
				Vector2 circular = new Vector2(48, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
				int dust2 = Dust.NewDust(projectile.Center - new Vector2(12, 12), 16, 16, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(0.5f));
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.4f;
				dust.velocity = dust.velocity * 0.1f - circular * 0.1f * Main.rand.NextFloat(2);
				dust.alpha = 125;
			}
		}
        public override bool PreAI()
		{
			if(runOnce)
            {
				veloScale = projectile.velocity.Length();
				ogVelo = veloScale;
				projectile.scale = 0.1f;
				runOnce = false;
            }
			counter++;
			if (!projectile.active)
				return false;
			Player player  = Main.player[projectile.owner];
			if (!player.channel)
				ended = true;
			if ((!ended || projectile.timeLeft > 120 || projectile.scale < 0.9f) && projectile.ai[0] >= 0)
			{
				if (Main.myPlayer == projectile.owner)
				{
					Vector2 toCursor = Main.MouseWorld - player.MountedCenter;
					projectile.velocity = toCursor.SafeNormalize(Vector2.Zero) * veloScale;
					projectile.netUpdate = true;
				}
				projectile.Center = player.MountedCenter + projectile.velocity;
				projectile.timeLeft = 120;
				projectile.ai[1]++;
				if(projectile.ai[1] > projectile.ai[0])
				{
					Main.PlaySound(SoundID.Item, (int)(projectile.Center.X), (int)(projectile.Center.Y), 15, 1f, -0.1f);
					if (Main.myPlayer == player.whoAmI)
					{
						VoidItem.DrainMana(player);
					}
					projectile.ai[1] -= projectile.ai[0];
					totalCharges++;
				}
			}
			else
			{
				if (projectile.ai[0] >= 0)
				{
					Main.PlaySound(SoundID.Item, (int)(projectile.Center.X), (int)(projectile.Center.Y), 92, 0.9f, -0.4f);
					projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * (10 + (float)Math.Sqrt(totalCharges * 1.3f + 1f));
					projectile.ai[0] = -1;
				}
				else
					projectile.velocity *= 0.965f;
            }
			float destinationScale = (float)Math.Pow(totalCharges, 0.3f) + (totalCharges - 1) * 0.03f;
			if (projectile.timeLeft < 20)
            {
				projectile.scale = MathHelper.Lerp(destinationScale, 0, 1 - projectile.timeLeft / 20f);
            }
			else
            {
				projectile.scale = MathHelper.Lerp(projectile.scale, destinationScale, 0.1f);
				veloScale = MathHelper.Lerp(veloScale, ogVelo * (1 + (destinationScale - 1) * 0.5f), 0.1f);
			}
			if (projectile.ai[0] >= 0)
			{
				float chance = 20 - destinationScale;
				if (chance <= 3)
					chance = 3;
				for(int i = 4; i >0; i--)
				{
					if (Main.rand.NextBool((int)chance))
					{
						Vector2 circular = new Vector2(48 * destinationScale, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
						int dust2 = Dust.NewDust(projectile.Center - new Vector2(12, 12) + circular, 16, 16, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(0.7f));
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 0.5f + (float)Math.Sqrt(destinationScale);
						dust.velocity = dust.velocity * 0.1f - circular * 0.06f;
						dust.alpha = 125;
					}
				}
			}
			else
			{
				float chance = 10 - destinationScale;
				if (chance <= 2)
					chance = 2;
				for (int i = 2; i > 0; i--)
				{
					if (Main.rand.NextBool((int)chance))
					{
						Vector2 circular = new Vector2(12 * destinationScale, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
						int dust2 = Dust.NewDust(projectile.Center - new Vector2(4, 4) + circular, 0, 0, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(0.7f));
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 0.8f + (float)Math.Sqrt(destinationScale);
						dust.velocity = dust.velocity * 0.1f + projectile.velocity * -0.5f;
						dust.alpha = 125;
					}
				}
			}
			return true;
		}
	}
}