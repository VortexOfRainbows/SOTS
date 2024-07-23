using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Permafrost;
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
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            counter = reader.ReadInt32();
            recoil = reader.ReadSingle();
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
                if (Projectile.ai[1] <= -1)
                {
                    Projectile.ai[1] = (int)player.HeldItem.mana;
                }
            }
            if (counter >= (int)Projectile.ai[0] && !player.channel)
                ended = true;
            if (!ended || Main.myPlayer != Projectile.owner)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.timeLeft = 12;
            }
            else
            {
                recoil = 0;
                pastRecoil = 0;
                Projectile.Kill();
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
                if(Main.rand.NextBool(3))
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(6.28f));
                    Dust dust = Dust.NewDustDirect(Barrel + new Vector2(-5) + circular * 12 + Projectile.velocity.SafeNormalize(Vector2.Zero) * 8, 0, 0, ModContent.DustType<Dusts.CopyIceDust>(), 0, 0, 0, Color.White);
                    dust.noGravity = true;
                    dust.scale = 1.2f;
                    dust.velocity *= 0.2f;
                    dust.velocity += Projectile.velocity * 0.5f * Main.rand.NextFloat() + circular * 0.6f;
                }
                float windUp = (counter + 30) / (Projectile.ai[0] + 30);
                Vector2 shaking = Main.rand.NextVector2Circular(1, 1) * windUp * 2f;
                offset += shaking;
                if (windUp >= 1)
                {
                    Projectile.netUpdate = true;
                    Shoot(true);
                }
            }
            else
            {
                if(counter % (int)Projectile.ai[0] == 0)
                {
                    Projectile.netUpdate = true;
                    Shoot();
                }
                else
                {
                    if(counter > Projectile.ai[0] * 4.5f)
                    {
                        SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.3f, -0.2f);
                        counter = 0;
                        Projectile.netUpdate = true;
                    }
                    if(counter > Projectile.ai[0] * 2)
                    {
                        if (Main.rand.NextBool(3))
                        {
                            Vector2 circular = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(6.28f));
                            Dust dust = Dust.NewDustDirect(Barrel + new Vector2(-5) + circular * 18 + Projectile.velocity.SafeNormalize(Vector2.Zero) * 12, 0, 0, ModContent.DustType<Dusts.CopyIceDust>(), 0, 0, 0, Color.White);
                            dust.noGravity = true;
                            dust.scale = 1.1f;
                            dust.velocity *= 0.2f;
                            dust.velocity += -Projectile.velocity * 0.05f * Main.rand.NextFloat() - circular * 1.3f;
                            dust.alpha = 150;
                        }
                    }
                    if (Main.rand.NextBool(2))
                    {
                        Dust dust = Dust.NewDustDirect(Barrel + new Vector2(-5) + Projectile.velocity.SafeNormalize(Vector2.Zero) * 12, 0, 0, ModContent.DustType<Dusts.CopyIceDust>(), 0, 0, 0, Color.White);
                        dust.scale = 0.8f;
                        dust.velocity *= 0.1f;
                        dust.alpha = 150;
                    }
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
        public Vector2 Barrel => Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(pastRecoil * Projectile.direction * Projectile.spriteDirection * -1)) * 36;
        private void Shoot(bool ice = false)
        {
            Player player = Main.player[Projectile.owner];
            SOTSUtils.PlaySound(SoundID.Item38, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.3f, -0.2f);
            if (Projectile.owner == Main.myPlayer)
            {
                player.CheckMana((int)Projectile.ai[1], true, false);
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