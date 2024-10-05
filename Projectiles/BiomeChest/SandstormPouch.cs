using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
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
            Projectile.height = 36;
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
            Texture2D textureGlow = ModContent.Request<Texture2D>("SOTS/Projectiles/BiomeChest/SandstormPouchGlow").Value;
            Vector2 drawOrigin = new Vector2(4, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
           
            Color color = new Color(100, 100, 100, 0);
            float percent = 0.5f;
            percent = Math.Clamp(percent, 0, 1);
            for (int i = 0; i < 6; i++)
            {
                Vector2 c = new Vector2(1.0f + recoil * 0.4f, 0).RotatedBy(MathHelper.TwoPi / 6f * i + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
                Main.spriteBatch.Draw(textureGlow, drawPos + c, null, color, Projectile.rotation, drawOrigin, (Projectile.scale + recoil * 0.02f) * new Vector2(1 + recoil * 0.02f, 1 - recoil * 0.02f), Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
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
                if(recoil <= 1f)
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
            recoil *= 0.9f;
            recoil -= 0.1f;
            recoil = MathHelper.Clamp(recoil, 0, 30);

            if (!Projectile.hide && counter > 0)
            {
                float windUp = MathF.Min(1, counter / Projectile.ai[0]);
                for (int i = 0; i < 2; i++)
                {
                    Vector2 circular = new Vector2(0, 1).RotatedBy(i * MathF.PI + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 6));
                    circular.X *= 0.5f;
                    circular = circular.RotatedBy(Projectile.velocity.ToRotation());
                    Dust dust = Dust.NewDustDirect(Barrel + new Vector2(-5) + circular * 24 * windUp, 0, 0, ModContent.DustType<ModSandDust>(), 0, 0, 0, ColorHelper.SandstormPouchColor * 0.5f);
                    dust.noGravity = true;
                    dust.scale = 1.0f;
                    dust.velocity *= 0.02f + recoil * 0.1f;
                    dust.velocity += player.velocity * 1.15f;
                }
            }

            if (counter == (int)Projectile.ai[0] || counter > Projectile.ai[0] * 1.5f)
            {
                Projectile.netUpdate = true;
                Shoot();
                if(player.HeldItem.ModItem is VoidItem v)
                    v.DrainMana(player);
                if (counter > Projectile.ai[0] * 1.5f)
                    counter -= (int)(Projectile.ai[0] * 0.5f);
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
        public Vector2 Barrel => Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(pastRecoil * Projectile.direction * Projectile.spriteDirection * -1)) * 32;
        private void Shoot()
        {
            SOTSUtils.PlaySound(SoundID.Item34, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.0f, 0.1f);
            if (Projectile.owner == Main.myPlayer)
            {
                int type = ModContent.ProjectileType<SandstormPuff>();
                for(int i = 0; i < 5; i++)
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Barrel, Projectile.velocity * Main.rand.NextFloat(0.8f, 1.2f), type, Projectile.damage, Projectile.knockBack, Main.myPlayer, i * 72f, Main.rand.NextFloat(0.8f, 1.2f));
            }
            if(!ended)
                recoil += 5;
            Projectile.netUpdate = true;
        }
    }
}