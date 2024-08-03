using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Projectiles.Blades
{    
    public class PyrocideSlash : SOTSBlade 
    {
        public override Color color1 => ColorHelpers.Inferno1;
        public override Color color2 => ColorHelpers.Inferno2;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.OnFire3, 900);
        }
        public override void SafeSetDefaults()
        {
            Projectile.timeLeft = 7200;
            Projectile.localNPCHitCooldown = 4;
            Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
            Projectile.friendly = true;
        }
        public override void SwingSound(Player player)
        {
            SOTSUtils.PlaySound(SoundID.Item74, (int)player.Center.X, (int)player.Center.Y, 0.6f, 0.7f * speedModifier);
        }
        public override float AdditionalTipLength => base.AdditionalTipLength;
        public override float HitboxWidth => 48;
        public override float ArmAngleOffset => 15;
        public override void Draw(SpriteBatch spriteBatch, ref Color lightColor)
        {
			Player player = Main.player[Projectile.owner];
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Blades/PyrocideScale");
			Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
			int length = (int)toProjectile.Length();
			Vector2 rotateToPosition = relativePoint(toProjectile);
			Vector2 drawPos = player.Center + rotateToPosition - Main.screenPosition;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 originFlame = new Vector2(texture2.Width / 2, texture2.Height / 2);

			int direction = 1;
			if (toCursor.X < 0)
			{
				direction = -1;
				direction *= -(int)FetchDirection;
			}
			else
				direction *= (int)FetchDirection;
			float rotation = toProjectile.ToRotation();
			Color baseColor = new Color(90, 80, 70, 0);
			int segmentHeight = 24;
			int totalSegments = length / segmentHeight;
			for (int i = 1; i < totalSegments + 1; i++)
			{
				float scale = MathHelper.Lerp(1.0f, 0.65f, (float)Math.Pow(i / (float)(totalSegments + 1), 2));
				Color color2 = Color.Lerp(ColorHelpers.Inferno2, ColorHelpers.Inferno1, 1f - (float)i / totalSegments);
				color2 = Color.Lerp(color2, baseColor, 0.7f);
				Vector2 toProj2 = rotateToPosition + rotateToPosition.SafeNormalize(Vector2.Zero) * (i * segmentHeight);
				for(int j = 0; j < 5; j++)
                {
					Vector2 random = Main.rand.NextVector2Circular(j + 1, j + 1) * 0.75f;
					color2 = Color.Lerp(color2, baseColor, 0.3f);
					color2 = Color.Lerp(color2, new Color(160, 70, 30, 0), 0.3f);
					scale *= 1.15f;
					spriteBatch.Draw(texture2, player.Center + toProj2 - Main.screenPosition + random, null, color2, rotation + MathHelper.Pi, originFlame, new Vector2(1.5f, scale), direction == 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
				}
			}
			rotation = toProjectile.ToRotation() + MathHelper.ToRadians(direction == -1 ? -225 : 45);
			spriteBatch.Draw(texture, drawPos, null, Color.White, rotation, origin, Projectile.scale * 1.4f, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
        public override float MaxSwipeDistance => 480;
        public override float MinSwipeDistance => 300;
        public override float ArcStartDegrees => 235 + 15f / speedModifier;
        public override float GetBaseSpeed(float swordLength)
        {
            return 1.2f + (2.2f / (float)Math.Pow(distance / 180f, 2.1f));
        }
        public override void SlashPattern(Player player, int slashNumber)
        {
            float speedBonus = 0f;
            if (slashNumber == 4)
            {
                speedBonus = 0.7f;
            }
            if (slashNumber == 3)
            {
                speedBonus = -0.1f;
            }
            if (slashNumber == 2)
            {
                speedBonus = 0.4f;
            }
            if (slashNumber == 1)
            {
                speedBonus = -0.5f;
            }
            int damage = Projectile.damage;
            if (slashNumber == 1)
                damage = (int)(damage * 1.5f);
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1] * 0.9f + speedBonus);
            if (proj.ModProjectile is PyrocideSlash a)
            {
                if (slashNumber == 4)
                    a.distance = distance * 0.65f + 16;
                else if (slashNumber == 3)
                    a.distance = distance * 1.10f + 8;
                else if (slashNumber == 2)
                    a.distance = distance * 0.90f + 16;
                if (slashNumber == 1)
                {
                    a.distance = distance * 1.2f + 220;
                }
            }
        }
        public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
        {
            original.Y *= 1f / speedModifier * 15f / (float)Math.Pow(distance, 0.5);
            if (swingNumber == 1)
                original.Y *= 0.18f;
            return original;
        }
        public override float swipeDegreesTotal => 235.0f + (5400f / distance);
        public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
        {
            if (dustAway != Vector2.Zero)
            {
                float amt = Main.rand.NextFloat(1.4f, 2.4f) * distance / 480f;
                for (int i = 0; i < amt; i++) //generates dust at the end of the blade
                {
                    float dustScale = 1f;
                    float rand = Main.rand.NextFloat(0.9f, 1.35f);
                    int type = ModContent.DustType<Dusts.CopyDust4>();
                    if (Main.rand.NextBool(5))
                        type = DustID.SolarFlare;
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + dustAway.SafeNormalize(Vector2.Zero) * 24, 16, 16, type);
                    dust.velocity *= 0.8f / rand;
                    dust.velocity += dustAway.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.2f, 2.0f) * rand;
                    dust.noGravity = true;
                    dust.scale *= 0.4f / rand;
                    dust.scale += 2.0f / rand * dustScale;
                    dust.fadeIn = 0.1f;
                    if (type == ModContent.DustType<Dusts.CopyDust4>())
                        dust.color = Color.Lerp(ColorHelpers.Inferno1, ColorHelpers.Inferno2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
                }
                Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
                for (int i = 0; i < amt; i++) //generates dust throughout the length of the blade
                {
                    float rand = Main.rand.NextFloat(0.9f, 1.35f);
                    int type = ModContent.DustType<Dusts.CopyDust4>();
                    if (Main.rand.NextBool(3))
                        type = DustID.SolarFlare;
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + (toProjectile.SafeNormalize(Vector2.Zero)) * 24 - toProjectile * Main.rand.NextFloat(0.95f), 16, 16, type);
                    dust.velocity *= 0.1f / rand;
                    dust.velocity += dustAway.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.4f, 0.9f) * rand;
                    dust.noGravity = true;
                    dust.scale *= 0.2f / rand;
                    dust.scale += 1.1f * rand;
                    dust.fadeIn = 0.1f;
                    if (type == ModContent.DustType<Dusts.CopyDust4>())
                        dust.color = Color.Lerp(ColorHelpers.Inferno2, ColorHelpers.Inferno1, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
                }
            }
        }
    }
}
		
			