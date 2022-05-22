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
            Projectile.width = 30;
            Projectile.height = 64;
            Projectile.aiStyle = 20;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
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
            return false;
        }
        const int fireFromDist = 30;
        const int fireFromTighten = 12;
        int textureHeight = 10;
        public void DrawArrows(SpriteBatch spriteBatch, Color drawColor)
        {
            if (Projectile.ai[0] != 0 && counter < Projectile.ai[0])
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
                float chargePercent = counter / Projectile.ai[0];
                if (chargePercent < 0)
                    chargePercent = 0;
                float scale = 1f;
                if (scale > 1)
                    scale = 1;
                Vector2 away = Projectile.velocity.SafeNormalize(Vector2.Zero);
                Vector2 fireFrom = Projectile.Center + away * (fireFromDist - textureHeight - chargePercent * fireFromTighten);
                spriteBatch.Draw(texture, fireFrom - Main.screenPosition, null, drawColor * additionalAlphaMult, (Projectile.velocity.SafeNormalize(Vector2.Zero) + away).ToRotation() + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
        }
        float counter = -1;
        bool ended = false;
        bool runOnce = true;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Item selectedItem = player.inventory[Main.player[Projectile.owner].selectedItem];
            if (counter >= (int)Projectile.ai[0])
                ended = true;
            if (!ended)
            {
                player.itemTime = 20;
                player.itemAnimation = 20;
                Projectile.timeLeft = 20;
            }
            Vector2 vector2_1 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
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
                Projectile.ai[0] = selectedItem.useTime * reduced;
            }
            if (counter == -1)
            {
                counter = 0;
            }
            if(counter < (int)Projectile.ai[0])
                counter++;
            else if(runOnce)
            {
                runOnce = false;
                if (Projectile.owner == Main.myPlayer)
                {
                    Vector2 fireFrom = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - textureHeight);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 5, 1.2f, -0.1f);
                    Projectile proj = Projectile.NewProjectileDirect(fireFrom, Projectile.velocity * 4f, (int)Projectile.ai[1], Projectile.damage, Projectile.knockBack, Main.myPlayer);
                    proj.GetGlobalProjectile<SOTSProjectile>().affixID = -1; //this sould sync automatically on the SOTSProjectile end
                }
            }
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
            Projectile.rotation = (float)(Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X)) + 1.57f;
            return false;
        }
    }
}