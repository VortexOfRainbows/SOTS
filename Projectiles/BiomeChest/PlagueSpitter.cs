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
using System.Text.RegularExpressions;
using System.Threading;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.BiomeChest
{
    public class PlagueSpitter : ModProjectile
    {
        public static Color SpitterColor
        { 
            get
            {
                return Color.Lerp(ColorHelpers.ToothAcheLime, ToothAcheSlash.toothAcheGreen, Main.rand.NextFloat(1f));
            }
        }
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
            if (ended)
                Projectile.direction = Projectile.oldDirection;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = texture.Size() /2;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation + MathHelper.PiOver4 * Projectile.direction * Projectile.spriteDirection, drawOrigin, Projectile.scale * 0.92f, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            DrawPlagueBalls();
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
        public void DrawPlagueBalls()
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
            for (int j = 0; j < Projectile.ai[2]; j++)
            {
                Vector2 previous = Vector2.Zero;
                for (int k = 0; k < 12; k++)
                {
                    Vector2 circular = PlagueBallPosition(j, k);
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
        }
        public Vector2 PlagueBallPosition(int i, int k)
        {
            float trailLength = 6;
            int groups = 3;
            int cirNum = (i / groups);
            Vector2 circular = new Vector2(10 + cirNum * 5, 0).RotatedBy(MathHelper.ToRadians((SOTSWorld.GlobalCounter - k * trailLength) * 1.5f + i * (360f / groups) + cirNum * 30f));
            circular.Y *= 0.4f;
            circular = circular.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
            return circular;
        }
        private bool skipDrawingFirstFrame = true;
        private int counter = 0;
        private bool ended = false;
        private float recoil = 0;
        private float pastRecoil = 0;
        private int LastDir = 0;
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
            bool notEnoughMana = !player.CheckMana((int)Projectile.ai[1], false, false);
            if ((Projectile.ai[2] > 1 || (Projectile.ai[2] == 1 && counter > Projectile.ai[0] / 1.5f)) && (!player.channel || notEnoughMana))
                ended = true;
            if (ended)
                Projectile.direction = Projectile.oldDirection;
            if(Main.myPlayer != Projectile.owner)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
            }
            if (!ended)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.timeLeft = 12;
            }
            else
            {
                if ((player.channel || player.controlUseItem) && !notEnoughMana)
                {
                    player.channel = true;
                    Projectile.netUpdate = true;
                    ended = false;
                    counter = -(int)(Projectile.ai[0] / 2);
                }
                else
                {
                    if (Projectile.ai[2] <= 0 && recoil <= 2f)
                    {
                        recoil = 0;
                        pastRecoil = 0;
                        Projectile.Kill();
                    }
                    else
                    {
                        if (Projectile.ai[2] > 0)
                        {
                            counter++;
                            if (counter > 0)
                            {
                                counter = -(int)(Projectile.ai[0] / 2);
                                Shoot();
                            }
                        }
                        player.itemTime = 2;
                        player.itemAnimation = 2;
                        Projectile.timeLeft = 12;
                    }
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
            if(!ended)
            {
                counter++;
            }
            recoil *= 0.96f;
            recoil -= 0.5f;
            recoil = MathHelper.Clamp(recoil, 0, 45f);
            if (counter <= Projectile.ai[0])
            {
                if (!ended)
                {
                    float rampRate = 1 - Projectile.ai[2] / 30f;
                    if (Projectile.ai[2] == 30)
                        rampRate = 0;
                    if (Main.rand.NextBool(4) || (rampRate > 0 && Main.rand.NextFloat() > rampRate))
                    {
                        Vector2 circular = new Vector2(1 + Projectile.ai[2] / 9f, 0).RotatedBy(Main.rand.NextFloat(6.28f));
                        circular.Y *= 0.5f;
                        circular = circular.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
                        Dust dust = Dust.NewDustDirect(Barrel + new Vector2(-5) + circular * 18, 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, SpitterColor);
                        dust.noGravity = true;
                        dust.scale = 1.0f;
                        dust.velocity *= 0.2f;
                        dust.velocity += Projectile.velocity * 0.075f * Main.rand.NextFloat() - circular * 1.4f + player.velocity;
                        dust.fadeIn = 9f;
                        dust.color.A = 0;
                    }
                    float windUp = (counter + 30) / (Projectile.ai[0] + 30);
                    Vector2 shaking = Main.rand.NextVector2Circular(1, 1) * windUp * 0.5f;
                    offset += shaking;
                    if (windUp >= 1)
                    {
                        Projectile.netUpdate = true;
                        GatherCharge();
                        counter = 0;
                    }
                }
                for (int i = 0; i < Projectile.ai[2]; i++)
                {
                    if (Main.rand.NextBool(30))
                    {
                        Vector2 away = PlagueBallPosition((int)i, 0);
                        Vector2 ballPosition = away + Barrel;
                        Dust dust = Dust.NewDustDirect(ballPosition + new Vector2(-5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, SpitterColor);
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                        dust.velocity *= 0.2f;
                        dust.velocity += Projectile.velocity * 0.01f * Main.rand.NextFloat() - away.SafeNormalize(Vector2.Zero) * 0.1f + player.velocity;
                        dust.fadeIn = 10f;
                        dust.color.A = 0;
                    }
                }
            }
            Projectile.spriteDirection = (int)player.gravDir;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(pastRecoil * Projectile.direction * player.gravDir * -1);
            Projectile.Center = center + offset - Projectile.velocity + Projectile.velocity.RotatedBy(MathHelper.ToRadians(pastRecoil * Projectile.direction * player.gravDir * -1));
            if(!skipDrawingFirstFrame)
            {
                if(!ended)
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
            if (Projectile.ai[2] < 30)
            {
                for (int i = 0; i < 12; i++)
                {
                    Vector2 away = PlagueBallPosition((int)Projectile.ai[2], i);
                    Vector2 ballPosition = away + Barrel;
                    Dust dust = Dust.NewDustDirect(ballPosition + new Vector2(-5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, SpitterColor);
                    dust.noGravity = true;
                    dust.scale = 2f - (i != 0 ? 1f : 0);
                    dust.velocity *= 0.0f;
                    dust.velocity += away.SafeNormalize(Vector2.Zero) * 0.5f + player.velocity;
                    dust.fadeIn = 7f;
                    dust.color.A = 0;
                }
                SOTSUtils.PlaySound(SoundID.Item20, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1f, -0.1f);
                player.CheckMana((int)Projectile.ai[1], true, false);
                Projectile.ai[2]++;
                if (Main.myPlayer == Projectile.owner)
                    Projectile.netUpdate = true;
            }
        }
        private void Shoot()
        {
            Player player = Main.player[Projectile.owner];
            SOTSUtils.PlaySound(SoundID.NPCDeath9, (int)player.Center.X, (int)player.Center.Y, 1f, -0.5f);
            Vector2 includingRecoil = (Barrel - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
            Vector2 ballPosition = PlagueBallPosition((int)Projectile.ai[2] - 1, 0) + Barrel;
            if (Projectile.owner == Main.myPlayer)
            {
                int type = ModContent.ProjectileType<PlagueBeam>();
                Vector2 velo = includingRecoil + Main.rand.NextVector2Circular(1, 1) * 0.2f;
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Barrel, velo, type, Projectile.damage, Projectile.knockBack, Main.myPlayer);
                Projectile.netUpdate = true;
            }
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(ballPosition + new Vector2(-5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, SpitterColor);
                dust.noGravity = true;
                if(i < 5)
                {
                    dust.scale = 1.5f;
                    dust.velocity *= 0.4f;
                }
                else
                {
                    dust.scale = 1.0f;
                    dust.velocity *= 0.1f;
                    dust.velocity += includingRecoil * 0.5f * Main.rand.NextFloat() + player.velocity;
                }
                dust.fadeIn = 12f;
                dust.color.A = 0;
            }
            Projectile.ai[2]--;
            float percent = Projectile.ai[2] / 30f;
            float recoilBonus = MathF.Sin(percent * MathHelper.Pi);
            recoil *= 1.16f;
            recoil += 4 + 4 * recoilBonus;
            if (Projectile.ai[2] <= 0)
                recoil += 26;
        }
    }
    public class PlagueBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = 12;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.hide = true;
        }
        Vector2 finalPosition = Vector2.Zero;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreAI()
        {
            if(!hasInit)
                InitializeLaser();
            return true;
        }
        public override void AI()
        {
            Projectile.hide = false;
        }
        bool hasInit = false;
        public void InitializeLaser()
        {
            Color color = PlagueSpitter.SpitterColor;   
            color.A = 0;
            Vector2 startingPosition = Projectile.Center;
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
            for (int b = 0; b < 640; b++)
            {
                startingPosition += Projectile.velocity * 2.5f;
                finalPosition = startingPosition;
                int i = (int)startingPosition.X / 16;
                int j = (int)startingPosition.Y / 16;
                if (WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(i, j))
                {
                    break;
                }
                Rectangle projHit = new Rectangle((int)startingPosition.X - 4, (int)startingPosition.Y - 4, 8, 8);
                bool hasCollided = false;
                for (int ID = 0; ID < Main.npc.Length; ID++)
                {
                    NPC npc = Main.npc[ID];
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                    {
                        if (npc.Hitbox.Intersects(projHit))
                        {
                            hasCollided = true;
                            break;
                        }
                    }
                }
                if (hasCollided)
                {
                    break;
                }
                int chance = SOTS.Config.lowFidelityMode ? 20 : 5;
                if (Main.rand.NextBool(chance))
                {
                    Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color);
                    dust.scale = 1.0f;
                    dust.velocity *= 0.33f;
                    dust.velocity += Projectile.velocity * Main.rand.NextFloat(1f, 4f);
                    dust.fadeIn = 8;
                }
            }
            for (int i = 10; i > 0; i--)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color, 1f);
                dust.noGravity = true;
                dust.velocity = dust.velocity * Main.rand.NextFloat() * 0.5f + Projectile.velocity * Main.rand.NextFloat(0f, 8f);
                dust.fadeIn = 8;
                dust.scale = Main.rand.Next(5, 8) / 4f;
            }
            for (int i = 15; i > 0; i--)
            {
                Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color, 1f);
                dust.noGravity = true;
                dust.velocity = dust.velocity * Main.rand.NextFloat() + Projectile.velocity * Main.rand.NextFloat(0.0f, 4f);
                dust.fadeIn = 4;
                dust.scale = Main.rand.Next(4, 9) / 4f;
            }
            hasInit = true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle projHit = new Rectangle((int)finalPosition.X - 6, (int)finalPosition.Y - 6, 12, 12);
            if (targetHitbox.Intersects(projHit))
            {
                return true;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!hasInit)
                return false;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = new Vector2(0, 1);
            float rotation = Projectile.velocity.ToRotation();
            Vector2 start = Projectile.Center;
            Vector2 final = finalPosition;
            float xScale = Vector2.Distance(start, final) / texture.Width;
            Color color = new Color(100, 100, 100, 0);
            color.A = 0;
            float percent = Projectile.timeLeft / 12f;
            float percentRoot = MathF.Sqrt(percent);
            for (int i = 0; i <= 8; i++)
            {
                float colorScale = 0.1f;
                Vector2 sinusoid = new Vector2(0, 2 + i % 2 * 2).RotatedBy(i * MathHelper.PiOver4 + SOTSWorld.GlobalCounter * MathHelper.TwoPi / 45f);
                if (i == 0)
                {
                    sinusoid *= 0f;
                    colorScale = 1f;
                }
                Main.spriteBatch.Draw(texture, sinusoid + start - Main.screenPosition, null, color * colorScale * percent * 1.25f, rotation, origin, new Vector2(xScale, 0.7f * percentRoot), SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}