using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
    public class RoseBow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rose Bow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 92;
            Projectile.aiStyle = 20;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 120;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            Texture2D gTexture = Mod.Assets.Request<Texture2D>("Projectiles/Chaos/RoseBowGlow").Value;
            float chargePercent = counter / Projectile.ai[0];
            if (chargePercent < 0)
                chargePercent = 0;
            float alphaMult = 0.2f * (chargePercent + chargeLevel);
            for (int i = 0; i < 6; i++)
            {
                Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(i * 120) + SOTSWorld.GlobalCounter);
                Color drawColor = ColorHelpers.pastelAttempt(circular.ToRotation(), false);
                drawColor.A = 0;
                Main.spriteBatch.Draw(gTexture, drawPos + circular, null, drawColor * alphaMult, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            Color c = ColorHelpers.ChaosPink;
            c.A = 0;
            DrawArrows();
            if (chargeLevel == 2)
            {
                alphaMult = afterCount1 / 15f;
                if (alphaMult > 0.6f)
                    alphaMult = 0.6f;
                Vector2 pos = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist + 12);
                SOTSProjectile.DrawStar(pos, c, alphaMult, Projectile.velocity.ToRotation(), (float)Math.Sin(afterCount1 * Math.PI / 90f) * MathHelper.Pi / 6, 6, 6, 12, 0.5f, 120);
            }
            else
                drawGradient();
            if(chargeLevel >= 1)
            {
                alphaMult = afterCount2 / 15f;
                if (alphaMult > 0.6f)
                    alphaMult = 0.6f;
                Vector2 pos = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - 24);
                SOTSProjectile.DrawStar(pos, c, alphaMult, Projectile.velocity.ToRotation(), (float)Math.Sin(afterCount2 * Math.PI / 90f) * MathHelper.Pi / 6, 10, 6, 16, 0.5f, 180);
            }
            return false;
        }
        const int fireFromDist = 48;
        const int fireFromTighten = 12;
        const float visualsOffsetAmt = 28f;
        const float firstDelay = 0.1f;
        const float secondDelay = 1.0f;
        float afterCount1 = 0;
        float afterCount2 = 0;
        float textureHeight = 10;
        public void DrawArrows()
        {
            if (Projectile.ai[0] != 0)
            {
                Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Chaos/ChaosArrow" + (chargeLevel + 1)).Value;
                Texture2D oldTexture = null;
                textureHeight = texture.Height;
                int oldHeight = texture.Height;
                if (chargeLevel >= 1)
                {
                    oldTexture = Mod.Assets.Request<Texture2D>("Projectiles/Chaos/ChaosArrow" + chargeLevel).Value;
                    oldHeight = oldTexture.Height;
                }
                float chargePercent = counter / Projectile.ai[0];
                if (chargePercent < 0)
                    chargePercent = 0;
                float compress = visualsOffsetAmt * (chargePercent + chargeLevel);
                float alphaMult = 1;
                if (oldTexture != null)
                {
                    if (chargeLevel == 2)
                    {
                        alphaMult = afterCount1 / 15f;
                        if (alphaMult > 1f)
                            alphaMult = 1f;
                    }
                    else
                    {
                        alphaMult = afterCount2 / 15f;
                        if (alphaMult > 1f)
                            alphaMult = 1f;
                    }
                    textureHeight = MathHelper.Lerp(textureHeight, oldHeight, 1 - alphaMult);
                }
                for (int i = -2; i <= 2; i++)
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
                    Vector2 fireFrom = Projectile.Center + away * (fireFromDist - 16 - (chargePercent + chargeLevel) * fireFromTighten);
                    for(int j = 0; j < 2; j++)
                    {
                        Vector2 circular = new Vector2(1.33f * (1 + chargePercent + chargeLevel), 0).RotatedBy(MathHelper.ToRadians(i * 72 + 180 * j + SOTSWorld.GlobalCounter));
                        Vector2 stretch = new Vector2(1 - 0.25f * (chargePercent + chargeLevel), 1 + 0.25f * (chargePercent + chargeLevel));
                        Color drawColor = ColorHelpers.pastelAttempt(circular.ToRotation(), false);
                        drawColor.A = 0;
                        float reverseAlphaMult = 1;
                        if(oldTexture != null)
                        {
                            reverseAlphaMult = alphaMult;
                            Main.spriteBatch.Draw(oldTexture, circular + fireFrom - Main.screenPosition, null, drawColor * chargeAlpha * 0.3f * (1 - alphaMult), (Projectile.velocity.SafeNormalize(Vector2.Zero) + away).ToRotation() + 1.57f, new Vector2(oldTexture.Width / 2, oldTexture.Height / 2), scale * stretch, SpriteEffects.None, 0f);
                        }
                        Main.spriteBatch.Draw(texture, circular + fireFrom - Main.screenPosition, null, drawColor * chargeAlpha * 0.3f * reverseAlphaMult, (Projectile.velocity.SafeNormalize(Vector2.Zero) + away).ToRotation() + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), scale * stretch, SpriteEffects.None, 0f);
                    }
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
                Vector2 fireFrom = getTip(chargePercent);
                Texture2D texture = Mod.Assets.Request<Texture2D>("Assets/StrangeGradient").Value;
                if(chargePercent > 0)
                {
                    Color c = ColorHelpers.ChaosPink;
                    c.A = 0;
                    float alphaMult = (float)Math.Sin(MathHelper.ToRadians(chargePercent * 170));
                    float length = 48 * (1 - chargePercent);
                    for (int i = 0; i < 180; i++)
                    {
                        Vector2 circular = new Vector2(length, 0).RotatedBy(MathHelper.ToRadians(i * 2));
                        circular.X *= 0.6f;
                        Vector2 scale = new Vector2(circular.Length() / length, 0.5f) * (0.25f + 0.75f * (1 - chargePercent));
                        circular = circular.RotatedBy(Projectile.velocity.ToRotation());
                        Main.spriteBatch.Draw(texture, fireFrom + circular - Main.screenPosition, null, c * alphaMult * ((0.6f + 0.2f * chargeLevel) / (circular.Length() / length)), circular.ToRotation(), new Vector2(texture.Width / 2, texture.Height / 2), scale * 1.25f, SpriteEffects.None, 0f);
                    }
                }
            }
        }
        int chargeLevel = 0;
        float counter = -1;
        bool ended = false;
        public override void OnKill(int timeLeft)
        {
            if(Projectile.owner == Main.myPlayer)
            {
                float percent = counter / Projectile.ai[0];
                Vector2 fireFrom = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - (percent + chargeLevel) * fireFromTighten);
                SOTSUtils.PlaySound(SoundID.Item5, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, -0.1f);
                int type = ModContent.ProjectileType<ChaosArrow1>();
                if (chargeLevel >= 1)
                    type = ModContent.ProjectileType<ChaosArrow2>();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), fireFrom, Projectile.velocity, type, Projectile.damage, Projectile.knockBack * (0.2f + 0.4f * (percent + chargeLevel)), Main.myPlayer, chargeLevel == 2 ? 1 : 0);
            }
            if(chargeLevel >= 1)
            {
                Vector2 pos = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - 24);
                float rand = Main.rand.NextFloat(MathHelper.Pi);
                DustStar(pos, Projectile.velocity * 1.65f, ModContent.DustType<CopyDust4>(), 8, 2.4f, 3.5f, 1.0f, 1f, 0.6f, Projectile.velocity.ToRotation(), rand, 0.9f);
                DustStar(pos, Projectile.velocity * 1.65f, ModContent.DustType<CopyDust4>(), 8, 1.6f, 2.5f, 1.0f, 1f, 0.4f, Projectile.velocity.ToRotation(), rand, 0.6f);

            }
            if(chargeLevel == 2)
            {
                Vector2 pos = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist + 12);
                float rand = Main.rand.NextFloat(MathHelper.Pi);
                DustStar(pos, Projectile.velocity * 2.5f, ModContent.DustType<CopyDust4>(), 6, 2.0f, 3.5f, 1.0f, 1f, 0.6f, Projectile.velocity.ToRotation(), rand, 0.9f);
                DustStar(pos, Projectile.velocity * 2.5f, ModContent.DustType<CopyDust4>(), 6, 1.2f, 2.5f,1.0f, 1f, 0.4f, Projectile.velocity.ToRotation(), rand, 0.6f);
            }
        }
        public Vector2 getTip(float percent)
        {
            return Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist + textureHeight / 2 - (percent + chargeLevel) * fireFromTighten - 16 * (1 - 0.5f * (percent + chargeLevel))); 
        }
        public void ChargeAI()
        {
            Player player = Main.player[Projectile.owner];
            float actionPercent = 0;
            if (Projectile.ai[0] != 0)
            {
                if (chargeLevel < 2)
                {
                    counter += 0.5f;
                }
                else
                {
                    afterCount1++;
                    counter = 0;
                }
                if (chargeLevel >= 1)
                    afterCount2++;
                int timeForThorn = Math.Max((int)(Projectile.ai[0] * 0.45f), 1);
                if((int)afterCount2 % timeForThorn == 0 && (int)afterCount2 <= timeForThorn * 6 && (int)afterCount2 > 0) //6 thorns one first charge
                {
                    if(Main.myPlayer == Projectile.owner)
                    {
                        float rotation = (float)(afterCount2 / timeForThorn) * 60;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.5f, 4f), ModContent.ProjectileType<ChaosThorn>(), (int)(Projectile.damage * 1.0f), Projectile.knockBack, Main.myPlayer, 60, rotation);
                    }
                }
                timeForThorn = Math.Max((int)(Projectile.ai[0] * 0.35f), 1);
                if ((int)afterCount1 % timeForThorn == 0 && (int)afterCount1 <= timeForThorn * 8 && (int)afterCount1 > 0) //8 thorns on second charge
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        float rotation = (float)(afterCount1 / timeForThorn) * 45;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(2f, 4.5f), ModContent.ProjectileType<ChaosThorn>(), (int)(Projectile.damage * 1.0f), Projectile.knockBack, Main.myPlayer, 120, rotation);
                    }
                }
                float percent = counter / Projectile.ai[0];
                if (chargeLevel < 2 && counter > 0)
                {
                    if ((int)counter == (int)Projectile.ai[0] / 2)
                        SOTSUtils.PlaySound(SoundID.Item15, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.1f, 0.6f);
                    if ((int)counter >= Projectile.ai[0])
                    {
                        SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.8f, -0.3f);
                        if(chargeLevel == 0)
                        {
                            Vector2 pos = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - 24);
                            float rand = Main.rand.NextFloat(MathHelper.Pi);
                            DustStar(pos, Vector2.Zero, ModContent.DustType<CopyDust4>(), 8, 2.4f, 3f, 1.0f, 1f, 0.6f, Projectile.velocity.ToRotation(), rand, 0.9f);
                            DustStar(pos, Vector2.Zero, ModContent.DustType<CopyDust4>(), 8, 1.6f, 2f, 1.0f, 1f, 0.4f, Projectile.velocity.ToRotation(), rand, 0.6f);
                        }
                        else
                        {
                            Vector2 pos = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist + 12);
                            float rand = Main.rand.NextFloat(MathHelper.Pi);
                            DustStar(pos, Vector2.Zero, ModContent.DustType<CopyDust4>(), 6, 2.0f, 3f, 1.0f, 1f, 0.6f, Projectile.velocity.ToRotation(), rand, 0.9f);
                            DustStar(pos, Vector2.Zero, ModContent.DustType<CopyDust4>(), 6, 1.2f, 2f, 1.0f, 1f, 0.4f, Projectile.velocity.ToRotation(), rand, 0.6f);
                        }
                        counter = -(int)Projectile.ai[0] * secondDelay;
                        chargeLevel++;
                        if(Main.myPlayer == player.whoAmI)
                        {
                            Item item = player.HeldItem;
                            VoidItem vItem = item.ModItem as VoidItem;
                            if (vItem != null)
                                vItem.DrainMana(player);
                        }
                    }
                }
                actionPercent = percent;
                if (actionPercent < 0)
                    actionPercent = 0;
                Vector2 fireFrom = getTip(actionPercent);
                if (percent > 0 || chargeLevel > 0)
                {
                    if (Main.rand.NextBool(4 - chargeLevel))
                    {
                        Vector2 circularLocation = new Vector2(0, 40).RotatedBy(Main.rand.NextFloat(MathHelper.Pi) * 2);
                        circularLocation.Y *= 1.0f;
                        circularLocation.X *= 0.7f;
                        circularLocation = circularLocation.RotatedBy(Projectile.velocity.ToRotation());
                        Dust dust = Dust.NewDustDirect(fireFrom + circularLocation + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, ColorHelpers.ChaosPink);
                        dust.noGravity = true;
                        dust.scale = dust.scale * 0.5f + 0.9f;
                        dust.velocity = circularLocation * -0.11f + player.velocity * 0.95f;
                        dust.fadeIn = 0.1f;
                    }
                }
            }
            Lighting.AddLight(Projectile.Center, (ColorHelpers.ChaosPink * 0.25f * (1 + actionPercent + chargeLevel)).ToVector3());
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
                float reduced = chargeLevel >= 1 ? 0.5f : 1f;
                Projectile.ai[0] = SOTSPlayer.ApplyAttackSpeedClassModWithGeneric(player, ModContent.GetInstance<VoidRanged>(), selectedItem.useTime) * reduced;
            }
            if (counter == -1 && Projectile.ai[0] != 0 && runOnce)
            {
                //Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 15, 1.1f, 0.6f);
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
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            return false;
        }
        public void DustStar(Vector2 position, Vector2 outWards, int DustType, float pointAmount = 5, float mainSize = 1, float dustDensity = 1, float pointDepthMult = 1f, float pointDepthMultOffset = 0.5f, float randomAmount = 0, float rotation = 0, float spin = 0, float scaleMult = 1f)
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
                Dust dust = Dust.NewDustDirect(position - new Vector2(4, 4), 0, 0, DustType, 0, 0, 0, ColorHelpers.ChaosPink);
                dust.noGravity = true;
                dust.scale = (dust.scale * 0.5f + 1) * scaleMult;
                dust.velocity = dust.velocity * 0.1f + velocity + outWards;
                if (outWards == Vector2.Zero)
                    dust.velocity += player.velocity * 0.95f;
                dust.fadeIn = 0.1f;
            }
        }
    }
}