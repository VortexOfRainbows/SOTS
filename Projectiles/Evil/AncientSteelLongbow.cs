using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Otherworld;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Evil
{
    public class AncientSteelLongbow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Steel Bow");
        }
        public override void SetDefaults()
        {
            projectile.width = 30;
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
        const int fireFromDist = 30;
        const int fireFromTighten = 12;
        int textureHeight = 10;
        public void DrawArrows(SpriteBatch spriteBatch, Color drawColor)
        {
            if (projectile.ai[0] != 0 && counter < projectile.ai[0])
            {
                int arrowType = (int)projectile.ai[1];
                if(!Main.projectileLoaded[arrowType])
                {
                    Main.instance.LoadProjectile(arrowType);
                }
                Texture2D texture = Main.projectileTexture[arrowType];
                if (arrowType == ModContent.ProjectileType<HardlightArrow>() || arrowType == ModContent.ProjectileType<ChargedHardlightArrow>())
                {
                    texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/HardlightArrowShaft").Value;
                }
                textureHeight = texture.Height / 2 + 2;
                float additionalAlphaMult = 1;
                float chargePercent = counter / projectile.ai[0];
                if (chargePercent < 0)
                    chargePercent = 0;
                float scale = 1f;
                if (scale > 1)
                    scale = 1;
                Vector2 away = projectile.velocity.SafeNormalize(Vector2.Zero);
                Vector2 fireFrom = projectile.Center + away * (fireFromDist - textureHeight - chargePercent * fireFromTighten);
                spriteBatch.Draw(texture, fireFrom - Main.screenPosition, null, drawColor * additionalAlphaMult, (projectile.velocity.SafeNormalize(Vector2.Zero) + away).ToRotation() + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
        }
        float counter = -1;
        bool ended = false;
        bool runOnce = true;
        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            Item selectedItem = player.inventory[Main.player[projectile.owner].selectedItem];
            if (counter >= (int)projectile.ai[0])
                ended = true;
            if (!ended)
            {
                player.itemTime = 20;
                player.itemAnimation = 20;
                projectile.timeLeft = 20;
            }
            Vector2 vector2_1 = Main.player[projectile.owner].RotatedRelativePoint(Main.player[projectile.owner].MountedCenter, true);
            if (Main.myPlayer == projectile.owner)
            {
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
                projectile.ai[0] = selectedItem.useTime * reduced;
            }
            if (counter == -1)
            {
                counter = 0;
            }
            if(counter < (int)projectile.ai[0])
                counter++;
            else if(runOnce)
            {
                runOnce = false;
                if (projectile.owner == Main.myPlayer)
                {
                    Vector2 fireFrom = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - textureHeight);
                    SoundEngine.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 5, 1.2f, -0.1f);
                    Projectile proj = Projectile.NewProjectileDirect(fireFrom, projectile.velocity * 4f, (int)projectile.ai[1], projectile.damage, projectile.knockBack, Main.myPlayer);
                    proj.GetGlobalProjectile<SOTSProjectile>().affixID = -1; //this sould sync automatically on the SOTSProjectile end
                }
            }
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
            projectile.rotation = (float)(Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X)) + 1.57f;
            return false;
        }
    }
}