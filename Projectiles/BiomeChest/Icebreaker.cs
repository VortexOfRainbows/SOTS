using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Permafrost;
using System;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.BiomeChest
{
    public class Icebreaker : ModProjectile
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
            if (Projectile.hide)
                return false;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D textureIceOnly = ModContent.Request<Texture2D>("SOTS/Projectiles/BiomeChest/IcebreakerIceOnly").Value;
            if (counter >= Projectile.ai[0])
                texture = ModContent.Request<Texture2D>("SOTS/Projectiles/BiomeChest/IcebreakerNoIce").Value;
            Vector2 drawOrigin = new Vector2(4, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            if(counter > Projectile.ai[0] * 2)
            {
                Color color = new Color(100, 100, 100, 0);
                float percent = (counter - (Projectile.ai[0] * 2)) / Projectile.ai[0] / 2.5f;
                percent = Math.Clamp(percent, 0, 1);
                for(int i = 0; i < 6; i++)
                {
                    Vector2 c = new Vector2(24 * (1 - percent), 0).RotatedBy(MathHelper.TwoPi / 6f * i + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
                    Main.spriteBatch.Draw(textureIceOnly, drawPos + c, null, color * MathF.Sin(percent * MathHelper.Pi) * 0.175f, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
                }
                if(counter > Projectile.ai[0] * 3.5f)
                {
                    percent = (counter - (Projectile.ai[0] * 3.5f)) / Projectile.ai[0];
                    Main.spriteBatch.Draw(textureIceOnly, drawPos, null, lightColor * percent * 1f, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
                }
            }
            return false;
        }
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
                if(recoil <= 2f)
                {
                    recoil = 0;
                    pastRecoil = 0;
                    Projectile.Kill();
                }
                else
                {
                    player.itemTime = 2;
                    player.itemAnimation = 2;
                    Projectile.timeLeft = 12;
                }

            }
            Vector2 center = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float offsetDist = 6 * Projectile.scale;
                Vector2 toMouse = Main.MouseWorld - center;
                toMouse = toMouse.SafeNormalize(Vector2.Zero) * offsetDist;
                if ((double)toMouse.X != Projectile.velocity.X || (double)toMouse.Y != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity = toMouse;
            }
            Vector2 offset = new Vector2(0, -8 * player.gravDir * Projectile.direction).RotatedBy(Projectile.rotation);
            offset.X *= 0.5f;
            counter++;
            recoil *= 0.935f;
            recoil -= 0.06f;
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
            if (Projectile.hide == false)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(player.gravDir * Projectile.rotation + MathHelper.ToRadians(-90)));
            }
            Projectile.hide = false;
            pastRecoil = recoil;
            return false;
        }
        public Vector2 Barrel => Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(pastRecoil * Projectile.direction * Projectile.spriteDirection * -1)) * 36;
        private void Shoot(bool ice = false)
        {
            SOTSUtils.PlaySound(SoundID.Item38, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.3f, -0.2f);
            if (Projectile.owner == Main.myPlayer)
            {
                int type = (int)Projectile.ai[1];
                for(int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Barrel, (Projectile.velocity * 1.15f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-2, 2)) * i) + Main.rand.NextVector2Circular(1, 1) / (i + 1), type, Projectile.damage, Projectile.knockBack, Main.myPlayer);
                }
                if(ice)
                {
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Barrel, Projectile.velocity * 1.4f, ModContent.ProjectileType<IcebreakerIce>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                }
                else
                {
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Barrel, Projectile.velocity * 2.0f, ModContent.ProjectileType<IcebreakerShard>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                }
            }
            if(!ended)
                recoil += 30;
            Projectile.netUpdate = true;
        }
    }
    public class IcebreakerIce : ModProjectile
    {
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.hide)
                return false;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = texture.Size() / 2;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale * 1.1f, Projectile.direction != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 12;
            height = 12;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 90;
            Projectile.width = 34;
            Projectile.height = 30;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = 40;
            hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        public override void OnKill(int timeLeft)
        {
            SOTSUtils.PlaySound(SoundID.Item50, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.4f, 0.5f);
            for (int i = 0; i < 16; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center + new Vector2(-5), 0, 0, ModContent.DustType<Dusts.CopyIceDust>(), 0, 0, 0, Color.White);
                dust.noGravity = true;
                dust.scale = 1.6f;
                dust.velocity *= 1.5f;
                dust.velocity += Projectile.velocity * 0.8f * Main.rand.NextFloat();
            }
            if(Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 velo = (Projectile.velocity * 0.175f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-7.5f, 7.5f)) * i) + Main.rand.NextVector2Circular(2, 2) / (i + 1) - Vector2.UnitY * 1.75f;
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, velo, ModContent.ProjectileType<IcebreakerShard>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                }
            }
        }
        public override void AI()
        {
            if (Projectile.scale != 1.1f)
            {
                for (int i = 0; i < 16; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center + new Vector2(-5), 0, 0, ModContent.DustType<Dusts.CopyIceDust>(), 0, 0, 0, Color.White);
                    dust.noGravity = true;
                    dust.scale = 2f;
                    dust.velocity *= 1.5f;
                    dust.velocity += Projectile.velocity * 0.8f * Main.rand.NextFloat();
                }
                SOTSUtils.PlaySound(SoundID.Item50, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.5f, -0.1f);
                Projectile.scale = 1.1f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            for (int i = 0; i < 3; i++)
            {
                Vector2 spawnPos = Vector2.Lerp(Projectile.Center, Projectile.oldPosition + Projectile.Size / 2, i * 0.34f);
                Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, ModContent.DustType<Dusts.CopyIceDust>(), 0, 0, 0, Color.White);
                dust.noGravity = true;
                dust.scale = 1.15f;
                dust.velocity *= 0.45f;
            }
        }
    }
    public class IcebreakerShard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.IceSpike);
            Projectile.width = 24;
            Projectile.height = 14;
            Projectile.alpha = 0;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.idStaticNPCHitCooldown = 8;
            Projectile.usesIDStaticNPCImmunity = true;
        }
        int counter = 0;
        public override bool PreAI()
        {
            if (Projectile.hide)
            {
                SOTSUtils.PlaySound(SoundID.Item17, (int)Projectile.Center.X, (int)Projectile.Center.Y, .9f, 0.2f);
                Projectile.hide = false;
                Projectile.frame = Projectile.whoAmI % 3;
            }
            return true;
        }
        public override void AI()
        {
            Projectile.scale = 0.9f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            counter++;
            if (counter > 10 || (Projectile.velocity.Y > 0 && counter > 3))
            {
                Projectile.tileCollide = true;
            }
            for (int i = 0; i < 2; i++)
            {
                Vector2 spawnPos = Vector2.Lerp(Projectile.Center, Projectile.oldPosition + Projectile.Size / 2, i * 0.5f);
                Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, ModContent.DustType<Dusts.CopyIceDust>(), 0, 0, 0, Color.White);
                dust.noGravity = true;
                dust.scale = 0.9f;
                dust.velocity *= 0.175f;
                dust.alpha = 100;
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = 16;
            hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 6;
            height = 6;
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < 12; a++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<CopyIceDust>());
                dust.noGravity = true;
                dust.scale *= 1.1f;
                dust.velocity = dust.velocity * 0.6f + Projectile.velocity * 0.4f;
                dust.alpha = 100;
            }
        }
    }
}