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
using System.IO;

namespace SOTS.Projectiles.Pyramid
{    
    public class BrachialLance : ModProjectile 
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(aiCounter);
			writer.Write(firstHit);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			aiCounter = reader.ReadInt32();
			firstHit = reader.ReadBoolean();
		}
		public bool firstHit = true;
        public const int ThrowDuration = 30;
		public const float ChargeDuration = 90f;
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
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			damage = (int)(damage * (1 + chargeProgress * 0.6f));
			if (chargeProgress >= 1)
				crit = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 4;
			if(firstHit)
			{
				Explosion(2 + (int)(chargeProgress * 2.1f));
				firstHit = false;
				Projectile.netUpdate = true;
            }
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
			if (bounceCounter >= 3 + 8 * chargeProgress)
			{
				aiCounter = 3601;
				Projectile.velocity *= 0;
				Projectile.tileCollide = false;
				Projectile.friendly = false;
				Projectile.netUpdate = true;
			}
			else
			{
				if (bounceCounter < 1 + 4 * chargeProgress)
				{
					Explosion(1);
				}
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
			if (!WorldGen.InWorld((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 10))
			{
				aiCounter = 3601;
				Projectile.velocity *= 0;
				Projectile.tileCollide = false;
				Projectile.friendly = false;
				Projectile.netUpdate = true;
			}
			//SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (aiCounter > 3600)
            {
				Projectile.extraUpdates = 1;
				Projectile.velocity *= 0;
				Projectile.friendly = false;
				Projectile.tileCollide = false;
				Projectile.hide = true;
				if (firstHit)
				{
					Explosion(2 + (int)(chargeProgress * 2.1f));
					firstHit = false;
				}
				if (aiCounter > 4000)
					Projectile.Kill();
				if(Projectile.numUpdates == 0)
					catalogueParticles();
				return;
			}
			else if (aiCounter >= ThrowDuration)
			{
				Projectile.friendly = true;
				if (runOnce)
				{
					Projectile.extraUpdates = 10;
					if(fullCharge)
						Projectile.extraUpdates = 100;
					runOnce = false;
					Projectile.netUpdate = true;
					if(fullCharge)
                    {
						SOTSUtils.PlaySound(SoundID.Item96, Projectile.Center, 0.9f, -0.2f, 0);
                    }
					else
						SOTSUtils.PlaySound(SoundID.Item71, Projectile.Center, 0.9f, -0.4f * chargeProgress);
					return;
				}
				Projectile.tileCollide = true;
				if(Projectile.velocity.X != 0 || Projectile.velocity.Y != 0)
				{
					float increment = fullCharge && !SOTS.Config.lowFidelityMode ? 0.34f : 0.5f;
					if(Main.netMode != NetmodeID.Server)
					{
						for (float i = 0; i < 1; i += increment)
						{
							CurseFoam nextFoam = new CurseFoam(Projectile.Center + i * Projectile.velocity - Projectile.velocity, Main.rand.NextVector2Circular(1, 1) * (0.9f - chargeProgress * 0.35f) + Projectile.velocity * 0.2f, (1 + Main.rand.NextFloat(-0.1f, 0.1f)) * 0.8f * (1f + 0.225f * chargeProgress), false);
							foamParticleList1.Add(nextFoam);
						}
						if (Main.rand.NextBool(3))
						{
							Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4) + 0.5f * Projectile.velocity, 0, 0, ModContent.DustType<CopyDust4>());
							dust.noGravity = true;
							dust.velocity *= 0.8f * (1 - 0.2f * chargeProgress);
							dust.scale = 1.5f * (1 + 0.3f * chargeProgress);
							dust.fadeIn = 0.1f;
							dust.color = new Color(219, 43, 43, 0) * 0.6f * (1 + 0.3f * chargeProgress);
						}
						if (Main.rand.NextBool(2))
						{
							if (!SOTS.Config.lowFidelityMode || Main.rand.NextBool(2))
							{
								Dust dust3 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 16, 16, ModContent.DustType<CopyDust4>());
								dust3.noGravity = true;
								dust3.velocity *= 1.7f;
								dust3.scale = 1.4f * (1 + 0.2f * chargeProgress);
								dust3.fadeIn = 0.1f;
								dust3.color = new Color(200, 68, 70, 0) * 0.75f * (1 + chargeProgress);
							}
						}
						else
						{
							if (!SOTS.Config.lowFidelityMode || Main.rand.NextBool(2))
							{
								Dust dust2 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 16, 16, ModContent.DustType<AlphaDrainDust>());
								dust2.noGravity = true;
								dust2.velocity *= 0.4f;
								dust2.scale = 1.5f;
								dust2.color = new Color(188, 128, 228, 0) * 0.5f * (1 + 0.5f * chargeProgress);
							}
						}
					}
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
			if(Projectile.numUpdates == 0 && aiCounter <= 3600)
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
		public void Explosion(int num = 1)
        {
			if(Main.myPlayer == Projectile.owner)
			{
				for(int i = num; i > 0; i--)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RubySpawnerFinder>(), (int)(Projectile.damage * (1 + chargeProgress * 0.6f)), -Projectile.knockBack, Main.myPlayer);
			}
			if(Main.netMode != NetmodeID.Server)
			{
				SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f + num * 0.1f, 0.4f - num * 0.3f);
				for (int i = 0; i < (18 + num) * num; i++)
				{
					Vector2 circular = new Vector2(3 + num * 3, 0).RotatedBy(MathHelper.ToRadians(360f / (15f * num) * i));
					int num2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					Dust dust = Main.dust[num2];
					dust.color = new Color(255, 80, 80, 40);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.6f;
					dust.alpha = Projectile.alpha;
					dust.velocity += circular * Main.rand.NextFloat(0.5f, 1.4f);
					if (Main.rand.NextBool((!SOTS.Config.lowFidelityMode ? 2 : 3)))
					{
						CurseFoam nextFoam = new CurseFoam(Projectile.Center, Main.rand.NextVector2Circular(1, 1) + 0.4f * dust.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-15, 15))), (1 + Main.rand.NextFloat(-0.1f, 0.1f)) * 0.9f * (1f + 0.1f * chargeProgress), false);
						foamParticleList1.Add(nextFoam);
					}
				}
			}
		}
	}
}
		