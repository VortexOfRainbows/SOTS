using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using System.IO;

namespace SOTS.Projectiles.AbandonedVillage
{    
    public class FortressCrasher : ModProjectile 
    {
        public override string Texture => "SOTS/Items/AbandonedVillage/FortressCrasher";
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ThrowDuration);
            writer.Write(ChargeDuration);
            writer.Write(Projectile.penetrate);
			writer.WriteVector2(saveVelo);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ThrowDuration = reader.ReadSingle();
            ChargeDuration = reader.ReadSingle();
            Projectile.penetrate = reader.ReadInt32();
            saveVelo = reader.ReadVector2();
        }
        public float ThrowDuration = -1f;
		public float ChargeDuration = -1f;
        public int aiCounter = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = false;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 7200;
			Projectile.penetrate = 20;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 16;
			height = 16;
			fallThrough = true;
			return true;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			var norm = saveVelo.SNormalize();
            hitbox.X += (int)(norm.X * 44);
			hitbox.Y += (int)(norm.Y * 44);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.ArmorPenetration += 10;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.ai[1] = target.whoAmI;
			Projectile.ai[2] = 1;
			Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;
            if (Projectile.ai[2] >= -1)
            {
				Projectile.penetrate -= 5;
                Projectile.ai[1] = -1;
                Projectile.ai[2] = 1;
                Projectile.netUpdate = true;
            }
			if (Projectile.penetrate <= 0)
				return true;
            return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return aiCounter >= ThrowDuration;
		}
		public override bool PreDraw(ref Color lightColor)
        {
			Player player = Main.player[Projectile.owner];
            Texture2D pixel = ModContent.Request<Texture2D>("SOTS/Items/Secrets/WhitePixel").Value;
            float scaleMod = 1;
            float alphaMult = MathF.Sqrt(1 - Projectile.alpha / 255f);
            Vector2 previous = Projectile.Center;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Vector2 toPrev = previous - center;
                Main.spriteBatch.Draw(pixel, center - Main.screenPosition, null, new Color(179, 33, 68, 0) * perc * 1.0f * alphaMult, toPrev.ToRotation(), new Vector2(0, 1), new Vector2(toPrev.Length() / 2f, 3.75f * perc * scaleMod), SpriteEffects.None, 0f);
                previous = center;
            }
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(24, 40);
            float rot = Projectile.rotation + MathHelper.PiOver4;
            int dir = 1;
			if (player.direction * player.gravDir == -1)
			{
                dir = -1;
                rot = Projectile.rotation - 5 * MathHelper.PiOver4;
                drawOrigin = new Vector2(40, 40);
            }
			for (float k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(16 * (1 - chargeProgress) + 4, 0).RotatedBy(MathHelper.ToRadians(k * 60 + chargeProgress * 90 + SOTSWorld.GlobalCounter));
				float alphaScale = chargeProgress * 0.9f;
				Color color = new Color(179, 33, 68, 0);
                color = Projectile.GetAlpha(color) * alphaScale;
				Vector2 drawPos = Projectile.Center - Main.screenPosition;
				Main.spriteBatch.Draw(texture, drawPos + circular + new Vector2(0, Projectile.gfxOffY), null, color, rot, drawOrigin, Projectile.scale * 1f, dir == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			lightColor = Projectile.GetAlpha(Color.Lerp(lightColor, Color.White, 0.5f));
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, lightColor, rot, drawOrigin, Projectile.scale, dir == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			return false;
        }
        private float chargeProgress = 0;
        private bool runOnce = true;
		public void SetVelo(Vector2 velo) //Used by SOTSProjectile to give homing accessories synergy with the item visually
		{
			saveVelo = velo;
		}
		private Vector2 saveVelo;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if(ChargeDuration == -1 || ThrowDuration == -1)
			{
				if(Projectile.owner == Main.myPlayer)
                {
                    ChargeDuration = ThrowDuration = Math.Max(player.itemTime, 2);
                    Projectile.netUpdate = true;
                }
			}
			if (Projectile.ai[2] == 1 && Projectile.friendly)
			{
				Projectile.localNPCHitCooldown++;
                Projectile.friendly = Projectile.tileCollide = false;
				if (Projectile.ai[1] == -1)
                {
                    Explosion(10);
                    Projectile.velocity *= 0.2f;
					saveVelo *= 0.8f;
                    Projectile.ai[2] = -10;
                }
				else
                {
                    Explosion(25);
                    Projectile.Center -= saveVelo * 0.05f;
                    Projectile.velocity = saveVelo * -0.05f;
                    Projectile.ai[2] = -15;
                }
            }
			else if (Projectile.ai[2] < 0)
			{
				Projectile.ai[2]++;
				if (Projectile.ai[2] >= -7f)
				{
					Projectile.ai[1] = -1;
					Projectile.friendly = true;
					if (Projectile.ai[2] >= -1f)
                        Projectile.tileCollide = true;
                    Projectile.netUpdate = true;
                }
				else
                    Projectile.friendly = Projectile.tileCollide = false;
                if (Projectile.ai[2] >= -10)
				{
                    int num = 11 + (int)Projectile.ai[2];
                    Projectile.velocity += num / 55f * saveVelo;
                }
				else
				{
					Projectile.velocity += 0.01f * saveVelo;
				}
			}
			if (chargeProgress >= 1)
			{
				chargeProgress = 1;
                aiCounter++;
            }
			else
			{
				if (chargeProgress < 1)
					chargeProgress += 1f / ChargeDuration;
				if (chargeProgress > 1)
					chargeProgress = 1;
				if (chargeProgress == 1)
				{
					SOTSUtils.PlaySound(SoundID.Item22, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.75f, 0.1f);
				}
			}
			if (aiCounter > 3600)
            {
				Projectile.extraUpdates = 1;
				Projectile.velocity *= 0;
				Projectile.friendly = false;
				Projectile.tileCollide = false;
				Projectile.hide = true;
				if (aiCounter > 4000 || Projectile.penetrate < 0)
					Projectile.Kill();
				return;
			}
			else if (aiCounter >= ThrowDuration)
			{
				if (saveVelo == Vector2.Zero)
					saveVelo = Projectile.velocity;
				if (runOnce)
				{
					Projectile.ai[1] = Projectile.ai[0] = -1;
					Projectile.extraUpdates = 1;
					Projectile.netUpdate = true;
                    Projectile.tileCollide = true;
                    Projectile.friendly = true;
                    SOTSUtils.PlaySound(SoundID.Item71, Projectile.Center, 1.2f, -0.7f, 0);
					runOnce = false;
                    return;
                }
				else
				{
					int targetNum = (int)Projectile.ai[1];
					if (targetNum >= 0 && targetNum < 200)
                    {
						NPC npc = Main.npc[targetNum];
						if(npc.CanBeChasedBy())
						{
							Vector2 toNPC = npc.Center - Projectile.Center;
							float length = saveVelo.Length();
							saveVelo = (saveVelo + toNPC.SNormalize() * 0.7f).SNormalize() * length;
						}
						else
						{
                            Projectile.ai[1] = -1;
						}
					}
				}
				if((Projectile.velocity.X != 0 || Projectile.velocity.Y != 0) && Main.netMode != NetmodeID.Server)
                {
					for(float i = 0; i < 1; i += 0.5f)
					{
                        Dust d = PixelDust.Spawn(Projectile.Center + Projectile.velocity * i, 0, 0, Main.rand.NextVector2Square(-0.35f, 0.35f) - saveVelo * 0.5f, new Color(179, 33, 68, 0), 5);
                        d.scale = Main.rand.NextFloat(1, 2);
                    }
                    if (Main.rand.NextBool(2))
                    {
                        if (!SOTS.Config.lowFidelityMode || Main.rand.NextBool(2))
                        {
                            Dust dust3 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 16, 16, ModContent.DustType<CopyDust4>());
                            dust3.noGravity = true;
                            dust3.velocity *= 1.5f;
                            dust3.scale = 1.4f;
                            dust3.fadeIn = 0.1f;
                            dust3.color = new Color(179, 33, 68, 0) * 0.75f;
                        }
                    }
                }
				Projectile.rotation = saveVelo.ToRotation();
			}
			else
			{
				Vector2 playerToMouse = new Vector2(Projectile.ai[0], Projectile.ai[1]) - player.Center;
				player.direction = SOTSUtils.SignNoZero(playerToMouse.X);
				player.heldProj = Projectile.whoAmI;
				Vector2 holdUpOffset = new Vector2(0, player.direction * 17 * player.gravDir).RotatedBy(playerToMouse.ToRotation());
				holdUpOffset.X *= 0.9f;
				Projectile.velocity = (playerToMouse + holdUpOffset).SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.Center = player.Center;
				Projectile.Center -= holdUpOffset;
				Vector2 rotater = new Vector2(-10 * chargeProgress + 8 + aiCounter * 1.25f * (float)Math.Sin(MathHelper.ToRadians(410f / ThrowDuration * aiCounter)), 0).RotatedBy(Projectile.rotation);
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
				player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle((Projectile.Center - player.Center).ToRotation() * player.gravDir -MathHelper.PiOver2));
			}
		}
		public void Explosion(int num = 1)
        {
			if(Main.netMode != NetmodeID.Server)
            {
                SOTSUtils.PlaySound(SoundID.Item23, Projectile.Center, 1f, 0.4f);
				Vector2 center = Projectile.Center + saveVelo.SNormalize() * 42;
				Vector2 normVelo = Projectile.velocity.SNormalize();
                for (int i = 0; i < num; i++)
                {
                    Dust d = PixelDust.Spawn(center, 0, 0, Main.rand.NextVector2Square(-3f, 3f) + normVelo * Main.rand.NextFloat(15), new Color(179, 33, 68, 0), 10);
                    d.scale = Main.rand.NextFloat(1, 2);
                }
			}
		}
        public override void OnKill(int timeLeft)
        {
            float incre = SOTS.Config.lowFidelityMode ? 0.5f : 0.34f;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
				for(float j = 0; j < 1; j+= incre)
				{
                    Dust dust3 = Dust.NewDustDirect(Projectile.oldPos[i] - new Vector2(5) + Projectile.Size / 2 - saveVelo * j, 0, 0, ModContent.DustType<CopyDust4>());
                    dust3.noGravity = true;
                    dust3.velocity = dust3.velocity * 0.1f + saveVelo * 0.4f;
                    dust3.scale = 2f * perc;
                    dust3.fadeIn = 0.1f;
                    dust3.color = new Color(179, 33, 68, 0);
                }
            }
            SOTSUtils.PlaySound(SoundID.Item62, Projectile.Center, 0.75f, 0.35f);
			int num = SOTS.Config.lowFidelityMode ? 25 : 40;
            for (int i = 0; i < num; i++)
            {
                Dust dust3 = Dust.NewDustDirect(Projectile.oldPosition + Projectile.Size /2  - new Vector2(5, 5) + saveVelo * Main.rand.NextFloat(-3, 3), 0, 0, Main.rand.NextBool(3) ? DustID.Iron : DustID.Lead);
                dust3.noGravity = true;
                dust3.velocity = dust3.velocity * 1f + saveVelo * 0.4f;
                dust3.scale *= 0.5f;
				dust3.scale += 1f;
            }
        }
    }
}
		