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
            projectile.width = 34;
            projectile.height = 64;
            projectile.aiStyle = 20;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ranged = true;
            projectile.timeLeft = 120;
            projectile.hide = true;
            projectile.alpha = 255;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            spriteBatch.Draw(texture, drawPos, null, drawColor, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            DrawArrows(spriteBatch, drawColor);
            return false;
        }
        const int fireFromDist = 24;
        const int fireFromTighten = 12;
        const int visualsOffsetAmt = 30;
        const float firstDelay = 0.45f;
        const float secondDelay = 0.45f;
        public void DrawArrows(SpriteBatch spriteBatch, Color drawColor)
        {
            if (projectile.ai[0] != 0)
            {
                int arrowType = (int)projectile.ai[1];
                Projectile proj = new Projectile();
                if(!Main.projectileLoaded[arrowType])
                {
                    Main.instance.LoadProjectile(arrowType);
                }
                Texture2D texture = Main.projectileTexture[arrowType];
                if(arrowType == ModContent.ProjectileType<HardlightArrow>() || arrowType == ModContent.ProjectileType<ChargedHardlightArrow>())
                {
                    texture = mod.GetTexture("Projectiles/Otherworld/HardlightArrowShaft");
                }    
                float additionalAlphaMult = 1;
                if(counter < 0 && chargeLevel == 0)
                {
                    additionalAlphaMult = ((projectile.ai[0] * firstDelay) + counter) / (projectile.ai[0] * firstDelay);
                }    
                float chargePercent = counter / projectile.ai[0];
                if (chargePercent < 0)
                    chargePercent = 0;
                float compress = visualsOffsetAmt * (chargePercent + chargeLevel);
                for(int i = -2; i <= 2; i++)
                {
                    float scale = 1f - Math.Abs(i) * 0.1f + (chargePercent + chargeLevel) * 0.2f;
                    if (scale > 1)
                        scale = 1;
                    float chargeAlpha = 1 - MathHelper.Clamp(Math.Abs(i) - (chargePercent + chargeLevel), 0, 2) * 0.4f;
                    if (i != 0)
                        chargeAlpha *= 0.8f;
                    float degreeOff = visualsOffsetAmt * Math.Abs(i) - compress;
                    if (degreeOff < 0)
                        degreeOff = 0;
                    Vector2 away = projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(degreeOff * Math.Sign(i)));
                    Vector2 fireFrom = projectile.Center + away * (fireFromDist - (chargePercent + chargeLevel) * fireFromTighten);
                    spriteBatch.Draw(texture, fireFrom - Main.screenPosition, null, drawColor * chargeAlpha * additionalAlphaMult, away.ToRotation() + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
                }
            }
        }
        int chargeLevel = 0;
        float counter = -1;
        bool ended = false;
        public override void Kill(int timeLeft)
        {
            if(projectile.owner == Main.myPlayer)
            {
                float percent = counter / projectile.ai[0];
                Vector2 fireFrom = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - (percent + chargeLevel) * fireFromTighten);
                Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 5, 1.2f, -0.1f);
                Projectile proj = Projectile.NewProjectileDirect(fireFrom, projectile.velocity * (0.5f + 0.5f * chargeLevel), (int)projectile.ai[1], projectile.damage, projectile.knockBack * (0.2f + 0.4f * (percent + chargeLevel)), Main.myPlayer);
                proj.GetGlobalProjectile<SOTSProjectile>().frostFlake = chargeLevel; //this sould sync automatically on the SOTSProjectile end
            }
        }
        public void ChargeAI()
        {
            Player player = Main.player[projectile.owner]; 
            if (projectile.ai[0] != 0)
            {
                if (chargeLevel < 2)
                    counter += 0.5f;
                else
                    counter = 0;
                if (chargeLevel < 2 && counter > 0)
                {
                    float percent = counter / projectile.ai[0];
                    Vector2 fireFrom = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - (percent + chargeLevel) * fireFromTighten);
                    if (counter == (int)projectile.ai[0] / 2)
                        Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 1.1f, 0.6f);
                    if (counter >= projectile.ai[0])
                    {
                        Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 30, 0.8f, -0.3f);
                        for (int k = 0; k < 30; k++)
                        {
                            Vector2 circularLocation = new Vector2(0, 12).RotatedBy(MathHelper.ToRadians(k * 12));
                            circularLocation.Y *= 1.0f;
                            circularLocation.X *= 0.6f;
                            circularLocation = circularLocation.RotatedBy(projectile.velocity.ToRotation());
                            Dust dust = Dust.NewDustDirect(fireFrom + circularLocation + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(120, 200, 255));
                            dust.noGravity = true;
                            dust.scale = dust.scale * 0.5f + 1f;
                            dust.velocity = dust.velocity * 0.2f + circularLocation * 0.3f;
                            dust.fadeIn = 0.1f;
                        }
                        counter = -(int)projectile.ai[0] * secondDelay;
                        chargeLevel++;
                    }
                }
            }
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            if (!Main.player[projectile.owner].channel && (counter >= 0 || chargeLevel != 0))
                ended = true;
            if (!ended)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                projectile.timeLeft = 2;
            }
            Vector2 vector2_1 = Main.player[projectile.owner].RotatedRelativePoint(Main.player[projectile.owner].MountedCenter, true);
            if (Main.myPlayer == projectile.owner)
            {
                Item selectedItem = player.inventory[Main.player[projectile.owner].selectedItem];
                float num1 = selectedItem.shootSpeed * projectile.scale;
                Vector2 vector2_2 = vector2_1;
                float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                if ((double)Main.player[projectile.owner].gravDir == -1.0)
                    num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                float num4 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num6 = num1 / num5;
                float num7 = num2 * num6;
                float num8 = num3 * num6;

                if ((double)num7 != projectile.velocity.X || (double)num8 != projectile.velocity.Y || projectile.ai[0] != selectedItem.useTime)
                    projectile.netUpdate = true;
                projectile.velocity.X = num7;
                projectile.velocity.Y = num8;
                float reduced = 1f;
                if (chargeLevel >= 1)
                    reduced = 0.4f;
                projectile.ai[0] = selectedItem.useTime * reduced;
            }
            if (counter == -1 && projectile.ai[0] != 0 && runOnce)
            {
                runOnce = false;
                counter = -(int)(projectile.ai[0] * firstDelay);
            }
            ChargeAI();
            if (projectile.hide == false)
            {
                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
                projectile.alpha = 0;
            }
            projectile.hide = false;
            projectile.spriteDirection = projectile.direction;
            projectile.position.X = (vector2_1.X - (projectile.width / 2));
            projectile.position.Y = (vector2_1.Y - (projectile.height / 2));
            projectile.rotation = (float)(Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(projectile.direction == 1 ? 72.5f : 107.5f));
            return false;
        }
    }
}