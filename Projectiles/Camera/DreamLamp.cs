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
        /*public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/EarthshakerGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            for (int i = 0; i < 4; i++)
            {
                Vector2 addition = Main.rand.NextVector2Circular(1, 1);
                Main.spriteBatch.Draw(texture, drawPos + addition, null, new Color(110, 105, 100, 0), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
        }*/
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
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
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ended);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ended = reader.ReadBoolean();
        }
        bool ended = false;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 centerOnPlayer = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
            int ID = -1;
            for(int i = 0; i < 1000; i++)
            {
                Projectile camera = Main.projectile[i];
                if(camera.active && camera.owner == Projectile.owner && camera.type == ModContent.ProjectileType<DreamingFrame>())
                {
                    ID = i;
                    break;
                }
            }
            if(ID != -1)
            {
                Projectile camera = Main.projectile[ID];
                Projectile.velocity = Projectile.velocity.Length() * (camera.Center - player.Center).SafeNormalize(Projectile.velocity.SafeNormalize(new Vector2(0, 1)));
                if (Projectile.hide == false)
                {
                    Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
                    Projectile.alpha = 0;
                }
                Projectile.timeLeft = 2;
                Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57000005245209);
                if (player.itemTime != 0 && player.itemAnimation != 0)
                {
                    player.ChangeDir(Projectile.direction);
                    player.heldProj = Projectile.whoAmI;
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + MathHelper.Pi);
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + MathHelper.Pi);
                }
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