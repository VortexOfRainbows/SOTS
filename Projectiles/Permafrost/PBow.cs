using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Metrics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{
    public class PBow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 114;
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
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            DrawArrows(Main.spriteBatch, lightColor);
            return false;
        }
        private int fireFromDist = 48;
        private int fireFromTighten = 22;
        private int textureHeight = 10;
        public void DrawArrows(SpriteBatch spriteBatch, Color drawColor)
        {
            if (Projectile.ai[0] != 0 && counter < Projectile.ai[0])
            {
                Texture2D texture= Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/PBowArrow").Value;
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
        private float counter = -1;
        private bool ended = false;
        private bool runOnce = true;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                if (Projectile.ai[0] <= -1)
                {
                    Projectile.ai[0] = (int)player.itemTime;
                    Projectile.netUpdate = true;
                    counter = 0;
                }
            }
            if (counter >= (int)Projectile.ai[0])
            {
                if (!player.channel)
                    ended = true;
            }
            if (!ended)
            {
                player.itemTime = 20;
                player.itemAnimation = 20;
                Projectile.timeLeft = 20;
            }
            Vector2 center = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float offsetDist = 14 * Projectile.scale;
                Vector2 toMouse = Main.MouseWorld - center;
                toMouse = toMouse.SafeNormalize(Vector2.Zero) * offsetDist;
                if ((double)toMouse.X != Projectile.velocity.X || (double)toMouse.Y != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity = toMouse;
            }
            if(counter < (int)Projectile.ai[0])
                counter++;
            else
            {
                SOTSUtils.PlaySound(SoundID.Item5, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, -0.1f);
                if (Projectile.owner == Main.myPlayer)
                {
                    Vector2 fireFrom = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - textureHeight);
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), fireFrom, Projectile.velocity * 2f, ModContent.ProjectileType<PBowArrow>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, (int)Projectile.ai[0]);
                }
                counter = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (!Projectile.hide)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(player.gravDir * Projectile.rotation - MathHelper.ToRadians(90)));
            }
            Projectile.hide = false;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.Center = center;
            Projectile.rotation += 1.57f;
            return false;
        }
    }
    public class PBowArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(1);
            Projectile.aiStyle = 1;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 3000;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            if (runOnce)
            {
                for (int i = 0; i < 20; i++)
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Electric);
                    Main.dust[dust].scale *= 1f;
                    Main.dust[dust].velocity += Projectile.velocity * 0.1f;
                    Main.dust[dust].noGravity = true;
                }
                runOnce = false;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            return false;
        }
        public override void OnKill(int timeLeft)
        {

        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 12;
            height = 12;
            return true;
        }
    }
}