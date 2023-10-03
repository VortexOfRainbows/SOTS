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
using Terraria.GameContent.UI.Elements;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;
using SOTS.Buffs;

namespace SOTS.Projectiles.Tide
{    
    public class AtlantisProj : ModProjectile 
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(aiCounter);
			writer.Write(trueChannel);
            writer.Write(returnHit);
            writer.Write(Projectile.tileCollide);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			aiCounter = reader.ReadInt32();
            trueChannel = reader.ReadBoolean();
			returnHit = reader.ReadBoolean();
            Projectile.tileCollide = reader.ReadBoolean();
        }
		public bool trueChannel = true;
		public bool returnHit = false;
        public const int ThrowDuration = 30;
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 4;
			if(!Projectile.tileCollide)
				returnHit = true;
            Projectile.netUpdate = true;
			if(Main.myPlayer == Projectile.owner && !Projectile.tileCollide)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HydroBubble>(), (int)(Projectile.damage * 0.2f), Projectile.knockBack * 0.1f, Projectile.owner, 6, target.whoAmI);
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
			if(Projectile.tileCollide)
            {
                Projectile.tileCollide = false;
                if (Projectile.velocity.X != oldVelocity.X)
					Projectile.velocity.X = -oldVelocity.X;
				if (Projectile.velocity.Y != oldVelocity.Y)
					Projectile.velocity.Y = -oldVelocity.Y;
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.netUpdate = true;
			}
			return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return aiCounter >= ThrowDuration;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			lightColor = Projectile.GetAlpha(Color.Lerp(lightColor, Color.White, 0.5f));
			Color color = new Color(40, 50, 120, 0);
			if (returnHit)
			{
                color = new Color(140, 40, 50, 0);
            }
			else if(aiCounter >= ThrowDuration)
			{
				if(!Projectile.tileCollide)
				{
                    color = new Color(120, 50, 120, 0);
                }
            }
            for (int i = 0; i < 6; i++)
            {
                Vector2 circular = new Vector2(2, 0).RotatedBy(i * MathHelper.TwoPi / 6f + MathHelper.ToRadians(SOTSWorld.GlobalCounter * -1.5f));
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY) + circular, null, color, Projectile.rotation + MathHelper.PiOver4, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, lightColor, Projectile.rotation + MathHelper.PiOver4, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		bool runOnce = true;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if(player.controlUseItem && player.whoAmI == Main.myPlayer)
			{
				trueChannel = true;
            }
			else
			{
				trueChannel = false;
            }
            aiCounter++;
			if (aiCounter >= ThrowDuration) //this controls when the spear is actually left the player
			{
				Projectile.extraUpdates = 1;
				Projectile.friendly = true;
				if (runOnce)
				{
					runOnce = false;
					Projectile.tileCollide = true;
					Projectile.netUpdate = true;
                    SOTSUtils.PlaySound(SoundID.Item71, Projectile.Center, 0.9f, -0.4f);
					for(int i = 0; i < 28; i++)
						WaterParticle.NewWaterParticle(Projectile.Center, Projectile.velocity * 0.75f + Main.rand.NextVector2Circular(12, 12) * 0.4f, Main.rand.NextFloat(1.4f, 1.7f));
                    return;
				}
				if(aiCounter >= ThrowDuration + 60)
				{
					Projectile.tileCollide = false;
                }
                Vector2 toPlayer = player.Center - Projectile.Center;
                if (!Projectile.tileCollide && aiCounter > ThrowDuration + 20)
                {
                    Vector2 holdUpOffset = new Vector2(0, 17);
					float length = toPlayer.Length();
					toPlayer -= holdUpOffset;
                    float speed = 1.05f + Projectile.velocity.Length() * 0.05f + length * 0.00005f * Math.Clamp(aiCounter * aiCounter / 900f, 0, 100);
					if (speed > length)
						speed = length;
                    Projectile.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed;
					if(toPlayer.Length() < 24 )
                    {
                        if(player.channel)
                        {
							if(Projectile.owner == Main.myPlayer)
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity.SafeNormalize(Vector2.Zero) * 16f, Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
							player.StartChanneling();
                        }
                        Projectile.Kill();
                    }
					float reducedInertia = (aiCounter - ThrowDuration - 60) / 60f;
					reducedInertia = Math.Clamp(reducedInertia, 0, 1);
					Projectile.velocity *= 0.9375f - 0.05f * reducedInertia;
					for(float i = 0; i < 1; i += 0.125f)
                    {
                        if (Main.rand.NextBool(3))
                        {
                            Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5) + Projectile.velocity * i, 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 55);
                            dust.scale = 1.0f;
                            dust.velocity += Projectile.velocity * 0.35f;
                            dust.velocity *= 0.75f;
                            dust.noGravity = true;
                            dust.color = ColorHelpers.AtlantisColorInverse;
                            dust.fadeIn = 0.2f;
                        }
                        else
                            WaterParticle.NewWaterParticle(Projectile.Center + Projectile.velocity * i, Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(1, 1) * 0.4f, Main.rand.NextFloat(0.8f, 1.0f));
                    }
                }
				else
                {
                    for (float i = 0; i < 1; i += 0.125f)
                    {
						if(Main.rand.NextBool(3))
                        {
                            Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5) + Projectile.velocity * i, 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 55);
                            dust.scale = 1.0f;
                            dust.velocity += Projectile.velocity * 0.35f;
                            dust.velocity *= 0.75f;
                            dust.noGravity = true;
                            dust.color = ColorHelpers.AtlantisColor;
                            dust.fadeIn = 0.2f;
                        }
						else
							WaterParticle.NewWaterParticle(Projectile.Center + Projectile.velocity * i, Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(1, 1) * 0.4f, Main.rand.NextFloat(0.8f, 1.0f));
                    }
                }
				Projectile.velocity += new Vector2(0, -0.1f * Math.Sign(toPlayer.X)).RotatedBy(toPlayer.ToRotation());
				Projectile.rotation = Projectile.velocity.ToRotation();
			}
			else //This is the animation for charging and holding out the proj, and for throwing it
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
				Vector2 rotater = new Vector2(11 + aiCounter * 1.75f * (float)Math.Sin(MathHelper.ToRadians(410f / ThrowDuration * aiCounter)), 0).RotatedBy(Projectile.rotation);
				Projectile.position += rotater;
				if(Main.myPlayer == Projectile.owner)
                {
					Projectile.ai[0] = Main.MouseWorld.X;
					Projectile.ai[1] = Main.MouseWorld.Y;
					Projectile.netUpdate = true;
                }
            }
            if (player.itemAnimation <= 10)
            {
                player.itemAnimation = 10;
                player.itemTime = 10;
				if(aiCounter >= ThrowDuration)
                {
                    Vector2 toPlayer = player.Center - Projectile.Center;
					if(toPlayer.X != 0)
						player.direction = -Math.Sign(toPlayer.X);
				}
            }
            if (player.ItemAnimationActive) //This manages how it is held.
			{
				player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
				if(aiCounter < ThrowDuration + 9)
					player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle((Projectile.Center - player.Center).ToRotation() + MathHelper.ToRadians(player.gravDir * -90)));
                else
				{
					float lerp = (aiCounter - ThrowDuration) / 120f;
					if (lerp > 1)
						lerp = 1;
					Vector2 aimTowards = Vector2.Lerp(Projectile.Center, player.Center - new Vector2(0, 17), lerp);
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle((aimTowards - player.Center).ToRotation() + MathHelper.ToRadians(player.gravDir * -90)));
                }
            }
		}
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (returnHit)
			{
				if(player.lifeSteal > 0)
				{
					player.lifeSteal -= 5;
					player.statLife += 5;
					if(Main.myPlayer == Projectile.owner)
						player.HealEffect(5, true);
				}
				player.AddBuff(ModContent.BuffType<AtlantisBuff>(), 15 * 60);
				if(Main.netMode != NetmodeID.Server)
                {
                    SOTSUtils.PlaySound(SoundID.Item3, Projectile.Center, 0.9f, -0.5f);
                    for (int i = 0; i < 30; i++)
                    {
						Vector2 circular = new Vector2(1, 0).RotatedBy(i * MathHelper.TwoPi / 30f);
                        WaterParticle.NewWaterParticle(player.Center + circular * 48, -circular * 2.5f + Main.rand.NextVector2Circular(1, 1) * 0.4f, Main.rand.NextFloat(1.4f, 1.6f));
                    }
                }
            }
        }
    }
}
		