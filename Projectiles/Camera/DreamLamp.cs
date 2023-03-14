using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Camera
{
    public class DreamLamp : ModProjectile
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(mousePosition.X);
            writer.Write(mousePosition.Y);
            writer.Write(ended);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            mousePosition.X = reader.ReadSingle();
            mousePosition.Y = reader.ReadSingle();
            ended = reader.ReadBoolean();
        }
        public Vector2 mousePosition = Vector2.Zero;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if (player.itemTime == 0 || player.itemAnimation == 0)
            {
                return false;
            }
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            if(type == 2)
            {
                texture = ModContent.Request<Texture2D>("SOTS/Projectiles/Camera/FalseLamp").Value;
            }
            Vector2 drawOrigin = new Vector2(texture.Width / 2 + Projectile.direction * 10, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale * 0.8f, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<Void.VoidMagic>();
            Projectile.timeLeft = 40;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }
        bool RunOnce = true;
        bool ended = false;
        public int type => (int)Projectile.ai[0];
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 centerOnPlayer = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
            if (RunOnce)
            {
                RunOnce = false;
                Projectile.timeLeft = (int)Projectile.ai[1] + 1;
                if(type == 0)
                    SOTSUtils.PlaySound(SoundID.Item8, centerOnPlayer, 1f, -0.1f);
                else
                    SOTSUtils.PlaySound(SoundID.Item85, centerOnPlayer, 0.8f, -0.25f);
            }
            if((int)Projectile.ai[1] == Projectile.timeLeft && type != 0)
            {
                if(Main.myPlayer == Projectile.owner)
                {
                    Vector2 velo = new Vector2(7, -3 * Projectile.direction).RotatedBy(Projectile.velocity.ToRotation()) + Main.rand.NextVector2Circular(1.2f, 1.0f);
                    Vector2 offset = velo.SafeNormalize(Vector2.Zero) * 60;
                    if (type == 2)
                        offset *= 0.25f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + offset, velo, ModContent.ProjectileType<DreamingSmog>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, type);
                }
            }
            int ID = -1;
            if (type == 0)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Projectile camera = Main.projectile[i];
                    if (camera.active && camera.owner == Projectile.owner && camera.type == ModContent.ProjectileType<DreamingFrame>())
                    {
                        ID = i;
                        break;
                    }
                }
            }
            if(Main.myPlayer == Projectile.owner)
            {
                mousePosition = Main.MouseWorld;
                Projectile.netUpdate = true;
                if(!Main.mouseRight)
                    ended = true;
            }
            if (ID != -1 || (type != 0 && mousePosition != Vector2.Zero))
            {
                Vector2 center = mousePosition;
                if (type == 0 && ID != -1)
                {
                    Projectile camera = Main.projectile[ID];
                    center = camera.Center;
                }
                Projectile.velocity = Projectile.velocity.Length() * (center - player.Center).SafeNormalize(Projectile.velocity.SafeNormalize(new Vector2(0, 1)));
                if (Projectile.hide == false)
                {
                    Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
                    Projectile.alpha = 0;
                }
                if (type == 0)
                    Projectile.timeLeft = 2;
                else
                {
                    if (!ended || Projectile.timeLeft > 2)
                    {
                        if (player.itemTime < 2 || player.itemAnimation < 2)
                        {
                            player.itemTime = 2;
                            player.itemAnimation = 2;
                        }
                    }
                }
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                if (player.itemTime != 0 && player.itemAnimation != 0)
                {
                    player.ChangeDir(Projectile.direction);
                    player.heldProj = Projectile.whoAmI;
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + MathHelper.Pi);
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + MathHelper.Pi);
                }
                else
                    Projectile.Kill();
                Projectile.hide = false;
                Projectile.spriteDirection = Projectile.direction;
                Projectile.Center = centerOnPlayer;
            }
            else
            {
                Projectile.Kill();
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
        }
    }
}