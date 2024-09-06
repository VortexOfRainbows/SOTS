using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using System.Diagnostics.Metrics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{
    public class PBow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 114;
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
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            DrawArrows(Main.spriteBatch, lightColor);
            return false;
        }
        private int fireFromDist = 48;
        private int fireFromTighten = 22;
        private int textureHeight = 10;
        public void DrawArrows(SpriteBatch spriteBatch, Color drawColor)
        {
            if (Projectile.ai[0] != 0 && counter < Projectile.ai[0])
            {
                Texture2D texture= Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/PBowArrow").Value;
                textureHeight = texture.Height / 2 + 2;
                float additionalAlphaMult = 1;
                float chargePercent = counter / Projectile.ai[0];
                if (chargePercent < 0)
                    chargePercent = 0;
                float scale = 1f;
                if (scale > 1)
                    scale = 1;
                Vector2 away = Projectile.velocity.SafeNormalize(Vector2.Zero);
                Vector2 fireFrom = Projectile.Center + away * (fireFromDist - textureHeight - chargePercent * fireFromTighten);
                spriteBatch.Draw(texture, fireFrom - Main.screenPosition, null, drawColor * additionalAlphaMult, (Projectile.velocity.SafeNormalize(Vector2.Zero) + away).ToRotation() + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
        }
        private float counter = -1;
        private bool ended = false;
        private bool runOnce = true;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                if (Projectile.ai[0] <= -1)
                {
                    Projectile.ai[0] = (int)player.itemTime;
                    Projectile.netUpdate = true;
                    counter = 0;
                }
            }
            if (counter >= (int)Projectile.ai[0])
            {
                if (!player.channel)
                    ended = true;
            }
            if (!ended)
            {
                player.itemTime = 20;
                player.itemAnimation = 20;
                Projectile.timeLeft = 20;
            }
            Vector2 center = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float offsetDist = 14 * Projectile.scale;
                Vector2 toMouse = Main.MouseWorld - center;
                toMouse = toMouse.SafeNormalize(Vector2.Zero) * offsetDist;
                if ((double)toMouse.X != Projectile.velocity.X || (double)toMouse.Y != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity = toMouse;
            }
            if(counter < (int)Projectile.ai[0])
                counter++;
            else
            {
                SOTSUtils.PlaySound(SoundID.Item5, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, -0.1f);
                if (Projectile.owner == Main.myPlayer)
                {
                    Vector2 fireFrom = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (fireFromDist - textureHeight);
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), fireFrom, Projectile.velocity * 2f, ModContent.ProjectileType<PBowArrow>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, (int)Projectile.ai[1]);
                }
                counter = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (!Projectile.hide)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(player.gravDir * Projectile.rotation - MathHelper.ToRadians(90)));
            }
            Projectile.hide = false;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.Center = center;
            Projectile.rotation += 1.57f;
            return false;
        }
    }
    public class PBowArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(1);
            Projectile.aiStyle = 1;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 3000;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            if (runOnce)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), newColor: ColorHelpers.PolarisColor(Main.rand.NextFloat(1)));
                    dust.scale *= 1f;
                    dust.velocity *= 0.75f;
                    dust.velocity += Projectile.velocity * 0.2f;
                    dust.noGravity = true;
                    dust.fadeIn = .2f;
                }
                runOnce = false;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            int arrowType = (int)Projectile.ai[0];
            if(Main.myPlayer == Projectile.owner)
            {
                for(int i = -1; i <= 1; i++)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(i * 6)), arrowType, Projectile.damage, Projectile.knockBack * 0.5f, Main.myPlayer);
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 12;
            height = 12;
            return true;
        }
    }
    public class PBolt : ModProjectile
    {
        bool end = false;
        int bounceCount = -1;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Color color2 = new Color(110, 110, 110, 0);
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
                color2 = Projectile.GetAlpha(color2) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
                for (int j = 0; j < 5; j++)
                {
                    float x = Main.rand.Next(-10, 11) * 0.1f;
                    float y = Main.rand.Next(-10, 11) * 0.1f;
                    if (!Projectile.oldPos[k].Equals(Projectile.position))
                    {
                        Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color2, Projectile.rotation, drawOrigin, (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
                    }
                }
            }
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.height = 10;
            Projectile.width = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 7200;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20 * (1 + Projectile.extraUpdates);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            bounceCount++;
            if (bounceCount > 3)
                UpdateEnd();
            Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
            target.immune[Projectile.owner] = 0;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bounceCount++;
            if (bounceCount > 3)
                UpdateEnd();
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            initialVelo = Projectile.velocity;

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 14; i++)
            {
                int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
                Dust dust = Main.dust[num2];
                Color color2 = new Color(110, 210, 90, 0);
                dust.color = color2;
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= 2f;
                dust.alpha = 255 - (int)(255 * (Projectile.timeLeft / 40f));
                dust.velocity += Projectile.velocity * 0.2f;
            }
        }
        bool runOnce = true;
        Vector2 initialVelo;
        public void UpdateEnd()
        {
            if (bounceCount > 3)
            {
                if (Projectile.timeLeft > 40)
                    Projectile.timeLeft = 40;
                end = true;
                Projectile.velocity *= 0;
                Projectile.friendly = false;
                Projectile.extraUpdates = 1;
                if (Main.myPlayer == Projectile.owner)
                    Projectile.netUpdate = true;
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.tileCollide);
            writer.Write(Projectile.friendly);
            writer.Write(end);
            writer.Write(Projectile.extraUpdates);
            writer.Write(bounceCount);
            base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.tileCollide = reader.ReadBoolean();
            Projectile.friendly = reader.ReadBoolean();
            end = reader.ReadBoolean();
            Projectile.extraUpdates = reader.ReadInt32();
            bounceCount = reader.ReadInt32();
            base.ReceiveExtraAI(reader);
        }
        public override bool PreAI()
        {
            if (runOnce)
            {
                initialVelo = Projectile.velocity;
                runOnce = false;
            }
            if (end == true && Projectile.timeLeft > 40)
                Projectile.timeLeft = 40;
            if ((Main.rand.NextBool(2) && end) || Main.rand.NextBool(22))
            {
                int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(4, 4), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
                Dust dust = Main.dust[num2];
                Color color2 = new Color(110, 210, 90, 0);
                dust.color = color2;
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= 2f;
                int alpha = 255 - (int)(255 * (Projectile.timeLeft / 40f));
                alpha = alpha > 255 ? 255 : alpha;
                alpha = alpha < 0 ? 0 : alpha;
                dust.alpha = alpha;
            }
            if (bounceCount > 3)
            {
                UpdateEnd();
                return false;
            }
            return base.PreAI();
        }
    }
}