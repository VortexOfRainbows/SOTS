using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.BiomeChest
{
    public class SandstormPouch : ModProjectile
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
            Projectile.width = 28;
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
            //Texture2D textureIceOnly = ModContent.Request<Texture2D>("SOTS/Projectiles/BiomeChest/IcebreakerIceOnly").Value;
            //if (counter >= Projectile.ai[0])
            //    texture = ModContent.Request<Texture2D>("SOTS/Projectiles/BiomeChest/IcebreakerNoIce").Value;
            Vector2 drawOrigin = new Vector2(4, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
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
                    counter = 0;
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
                if(recoil <= 0.1f)
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
                float offsetDist = 5 * Projectile.scale;
                Vector2 toMouse = Main.MouseWorld - center;
                toMouse = toMouse.SafeNormalize(Vector2.Zero) * offsetDist;
                if ((double)toMouse.X != Projectile.velocity.X || (double)toMouse.Y != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity = toMouse;
            }
            Vector2 offset = new Vector2(0, -4 * player.gravDir * Projectile.direction).RotatedBy(Projectile.rotation);
            offset.X *= 0.5f;
            counter++;
            recoil *= 0.925f;
            recoil -= 0.05f;
            recoil = MathHelper.Clamp(recoil, 0, 30);

            if (Main.rand.NextBool(3))
            {
                Vector2 circular = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(6.28f));
                Dust dust = Dust.NewDustDirect(Barrel + new Vector2(-5) + circular * 12 + Projectile.velocity.SafeNormalize(Vector2.Zero) * 8, 0, 0, ModContent.DustType<Dusts.ModSandDust>(), 0, 0, 0, Color.White);
                dust.noGravity = true;
                dust.scale = 1.2f;
                dust.velocity *= 0.2f;
                dust.velocity += Projectile.velocity * 0.5f * Main.rand.NextFloat() + circular * 0.6f;
            }
            float windUp = counter / Projectile.ai[0];
            if (windUp > 1)
                windUp = 1;
            Vector2 shaking = Main.rand.NextVector2Circular(1, 1) * windUp * 1f;
            offset += shaking;
            if (windUp >= 1 && (counter % (int)Projectile.ai[0] == 0 && counter > 0))
            {
                Projectile.netUpdate = true;
                Shoot();
                if(player.HeldItem.ModItem is VoidItem v)
                    v.DrainMana(player);
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
        private void Shoot()
        {
            SOTSUtils.PlaySound(SoundID.Item34, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.0f, 0.1f);
            if (Projectile.owner == Main.myPlayer)
            {
                int type = ModContent.ProjectileType<SandstormPuff>();
                for(int i = 0; i < 5; i++)
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Barrel, Projectile.velocity * 0.8f, type, Projectile.damage, Projectile.knockBack, Main.myPlayer, i * 72f);
            }
            if(!ended)
                recoil += 10;
            Projectile.netUpdate = true;
        }
    }
}