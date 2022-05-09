using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Tide
{
    public class ExtendoBaguette : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Extendo Baguette");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ranged = true;
            Projectile.timeLeft = 20;
            Projectile.hide = true;
            Projectile.alpha = 255;
            Projectile.ownerHitCheck = true;
        }
        float segmentDist = 11f;
        int length = 4;
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            damage += modPlayer.baguetteLength;
            float mult = rotationDifference / MathHelper.ToRadians(20f);
            if (mult > 1)
                mult = 1;
            if(mult < 0)
            {
                knockback *= 0.1f;
                damage = 1;
                return;
            }
            damage = (int)(damage * mult);
            knockback *= mult;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float mult = rotationDifference / MathHelper.ToRadians(20f);
            if (Projectile.hide || mult <= MathHelper.ToRadians(3) || ended)
                return false;
            for (int i = 0; i < length; i++)
            {
                int width = Projectile.width + length;
                if(i == 0 || i == length - 1)
                {
                    width = Projectile.width;
                }
                Vector2 drawPos = Projectile.Center;
                drawPos += new Vector2(i * segmentDist, 0).RotatedBy(Projectile.rotation);
                projHitbox = new Rectangle((int)drawPos.X - width / 2, (int)drawPos.Y - width / 2, width, width);
                if(projHitbox.Intersects(targetHitbox))
                {
                    return true;
                }
            }
            return false;
        }
        bool runOnce = true;
        public void cataloguePos()
        {
            Vector2 current = Projectile.Center;
            current += new Vector2(length * segmentDist - 4, 0).RotatedBy(Projectile.rotation);
            float currentR = Projectile.rotation;
            for (int i = 0; i < trailPos.Length; i++)
            {
                Vector2 previousPosition = trailPos[i];
                trailPos[i] = current;
                current = previousPosition;
                float previousR = rotations[i];
                rotations[i] = currentR;
                currentR = previousR;
            }
        }
        public void checkPos()
        {
            float iterator = 0f;
            Vector2 current = Projectile.Center;
            current += new Vector2(length * segmentDist - 4, 0).RotatedBy(Projectile.rotation);
            for (int i = 0; i < trailPos.Length; i++)
            {
                Vector2 previousPosition = trailPos[i];
                if (current == previousPosition)
                {
                    iterator++;
                }
            }
            //if (iterator >= trailPos.Length)
            //	Projectile.Kill();
        }
        Vector2[] trailPos = new Vector2[20];
        float[] rotations = new float[20];
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Tide/GradientScale").Value;
            Vector2 previousPosition = Projectile.Center + new Vector2(length * segmentDist - 4, 0).RotatedBy(Projectile.rotation);
            for (int k = 1; k < trailPos.Length; k++)
            {
                float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
                scale *= 0.75f + (0.75f * 0.225f * modPlayer.baguetteLength);
                if (trailPos[k] == Vector2.Zero)
                {
                    goto aftertrail;
                }
                Vector2 drawPos = trailPos[k] - Main.screenPosition;
                Vector2 currentPos = trailPos[k];
                Vector2 betweenPositions = previousPosition - currentPos;
                Color color = new Color(85, 55, 17, 0) * (0.5f * ((trailPos.Length - k) / (float)trailPos.Length) + 0.5f) * 0.125f;
                float max = betweenPositions.Length() / (0.25f * scale);
                for (int i = 0; i < max; i++)
                {
                    drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
                    if (trailPos[k] != previousPosition)
                        Main.spriteBatch.Draw(texture2, drawPos, null, color, rotations[k] + MathHelper.ToRadians(180), new Vector2(0, 1), scale, SpriteEffects.None, 0f);
                }
                previousPosition = currentPos;
            }
            aftertrail:
            if (ended)
                return false;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            for (int i = 0; i < length; i++)
            {
                Rectangle frame = new Rectangle(0, Projectile.height, Projectile.width, Projectile.height);
                if(i == 0)
                {
                    frame = new Rectangle(0, Projectile.height * 2, Projectile.width, Projectile.height);
                }
                else if(i >= length - 1)
                {
                    frame = new Rectangle(0, 0, Projectile.width, Projectile.height);
                }
                Vector2 drawPos = Projectile.Center;
                drawPos += new Vector2(i * segmentDist, 0).RotatedBy(Projectile.rotation);
                spriteBatch.Draw(texture, drawPos - Main.screenPosition, frame, lightColor, Projectile.rotation + MathHelper.ToRadians(player.direction != 1 ? -45 : 45), new Vector2(texture.Width / 2, Projectile.height / 2), 1f, player.direction != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0.0f);
            }
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ended);
            writer.Write(length);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ended = reader.ReadBoolean();
            length = reader.ReadInt32();
        }
        float previousRotation = 0f;
        float rotationDifference = 0f;
        bool ended = false;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            if(Projectile.owner == Main.myPlayer)
            {
                length = 4;
                length += modPlayer.baguetteLength;
                Projectile.netUpdate = true;
            }
            if (runOnce)
            {
                rotationDifference = MathHelper.ToRadians(20);
                for (int i = 0; i < trailPos.Length; i++)
                {
                    trailPos[i] = Vector2.Zero;
                }
                runOnce = false;
            }
            if(!Projectile.hide)
            {
                checkPos();
                cataloguePos();
            }
            if (!player.channel && Main.myPlayer == Projectile.owner && Projectile.timeLeft <= 20)
            {
                ended = true;
            }
            if (!ended && Projectile.timeLeft < 20)
                Projectile.timeLeft = 20;
            if (ended)
            {
                Projectile.alpha = 255;
            }
            else
            {
                Vector2 vector2_1 = player.RotatedRelativePoint(player.MountedCenter, true);
                if (Main.myPlayer == Projectile.owner)
                {
                    float num1 = Projectile.velocity.Length() * Projectile.scale;
                    Vector2 vector2_2 = vector2_1;
                    float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                    float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                    if (player.gravDir == -1.0)
                        num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                    float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                    float num6 = num1 / num5;
                    float num7 = num2 * num6;
                    float num8 = num3 * num6;

                    if ((double)num7 != Projectile.velocity.X || (double)num8 != Projectile.velocity.Y)
                        Projectile.netUpdate = true;
                    Projectile.velocity.X = num7;
                    Projectile.velocity.Y = num8;
                    Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);
                }
                if (Projectile.hide == false)
                {
                    player.ChangeDir(Projectile.direction);
                    player.heldProj = Projectile.whoAmI;
                    player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
                    if (!ended && player.itemTime < 2)
                    {
                        player.itemTime = 2;
                        player.itemAnimation = 2;
                    }
                    Projectile.alpha = 0;
                }
                Projectile.hide = false;
                Projectile.spriteDirection = Projectile.direction;
                Projectile.Center = vector2_1;
                Projectile.position += Projectile.velocity;
                previousRotation = Projectile.rotation;
                Projectile.rotation = Projectile.velocity.ToRotation();
                float a = MathHelper.ToDegrees(MathHelper.WrapAngle(Math.Abs(Projectile.rotation - previousRotation)));
                a += (a > 180) ? -360 : (a < -180) ? 360 : 0;
                if (MathHelper.ToRadians(a) > rotationDifference)
                {
                    rotationDifference = MathHelper.ToRadians(a);
                }
                else
                {
                    rotationDifference *= 0.75f;
                }
            }
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return !ended;
        }
    }
}