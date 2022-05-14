using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Otherworld;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{
    public class GlazeBow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glaze Bow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 64;
            Projectile.aiStyle = 20;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ranged = true;
            Projectile.timeLeft = 120;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            spriteBatch.Draw(texture, drawPos, null, drawColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            DrawArrows(spriteBatch, drawColor);
            if (chargeLevel == 2)
            {
                float alphaMult = afterCount / 15f;
                if (alphaMult > 0.6f)
                    alphaMult = 0.6f;
                Vector2 pos = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - 4 - 2 * fireFromTighten);
                SOTSProjectile.DrawStar(pos, alphaMult, Projectile.velocity.ToRotation(), (float)Math.Sin(afterCount * Math.PI / 90f) * MathHelper.Pi / 6);
            }
            else
                drawGradient();
            return false;
        }
        const int fireFromDist = 40;
        const int fireFromTighten = 12;
        const int visualsOffsetAmt = 30;
        const float firstDelay = 0.5f;
        const float secondDelay = 0.5f;
        float afterCount = 0;
        int textureHeight = 10;
        public void DrawArrows(SpriteBatch spriteBatch, Color drawColor)
        {
            if (Projectile.ai[0] != 0)
            {
                int arrowType = (int)Projectile.ai[1];
                if(!Main.projectileLoaded[arrowType])
                {
                    Main.instance.LoadProjectile(arrowType);
                }
                Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[arrowType].Value;
                if (arrowType == ModContent.ProjectileType<HardlightArrow>() || arrowType == ModContent.ProjectileType<ChargedHardlightArrow>())
                {
                    texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/HardlightArrowShaft").Value;
                }
                textureHeight = texture.Height / 2 + 2;
                float additionalAlphaMult = 1;
                if(counter < 0 && chargeLevel == 0)
                {
                    additionalAlphaMult = ((Projectile.ai[0] * firstDelay) + counter) / (Projectile.ai[0] * firstDelay);
                }    
                float chargePercent = counter / Projectile.ai[0];
                if (chargePercent < 0)
                    chargePercent = 0;
                float compress = visualsOffsetAmt * (chargePercent + chargeLevel);
                for(int i = -2; i <= 2; i++)
                {
                    float scale = 1f - Math.Abs(i) * 0.1f + (chargePercent + chargeLevel) * 0.2f;
                    if (scale > 1)
                        scale = 1;
                    float chargeAlpha = 1 - MathHelper.Clamp(Math.Abs(i) - (chargePercent + chargeLevel), 0, 2) * 0.45f;
                    if (i != 0)
                        chargeAlpha *= 0.8f;
                    float degreeOff = visualsOffsetAmt * Math.Abs(i) - compress;
                    if (degreeOff < 0)
                        degreeOff = 0;
                    Vector2 away = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(degreeOff * Math.Sign(i)));
                    Vector2 fireFrom = Projectile.Center + away * (fireFromDist - textureHeight - (chargePercent + chargeLevel) * fireFromTighten);
                    spriteBatch.Draw(texture, fireFrom - Main.screenPosition, null, drawColor * chargeAlpha * additionalAlphaMult, (Projectile.velocity.SafeNormalize(Vector2.Zero) + away).ToRotation() + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
                }
            }
        }
        public void drawGradient()
        {
            if (Projectile.ai[0] != 0)
            {
                float chargePercent = counter / Projectile.ai[0];
                if (chargePercent < 0)
                    chargePercent = 0;
                Vector2 fireFrom = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - 4 - (chargePercent + chargeLevel) * fireFromTighten);
                Texture2D texture = Mod.Assets.Request<Texture2D>("Assets/StrangeGradient").Value;
                if(chargePercent > 0)
                {
                    float alphaMult = (float)Math.Sin(MathHelper.ToRadians(chargePercent * 170));
                    float length = 30 * (1 - chargePercent);
                    for (int i = 0; i < 120; i++)
                    {
                        Vector2 circular = new Vector2(length, 0).RotatedBy(MathHelper.ToRadians(i * 3));
                        circular.X *= 0.6f;
                        Vector2 scale = new Vector2(circular.Length() / length, 0.5f) * (0.25f + 0.75f * (1 - chargePercent));
                        circular = circular.RotatedBy(Projectile.velocity.ToRotation());
                        Main.spriteBatch.Draw(texture, fireFrom + circular - Main.screenPosition, null, new Color(116, 125, 238, 0) * alphaMult * ((0.5f + 0.2f * chargeLevel) / (circular.Length() / length)), circular.ToRotation(), new Vector2(texture.Width / 2, texture.Height / 2), scale * 1.25f, SpriteEffects.None, 0f);
                    }
                }
            }
        }
        int chargeLevel = 0;
        float counter = -1;
        bool ended = false;
        public override void Kill(int timeLeft)
        {
            if(Projectile.owner == Main.myPlayer)
            {
                float percent = counter / Projectile.ai[0];
                Vector2 fireFrom = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - textureHeight - (percent + chargeLevel) * fireFromTighten);
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 5, 1.2f, -0.1f);
                Projectile proj = Projectile.NewProjectileDirect(fireFrom, Projectile.velocity * (0.5f + 0.5f * chargeLevel), (int)Projectile.ai[1], Projectile.damage, Projectile.knockBack * (0.2f + 0.4f * (percent + chargeLevel)), Main.myPlayer);
                proj.GetGlobalProjectile<SOTSProjectile>().frostFlake = chargeLevel; //this sould sync automatically on the SOTSProjectile end
            }
        }
        public void ChargeAI()
        {
            Player player = Main.player[Projectile.owner]; 
            if (Projectile.ai[0] != 0)
            {
                if (chargeLevel < 2)
                    counter += 0.5f;
                else
                {
                    afterCount++;
                    counter = 0;
                }
                float percent = counter / Projectile.ai[0];
                Vector2 fireFrom = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - (percent + chargeLevel) * fireFromTighten);
                if (chargeLevel < 2 && counter > 0)
                {
                    if (counter == (int)Projectile.ai[0] / 2)
                        SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 15, 1.1f, 0.6f);
                    if (counter >= Projectile.ai[0])
                    {
                        SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 30, 0.8f, -0.3f);
                        if(chargeLevel == 0)
                            for (int k = 0; k < 30; k++)
                            {
                                Vector2 circularLocation = new Vector2(0, 10).RotatedBy(MathHelper.ToRadians(k * 12));
                                circularLocation.Y *= 1.0f;
                                circularLocation.X *= 0.6f;
                                circularLocation = circularLocation.RotatedBy(Projectile.velocity.ToRotation());
                                Dust dust = Dust.NewDustDirect(fireFrom + circularLocation + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
                                dust.noGravity = true;
                                dust.scale = dust.scale * 0.5f + 0.9f;
                                dust.velocity = dust.velocity * 0.05f + circularLocation * 0.2f + player.velocity * 0.95f;
                                dust.fadeIn = 0.1f;
                            }
                        else
                        {
                            float rand = Main.rand.NextFloat(MathHelper.Pi);
                            DustStar(fireFrom, ModContent.DustType<CopyDust4>(), 6, 1.7f, 2.4f, 1.2f, 1f, 0.8f, Projectile.velocity.ToRotation(), rand, 0.9f);
                            DustStar(fireFrom, ModContent.DustType<CopyDust4>(), 6, 1.1f, 1.5f, 1.2f, 1f, 0.5f, Projectile.velocity.ToRotation(), rand, 0.6f);
                        }
                        counter = -(int)Projectile.ai[0] * secondDelay;
                        chargeLevel++;
                    }
                }
                float actionPercent = percent;
                if (actionPercent < 0)
                    actionPercent = 0;
                fireFrom = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - 2 - (actionPercent + chargeLevel) * fireFromTighten);
                if (percent > 0 || chargeLevel > 0)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Vector2 circularLocation = new Vector2(0, 40).RotatedBy(Main.rand.NextFloat(MathHelper.Pi) * 2);
                        circularLocation.Y *= 1.0f;
                        circularLocation.X *= 0.7f;
                        circularLocation = circularLocation.RotatedBy(Projectile.velocity.ToRotation());
                        Dust dust = Dust.NewDustDirect(fireFrom + circularLocation + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
                        dust.noGravity = true;
                        dust.scale = dust.scale * 0.5f + 0.9f;
                        dust.velocity = circularLocation * -0.11f + player.velocity * 0.95f;
                        dust.fadeIn = 0.1f;
                    }
                }
                if(percent > 0.4f || chargeLevel > 0)
                {
                    for (int k = 0; k <= chargeLevel; k++)
                    {
                        if (Main.rand.NextBool(4))
                        {
                            Dust dust = Dust.NewDustDirect(fireFrom + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
                            dust.noGravity = true;
                            dust.scale = dust.scale * 0.5f + 0.8f + chargeLevel * 0.2f;
                            dust.velocity = dust.velocity * 0.18f + Projectile.velocity * -0.04f + Main.rand.NextVector2Circular(0.5f, 0.5f) + player.velocity * 0.95f;
                            dust.fadeIn = 0.1f;
                        }
                    }
                }
            }
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Main.player[Projectile.owner].channel && (counter >= 0 || chargeLevel != 0))
                ended = true;
            if (!ended)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.timeLeft = 2;
            }
            Vector2 vector2_1 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                Item selectedItem = player.inventory[Main.player[Projectile.owner].selectedItem];
                float num1 = selectedItem.shootSpeed * Projectile.scale;
                Vector2 vector2_2 = vector2_1;
                float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                if ((double)Main.player[Projectile.owner].gravDir == -1.0)
                    num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                float num4 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num6 = num1 / num5;
                float num7 = num2 * num6;
                float num8 = num3 * num6;

                if ((double)num7 != Projectile.velocity.X || (double)num8 != Projectile.velocity.Y || Projectile.ai[0] != selectedItem.useTime)
                    Projectile.netUpdate = true;
                Projectile.velocity.X = num7;
                Projectile.velocity.Y = num8;
                float reduced = 1f;
                if (chargeLevel >= 1)
                    reduced = 0.5f;
                Projectile.ai[0] = selectedItem.useTime * reduced;
            }
            if (counter == -1 && Projectile.ai[0] != 0 && runOnce)
            {
                runOnce = false;
                counter = -(int)(Projectile.ai[0] * firstDelay);
            }
            ChargeAI();
            if (Projectile.hide == false)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
                Projectile.alpha = 0;
            }
            Projectile.hide = false;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.position.X = (vector2_1.X - (Projectile.width / 2));
            Projectile.position.Y = (vector2_1.Y - (Projectile.height / 2));
            Projectile.rotation = (float)(Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(Projectile.direction == 1 ? 72.5f : 107.5f));
            return false;
        }
        public void DustStar(Vector2 position, int DustType, float pointAmount = 5, float mainSize = 1, float dustDensity = 1, float pointDepthMult = 1f, float pointDepthMultOffset = 0.5f, float randomAmount = 0, float rotation = 0, float spin = 0, float scaleMult = 1f)
        {
            Player player = Main.player[Projectile.owner];
            float density = 1 / dustDensity * 0.1f;
            for (float k = 0; k < 6.28f; k += density)
            {
                float rand = 0;
                if (randomAmount > 0) { rand = Main.rand.NextFloat(-0.01f, 0.01f) * randomAmount; }

                float x = (float)Math.Cos(k + rand);
                float y = (float)Math.Sin(k + rand);
                float mult = (Math.Abs((k * (pointAmount / 2) % (float)Math.PI) - (float)Math.PI / 2) * pointDepthMult) + pointDepthMultOffset;//triangle wave function
                Vector2 velocity = new Vector2(x, y).RotatedBy(spin) * mult * mainSize;
                velocity.X *= 0.6f;
                velocity = velocity.RotatedBy(rotation);
                Dust dust = Dust.NewDustDirect(position - new Vector2(4, 4), 0, 0, DustType, 0, 0, 0, new Color(116, 125, 238));
                dust.noGravity = true;
                dust.scale = (dust.scale * 0.5f + 1) * scaleMult;
                dust.velocity = dust.velocity * 0.1f + velocity + player.velocity * 0.95f;
                dust.fadeIn = 0.1f;
            }
        }
    }
}