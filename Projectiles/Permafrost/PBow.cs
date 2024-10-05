using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using System;
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
                chargePercent = MathF.Pow(chargePercent, 0.34f);
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
                int i = (int)Projectile.ai[0] - (int)counter + 2;
                player.itemTime = i;
                player.itemAnimation = i;
                Projectile.timeLeft = i;
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
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), fireFrom, Projectile.velocity * 0.45f, ModContent.ProjectileType<PBowArrow>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, (int)Projectile.ai[1]);
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
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.arrow = true;
            Projectile.hide = true;
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
                for (int i = 0; i < 15; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), newColor: ColorHelper.PolarisColor(Main.rand.NextFloat(1)));
                    dust.scale *= 1.3f;
                    dust.velocity *= 0.8f;
                    dust.velocity += Projectile.velocity * 0.2f;
                    dust.noGravity = true;
                    dust.fadeIn = .2f;
                }
                runOnce = false;
            }
            else
            {
                for(float j = 0; j < 1; j += 0.34f)
                {
                    float scaleUp = (Projectile.ai[1] * 0.055f + 1) * 3 * MathF.Sin(MathHelper.ToRadians(Projectile.ai[1] * 3f));
                    for (int i = -1; i <= 1; i += 2)
                    {
                        Vector2 circular = new Vector2(0, scaleUp * i).RotatedBy(Projectile.rotation - MathHelper.PiOver2);
                        Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5) + circular + Projectile.velocity * j, 0, 0, ModContent.DustType<CopyDust4>(), newColor: ColorHelper.PolarisColor(0.5f + i * 0.5f * MathF.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2))));
                        dust.scale = dust.scale * 0.5f + .65f + Projectile.ai[1] / 350f;
                        dust.velocity *= 0.04f;
                        dust.velocity += Projectile.velocity * 0.05f;
                        dust.noGravity = true;
                        dust.fadeIn = .2f;
                        dust.color *= (1 - Projectile.timeLeft / 60f) * 0.7f;
                    }
                    Projectile.ai[1]++;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.hide = false;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 24; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>(), newColor: ColorHelper.PolarisColor(Main.rand.NextFloat(1)));
                dust.scale = dust.scale + 1.25f;
                dust.velocity *= 1.2f;
                dust.velocity += Projectile.velocity * Main.rand.NextFloat() * 0.7f;
                dust.noGravity = true;
                dust.fadeIn = .2f;
            }
            SOTSUtils.PlaySound(SoundID.Item75, Projectile.Center, 1.0f, -0.1f);
            int arrowType = (int)Projectile.ai[0];
            if(Main.myPlayer == Projectile.owner)
            {
                for(int i = -3; i <= 3; i++)
                {
                    int type = arrowType;
                    if (Math.Abs(i) % 2  == 1)
                    {
                        type = ModContent.ProjectileType<PBolt>();
                    }
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.oldVelocity.RotatedBy(MathHelper.ToRadians(i * 5)) * 1.3f, type, Projectile.damage, Projectile.knockBack * 0.5f, Main.myPlayer);
                }
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 previous = Projectile.Center + Projectile.velocity;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero || Projectile.oldPos[k] == Projectile.position)
                    continue;
                float scale = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                Color color2 = new Color(110, 110, 110, 0);
                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2;
                color2 = Projectile.GetAlpha(color2) * scale * 1.5f;
                Vector2 toPrev = previous - drawPos;
                Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color2, toPrev.ToRotation() + MathHelper.PiOver2, drawOrigin, new Vector2(scale, toPrev.Length() / texture.Height * 2f), SpriteEffects.None, 0f);
                previous = drawPos;
            }
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.height = 10;
            Projectile.width = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 900;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10 * (1 + Projectile.extraUpdates);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = Math.Max((int)(Projectile.damage * 0.9f), 1);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Vector2 previous = Projectile.Center + Projectile.velocity;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero)
                    continue;
                float scale = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2;
                Dust dust = Dust.NewDustDirect(drawPos - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>(), newColor: ColorHelper.PolarisColor(scale));
                dust.fadeIn = 0.2f;
                dust.noGravity = true;
                dust.color.A = 0;
                dust.scale = 1.0f + 0.5f * scale;
                dust.color *= scale + 0.25f;
                Vector2 toPrev = previous - drawPos;
                dust.velocity *= 0.15f + 0.1f * scale;
                dust.velocity += toPrev * (0.1f + 0.15f * scale);
                previous = drawPos;
            }
        }
        bool runOnce = true;
        public void UpdateEnd()
        {
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
        public override bool PreAI()
        {
            if (runOnce)
            {
                runOnce = false;
                Projectile.velocity *= 0.3f;
            }
            else
                Projectile.velocity += Projectile.velocity.SNormalize() * 0.005f;
            float dustInterval = SOTS.Config.lowFidelityMode ? 0.5f : 0.34f;
            for(float j = 0; j < 1; j += dustInterval)
            {
                if(Main.rand.NextBool(3))
                {
                    Dust dust = PixelDust.Spawn(Projectile.Center + Projectile.velocity * j, 0, 0, Main.rand.NextVector2Square(-0.1f, 0.1f), ColorHelper.PolarisColor(Main.rand.NextFloat(1)));
                    dust.color.A = 0;
                    dust.scale = 1.0f;
                    dust.color *= 0.5f;
                }
            }
            if(Projectile.timeLeft < 50)
            {
                Projectile.velocity *= 0.955f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            return true;
        }
    }
}