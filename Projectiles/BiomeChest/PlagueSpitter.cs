using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using SOTS.Dusts;
using SOTS.Items.Evil;
using SOTS.Items.Permafrost;
using SOTS.Projectiles.Blades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.BiomeChest
{
    public class PlagueSpitter : ModProjectile
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(counter);
            writer.Write(recoil);
            writer.Write(ended);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            counter = reader.ReadInt32();
            recoil = reader.ReadSingle();
            ended = reader.ReadBoolean();
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 20;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 120;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (skipDrawingFirstFrame)
                return false;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = texture.Size() /2;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation + MathHelper.PiOver4 * Projectile.direction * Projectile.spriteDirection, drawOrigin, Projectile.scale * 0.92f, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            DrawPlagueBalls(lightColor);
            //if(counter > Projectile.ai[0] * 2)
            //{
            //    Color color = new Color(100, 100, 100, 0);
            //    float percent = (counter - (Projectile.ai[0] * 2)) / Projectile.ai[0] / 2.5f;
            //    percent = Math.Clamp(percent, 0, 1);
            //    for(int i = 0; i < 6; i++)
            //    {
            //        Vector2 c = new Vector2(24 * (1 - percent), 0).RotatedBy(MathHelper.TwoPi / 6f * i + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
            //        Main.spriteBatch.Draw(textureIceOnly, drawPos + c, null, color * MathF.Sin(percent * MathHelper.Pi) * 0.175f, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            //    }
            //    if(counter > Projectile.ai[0] * 3.5f)
            //    {
            //        percent = (counter - (Projectile.ai[0] * 3.5f)) / Projectile.ai[0];
            //        Main.spriteBatch.Draw(textureIceOnly, drawPos, null, lightColor * percent * 1f, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            //    }
            //}
            return false;
        }
        public void DrawPlagueBalls(Color lightColor)
        {
            if (Projectile.ai[2] <= 0)
                return;
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/Projectiles/BiomeChest/PlagueBall").Value;
            Texture2D textureT = ModContent.Request<Texture2D>("SOTS/Projectiles/BiomeChest/PlagueBeam").Value;
            Vector2 drawOrigin = texture.Size() / 2;
            Vector2 drawOriginT = new Vector2(0, textureT.Height/ 2);
            Vector2 center = Barrel;
            Vector2 drawPos = center - Main.screenPosition;
            Color c = new Color(100, 100, 100, 0);
            float trailLength = 6;
            int groups = 3;
            for (int j = 0; j < Projectile.ai[2]; j++)
            {
                Vector2 previous = Vector2.Zero;
                int cirNum = (j / groups);
                for (int k = 0; k < 12; k++)
                {
                    int i = j;
                    Vector2 circular = new Vector2(10 + cirNum * 5, 0).RotatedBy(MathHelper.ToRadians((SOTSWorld.GlobalCounter - k * trailLength) * 1.5f + i * (360f / groups) + cirNum * 30f));
                    circular.Y *= 0.4f;
                    circular = circular.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
                    Vector2 pos = drawPos + circular;
                    float r = 0;
                    if(k == 0)
                    {
                        Main.spriteBatch.Draw(texture, pos, null, c, r * 2f, drawOrigin, Projectile.scale * 0.45f, SpriteEffects.None, 0f);
                        Main.spriteBatch.Draw(texture, pos, null, Color.Lerp(c, Color.White, 0.5f), r * 2f, drawOrigin, Projectile.scale * 0.3f, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        Vector2 toPrev = previous - pos;
                        float len = toPrev.Length();
                        Main.spriteBatch.Draw(textureT, pos, null, Color.Lerp(c, Color.Black, 0.1f) * 0.75f * (1 - k / 12f), toPrev.ToRotation(), drawOriginT, new Vector2(len / textureT.Width * 1.0f, .5f - k / 24f), SpriteEffects.None, 0f);
                    }
                    previous = pos;
                }
            }
            //for(int i = 0; i < 3; i++)
            //{
            //    float rot = MathHelper.ToRadians(SOTSWorld.GlobalCounter * (1 + i * 0.5f)) * (i % 2 * 2 - 1);
            //    Main.spriteBatch.Draw(texture, drawPos, null, Color.Lerp(c, Color.Black, 0.4f - i * 0.2f) * (1 - i * 0.15f), rot, drawOrigin, Projectile.scale * 0.75f * (1 + 0.25f * i), SpriteEffects.None, 0f);
            //}
        }
        private bool skipDrawingFirstFrame = true;
        private int counter = 0;
        private bool ended = false;
        private float recoil = 0;
        private float pastRecoil = 0;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                if (Projectile.ai[0] <= -1)
                {
                    Projectile.ai[0] = (int)player.itemTime;
                    counter = -(int)(Projectile.ai[0] / 2);
                    Projectile.netUpdate = true;
                }
                if (Projectile.ai[1] != player.HeldItem.mana)
                {
                    Projectile.ai[1] = (int)player.HeldItem.mana;
                    Projectile.netUpdate = true;
                }
            }
            if ((Projectile.ai[2] > 1 || (Projectile.ai[2] == 1 && counter > Projectile.ai[0] / 1.5f)) && !player.channel)
                ended = true;
            if (!ended || Main.myPlayer != Projectile.owner)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.timeLeft = 12;
            }
            else
            {
                if (player.channel || player.controlUseItem)
                {
                    player.channel = true;
                    Projectile.netUpdate = true;
                    ended = false;
                }
                else
                {
                    recoil = 0;
                    pastRecoil = 0;
                    Projectile.Kill();
                }
            }
            Vector2 center = player.RotatedRelativePoint(player.MountedCenter + new Vector2(4 * -player.direction, 0), true);
            if (Main.myPlayer == Projectile.owner)
            {
                float offsetDist = 12 * Projectile.scale;
                Vector2 toMouse = Main.MouseWorld - center;
                toMouse = toMouse.SafeNormalize(Vector2.Zero) * offsetDist;
                if ((double)toMouse.X != Projectile.velocity.X || (double)toMouse.Y != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity = toMouse;
            }
            Vector2 offset = new Vector2(0, 2 * player.gravDir * Projectile.direction).RotatedBy(Projectile.rotation);
            counter++;
            recoil *= 0.945f;
            recoil -= 0.05f;
            recoil = MathHelper.Clamp(recoil, 0, 45);
            if (counter <= Projectile.ai[0])
            {
                float rampRate = 1 - Projectile.ai[2] / 18f;
                if (Projectile.ai[2] == 18)
                    rampRate = 0;
                if (Main.rand.NextBool(4) || (rampRate > 0 && Main.rand.NextFloat() > rampRate))
                {
                    Vector2 circular = new Vector2(1 + Projectile.ai[2] / 9f, 0).RotatedBy(Main.rand.NextFloat(6.28f));
                    circular.Y *= 0.5f;
                    circular = circular.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
                    Dust dust = Dust.NewDustDirect(Barrel + new Vector2(-5) + circular * 18, 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, ToothAcheSlash.toothAcheLime);
                    dust.noGravity = true;
                    dust.scale = 1.0f;
                    dust.velocity *= 0.2f;
                    dust.velocity += Projectile.velocity * 0.075f * Main.rand.NextFloat() - circular * 1.4f;
                    dust.fadeIn = 9f;
                    dust.color.A = 0;
                }
                float windUp = (counter + 30) / (Projectile.ai[0] + 30);
                Vector2 shaking = Main.rand.NextVector2Circular(1, 1) * windUp * 2f;
                offset += shaking;
                if (windUp >= 1)
                {
                    Projectile.netUpdate = true;
                    GatherCharge();// Shoot(true);
                    counter = 0;
                }
            }
            Projectile.spriteDirection = (int)player.gravDir;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(pastRecoil * Projectile.direction * player.gravDir * -1);
            Projectile.Center = center + offset - Projectile.velocity + Projectile.velocity.RotatedBy(MathHelper.ToRadians(pastRecoil * Projectile.direction * player.gravDir * -1));
            if(!skipDrawingFirstFrame)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(player.gravDir * Projectile.rotation + MathHelper.ToRadians(-90)));
            }
            skipDrawingFirstFrame = false;
            pastRecoil = recoil;
            return false;
        }
        public Vector2 Barrel => Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(pastRecoil * Projectile.direction * Projectile.spriteDirection * -1)) * 24;
        private void GatherCharge()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[2] < 18)
            {
                SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.9f, -0.2f);
                player.CheckMana((int)Projectile.ai[1], true, false);
                Projectile.ai[2]++;
                if (Main.myPlayer == Projectile.owner)
                    Projectile.netUpdate = true;
            }
        }
        private void Shoot(bool ice = false)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int type = ModContent.ProjectileType<Pathogen>();
                for(int i = 0; i < 1; i++)
                {
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Barrel, (Projectile.velocity * 1.15f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-2, 2)) * i) + Main.rand.NextVector2Circular(1, 1) / (i + 1), type, Projectile.damage, Projectile.knockBack, Main.myPlayer);
                }
            }
            if(!ended)
                recoil += 30;
            Projectile.netUpdate = true;
        }
    }
}