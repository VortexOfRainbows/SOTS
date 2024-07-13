using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Projectiles.Planetarium;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.BiomeChest
{
    public class Icebreaker : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 72;
            Projectile.height = 30;
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
            Texture2D noIce = ModContent.Request<Texture2D>("SOTS/Projectiles/BiomeChest/IcebreakerNoIce").Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            return false;
        }
        float counter = 0;
        bool ended = false;
        bool runOnce = false;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] <= -1)
            {
                Projectile.ai[0] = player.itemTime;
                Projectile.netUpdate = true;
            }
            if (!ended)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.timeLeft = 2;
            }
            Vector2 center = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float offsetDist = 40 * Projectile.scale;
                Vector2 toMouse = Main.MouseWorld - center;
                toMouse = toMouse.SafeNormalize(Vector2.Zero) * offsetDist;
                if ((double)toMouse.X != Projectile.velocity.X || (double)toMouse.Y != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity = toMouse;
            }
            Vector2 offset = new Vector2(0, -8 * player.gravDir * Projectile.direction).RotatedBy(Projectile.rotation);
            offset.X *= 0.5f;
            counter++;
            if(counter < Projectile.ai[0])
            {
                float windUp = counter / Projectile.ai[0];
                Vector2 shaking = Main.rand.NextVector2Circular(1, 1) * windUp * 2f;
                offset += shaking;
            }
            else
            {
                if (counter >= (int)Projectile.ai[0] && !player.channel)
                    ended = true;
            }
            if(runOnce)
            {
                runOnce = false;
                if (Projectile.owner == Main.myPlayer)
                {
                    Vector2 fireFrom = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 32;
                    SOTSUtils.PlaySound(SoundID.Item5,(int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, -0.1f);
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), fireFrom, Projectile.velocity * 4f, (int)Projectile.ai[1], Projectile.damage, Projectile.knockBack, Main.myPlayer);
                }
            }
            Projectile.spriteDirection = (int)player.gravDir;
            Projectile.rotation = (Projectile.velocity + offset).ToRotation();
            Projectile.Center = center + offset;
            if (Projectile.hide == false)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(player.gravDir * Projectile.rotation + MathHelper.ToRadians(-90)));
            }
            Projectile.hide = false;
            return false;
        }
    }
}