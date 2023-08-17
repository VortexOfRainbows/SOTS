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
			// DisplayName.SetDefault("Plasma Sphere");
		}
        public override void SetDefaults()
        {
			Projectile.height = 32;
			Projectile.width = 32;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 6004;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.localNPCHitCooldown = 10;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.DamageType = ModContent.GetInstance<VoidMagic>();
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int timeLeft = Projectile.timeLeft - 2;
			if (timeLeft < 0)
				timeLeft = 0;
			float timeBeforeEnd = (float)Math.Sqrt(timeLeft / 50f);
			if (timeBeforeEnd > 1)
				timeBeforeEnd = 1;
			int width = (int)(48 * Projectile.scale * timeBeforeEnd);
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			int timeLeft = Projectile.timeLeft - 2;
			if (timeLeft < 0)
				timeLeft = 0;
			float timeBeforeEnd = (float)Math.Sqrt(timeLeft / 50f);
			if (timeBeforeEnd > 1)
				timeBeforeEnd = 1;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
			float sin = (float)Math.Sin(MathHelper.ToRadians(120 - Projectile.timeLeft));
			sin = 0.8f + sin * 0.3f;
			Color color = ColorHelpers.InfernoColorAttempt(0.3f + 0.3f * (float)Math.Sin(MathHelper.ToRadians(counter)));
			color.A = 0;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			SOTS.GodrayShader.Parameters["distance"].SetValue(6);
			SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
			SOTS.GodrayShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/noise").Value);
			SOTS.GodrayShader.Parameters["rotation"].SetValue(MathHelper.ToRadians(counter));
			SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f * sin);
			SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale * sin * timeBeforeEnd, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix); 
			color = ColorHelpers.Inferno1 * 1.1f;
			color.A = 0;
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale / 2f * sin * timeBeforeEnd, SpriteEffects.None, 0f);
			color = ColorHelpers.Inferno2 * 1.1f;
			color.A = 0;
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale / 2f * sin * timeBeforeEnd, SpriteEffects.None, 0f);
			return false;
		}
		public override bool ShouldUpdatePosition() 
		{
			return Projectile.ai[0] < 0;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			float damageMult = 1f;
			if (!ended)
				damageMult = 0.5f;
			modifiers.SourceDamage *= (0.6f + totalCharges * 0.4f) * damageMult;
        }
        int counter = 0;
		int totalCharges = 1;
		bool runOnce = true;
		bool ended = false;
		float ogVelo = 0;
		float veloScale = 0;
        public override void Kill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.9f, -0.5f);
			for (int i = 30; i > 0; i--)
			{
				Vector2 circular = new Vector2(48, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
				int dust2 = Dust.NewDust(Projectile.Center - new Vector2(12, 12), 16, 16, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = ColorHelpers.InfernoColorAttempt(Main.rand.NextFloat(0.5f));
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
				veloScale = Projectile.velocity.Length();
				ogVelo = veloScale;
				Projectile.scale = 0.1f;
				runOnce = false;
            }
			counter++;
			if (!Projectile.active)
				return false;
			Player player  = Main.player[Projectile.owner];
			if (!player.channel)
				ended = true;
			if ((!ended || Projectile.timeLeft > 120 || Projectile.scale < 0.9f) && Projectile.ai[0] >= 0)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 toCursor = Main.MouseWorld - player.MountedCenter;
					Projectile.velocity = toCursor.SafeNormalize(Vector2.Zero) * veloScale;
					Projectile.netUpdate = true;
				}
				Projectile.Center = player.MountedCenter + Projectile.velocity;
				Projectile.timeLeft = 120;
				Projectile.ai[1]++;
				if(Projectile.ai[1] > Projectile.ai[0] && totalCharges < 80)
				{
					SOTSUtils.PlaySound(SoundID.Item15, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1f, -0.1f);
					if (Main.myPlayer == player.whoAmI)
					{
						Item item = player.HeldItem;
						VoidItem vItem = item.ModItem as VoidItem;
						if(vItem != null)
							vItem.DrainMana(player);
					}
					Projectile.ai[1] -= Projectile.ai[0];
					totalCharges++;
				}
			}
			else
			{
				Projectile.localNPCHitCooldown = 5;
				if (Projectile.ai[0] >= 0)
				{
					SOTSUtils.PlaySound(SoundID.Item92, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.9f, -0.4f);
					Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * (10 + (float)Math.Sqrt(totalCharges * 1.3f + 1f));
					Projectile.ai[0] = -1;
				}
				else
					Projectile.velocity *= 0.965f;
            }
			float destinationScale = (float)Math.Pow(totalCharges, 0.3f) + (totalCharges - 1) * 0.03f;
			if (Projectile.timeLeft < 20)
            {
				Projectile.scale = MathHelper.Lerp(destinationScale, 0, 1 - Projectile.timeLeft / 20f);
            }
			else
            {
				Projectile.scale = MathHelper.Lerp(Projectile.scale, destinationScale, 0.1f);
				veloScale = MathHelper.Lerp(veloScale, ogVelo * (1 + (destinationScale - 1) * 0.5f), 0.1f);
			}
			if (Projectile.ai[0] >= 0)
			{
				float chance = 20 - destinationScale;
				if (chance <= 3)
					chance = 3;
				for(int i = 4; i >0; i--)
				{
					if (Main.rand.NextBool((int)chance))
					{
						Vector2 circular = new Vector2(48 * destinationScale, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
						int dust2 = Dust.NewDust(Projectile.Center - new Vector2(12, 12) + circular, 16, 16, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = ColorHelpers.InfernoColorAttempt(Main.rand.NextFloat(0.7f));
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
						int dust2 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + circular, 0, 0, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = ColorHelpers.InfernoColorAttempt(Main.rand.NextFloat(0.7f));
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 0.8f + (float)Math.Sqrt(destinationScale);
						dust.velocity = dust.velocity * 0.1f + Projectile.velocity * -0.5f;
						dust.alpha = 125;
					}
				}
			}
			return true;
		}
	}
}