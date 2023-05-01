using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using Terraria.Audio;
using SOTS.NPCs.Boss.Curse;
using System.Collections.Generic;

namespace SOTS.Projectiles.Pyramid
{    
    public class BrachialLance : ModProjectile 
    {
		public const int ThrowDuration = 36;
		public const float ChargeDuration = 120f;
		int bounceCounter = 0;
		int aiCounter = 0;
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = false;
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.timeLeft = 7200;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			damage = (int)(damage * (1 + chargeProgress));
			if (chargeProgress >= 1)
				crit = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 4;
			base.OnHitNPC(target, damage, knockback, crit);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 16;
			height = 16;
			fallThrough = true;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (bounceCounter >= 2)
			{
				Projectile.velocity *= 0;
				Projectile.tileCollide = false;
				Projectile.friendly = false;
			}
			else
			{
				if (Projectile.velocity.X != oldVelocity.X)
					Projectile.velocity.X = -oldVelocity.X;
				if (Projectile.velocity.Y != oldVelocity.Y)
					Projectile.velocity.Y = -oldVelocity.Y;
				Projectile.rotation = Projectile.velocity.ToRotation();
			}
			bounceCounter++;
			return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return aiCounter >= ThrowDuration;
		}
		float chargeProgress = 0;
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float otherAlphaMult = 1 - 0.4f * (float)aiCounter / ThrowDuration;
			if (otherAlphaMult < 0)
				otherAlphaMult = 0;
			for (float k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(16 * (1 - chargeProgress) + 4, 0).RotatedBy(MathHelper.ToRadians(k * 60 + chargeProgress * 90 + SOTSWorld.GlobalCounter));
				float alphaScale = chargeProgress * 0.9f * otherAlphaMult;
				Color color = new Color(160, 30, 35, 0);
				color = Projectile.GetAlpha(color) * alphaScale;
				color.A = 0;
				Vector2 drawPos = Projectile.Center - Main.screenPosition;
				Main.spriteBatch.Draw(texture, drawPos + circular + new Vector2(-1, Projectile.gfxOffY + 2), null, color, Projectile.rotation + MathHelper.PiOver4, drawOrigin, Projectile.scale * 1f, SpriteEffects.None, 0f);
			}
			lightColor = Projectile.GetAlpha(Color.Lerp(lightColor, Color.White, 0.5f));
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(-1, Projectile.gfxOffY + 2), null, lightColor, Projectile.rotation + MathHelper.PiOver4, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		bool runOnce = true;
		bool fullCharge = false;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.channel || aiCounter > 0)
				aiCounter++;
			else
			{
				if (chargeProgress < 1)
					chargeProgress += 1f / ChargeDuration;
				if (chargeProgress > 1)
					chargeProgress = 1;
				if (chargeProgress == 1 && !fullCharge)
				{
					SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.75f, -0.2f);
					fullCharge = true;
				}
			}
			//SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (aiCounter >= ThrowDuration)
			{
				Projectile.friendly = true;
				if (runOnce)
				{
					Projectile.extraUpdates = 12;
					if(fullCharge)
						Projectile.extraUpdates = 20;
					runOnce = false;
					Projectile.netUpdate = true;
					SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
					return;
				}
				Projectile.tileCollide = true;
				if(Projectile.velocity.X != 0 || Projectile.velocity.Y != 0)
				{
					float increment = fullCharge ? 0.25f : 0.5f;
					for (float i = 0; i < 1; i += increment)
					{
						CurseFoam nextFoam = new CurseFoam(Projectile.Center + i * Projectile.velocity, new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.1f, 0.1f)) * 0.2f + Projectile.velocity * 0.5f, (1 + Main.rand.NextFloat(-0.1f, 0.1f)) * 0.9f * (1f + 0.4f * chargeProgress), false);
						foamParticleList1.Add(nextFoam);
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4) + i * Projectile.velocity, 0, 0, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.6f * (1 - 0.3f * chargeProgress);
						dust.scale = 1.75f * (1 + 0.25f * chargeProgress);
						dust.fadeIn = 0.1f;
						dust.color = new Color(219, 43, 43, 0) * 0.6f * (1 + 0.3f * chargeProgress);
					}
					Dust dust3 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
					dust3.noGravity = true;
					dust3.velocity *= 1.6f;
					dust3.scale = 1.5f * (1 + 0.5f * chargeProgress);
					dust3.fadeIn = 0.1f;
					dust3.color = new Color(200, 68, 70, 0) * 0.75f * (1 + chargeProgress);
					Dust dust2 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 16, 16, ModContent.DustType<AlphaDrainDust>());
					dust2.noGravity = true;
					dust2.velocity *= 0.3f;
					dust2.scale = 1.75f;
					dust2.color = new Color(188, 128, 228, 0) * 0.5f * (1 + 0.5f * chargeProgress);
				}
			}
			else
			{
				Vector2 playerToMouse = new Vector2(Projectile.ai[0], Projectile.ai[1]) - player.Center;
				player.direction = Math.Sign(playerToMouse.X);
				if (player.direction == 0)
					player.direction = 1;
				player.heldProj = Projectile.whoAmI;
				Vector2 holdUpOffset = new Vector2(0, player.direction * 17).RotatedBy(playerToMouse.ToRotation());
				holdUpOffset.X *= 0.9f;
				Projectile.velocity = (playerToMouse + holdUpOffset).SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.Center = player.Center;
				Projectile.Center -= holdUpOffset;
				Vector2 rotater = new Vector2(-12 * chargeProgress + 11 + aiCounter * 1.75f * (float)Math.Sin(MathHelper.ToRadians(410f / ThrowDuration * aiCounter)), 0).RotatedBy(Projectile.rotation);
				Projectile.position += rotater;
				if (player.itemAnimation <= 10)
				{
					player.itemAnimation = 10;
					player.itemTime = 10;
				}
				if(Main.myPlayer == Projectile.owner)
                {
					Projectile.ai[0] = Main.MouseWorld.X;
					Projectile.ai[1] = Main.MouseWorld.Y;
					Projectile.netUpdate = true;
                }
			}
			if(player.ItemAnimationActive && aiCounter < ThrowDuration + 9)
			{
				player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
				player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle((Projectile.Center - player.Center).ToRotation() + MathHelper.ToRadians(player.gravDir * -90)));
			}
			if(Projectile.numUpdates == 0)
				catalogueParticles();
		}
		public List<CurseFoam> foamParticleList1 = new List<CurseFoam>();
		public void catalogueParticles()
		{
			for (int i = 0; i < foamParticleList1.Count; i++)
			{
				CurseFoam particle = foamParticleList1[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					foamParticleList1.RemoveAt(i);
					i--;
				}
				else
				{
					particle.Update();
					if (!particle.active)
					{
						particle = null;
						foamParticleList1.RemoveAt(i);
						i--;
					}
					else
                    {
						particle.scale *= 1.01f;
						particle.position += particle.velocity * 0.975f;
					}
				}
			}
		}
	}
}
		