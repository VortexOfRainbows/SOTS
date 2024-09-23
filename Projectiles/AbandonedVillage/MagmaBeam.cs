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

namespace SOTS.Projectiles.AbandonedVillage
{
    public class MagmaBeam : ModProjectile
    {
        public override string Texture => "SOTS/Items/AbandonedVillage/MagmaBeam";
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(counter);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            counter = reader.ReadInt32();
        }
        public override void SetDefaults()
        {
            Projectile.width = 44;
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
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.hide)
                return false;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D textureGlow = ModContent.Request<Texture2D>("SOTS/Items/AbandonedVillage/MagmaBeamGlow").Value;
            Vector2 drawOrigin = new Vector2(4, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
           
            Color color = new Color(100, 100, 100, 0);
            float percent = 0.5f;
            percent = Math.Clamp(percent, 0, 1);
            for (int i = 0; i < 6; i++)
            {
                Vector2 c = new Vector2(1.0f, 0).RotatedBy(MathHelper.TwoPi / 6f * i + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
                Main.spriteBatch.Draw(textureGlow, drawPos + c, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            }
            return false;
        }
        private int counter = 0;
        private bool ended = false;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                if (Projectile.ai[0] <= 0)
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
                Projectile.Kill();
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
            if (counter == (int)Projectile.ai[0])
            {
                if(Main.myPlayer == Projectile.owner)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<MagmaLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Projectile.identity);
                Projectile.netUpdate = true;
            }

            Projectile.spriteDirection = (int)player.gravDir;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(Projectile.direction * player.gravDir * -1);
            Projectile.Center = center + offset - Projectile.velocity + Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.direction * player.gravDir * -1));

            if (Projectile.hide == false)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(player.gravDir * Projectile.rotation + MathHelper.ToRadians(-90)));
            }
            Projectile.hide = false;
            return false;
        }
        public Vector2 Barrel => Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(Projectile.direction * Projectile.spriteDirection * -1)) * 32;
    }
    public class MagmaLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.height = 40;
            Projectile.width = 40;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
            Projectile.idStaticNPCHitCooldown = 20;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (RunOnce)
                return false;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = texture.Size() / 2;
            float rot = Projectile.velocity.ToRotation();
            Vector2 toFinal = finalPosition - Projectile.Center;
            Vector2 norm = toFinal.SNormalize();
            float dist = toFinal.Length();
            float separation = 2;
            Vector2 pos = Projectile.Center;
            Color c = Color.Lerp(ColorHelpers.Inferno1, ColorHelpers.EarthColor, 0.3f);
            c.A = 0;
            for (float i = 0; i < dist; i += separation)
            {
                Vector2 position = pos;
                Main.spriteBatch.Draw(texture, position - Main.screenPosition, null, c, rot, drawOrigin, 1f, SpriteEffects.None, 0f);
                pos += norm * separation;
            }
            DrawPlasma(pos);
            return false;
        }
        public void DrawPlasma(Vector2 pos)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            float sin = 1f;
            float scale = Projectile.scale * sin;
            Color color = new Color(157, 93, 213);
            color.A = 0;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            SOTS.GodrayShader.Parameters["distance"].SetValue(6 * sin);
            SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
            SOTS.GodrayShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/noise").Value);
            SOTS.GodrayShader.Parameters["rotation"].SetValue(Projectile.rotation + Projectile.whoAmI * 1.1f + SOTSWorld.GlobalCounter * MathF.PI / 180f);
            SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f * sin);
            SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(texture, pos - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            for (int i = 0; i < 2; i++)
                Main.spriteBatch.Draw(texture, pos - Main.screenPosition, null, i == 1 ? new Color(255, 255, 255, 0) : color * 2, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale * 0.5f, SpriteEffects.None, 0f);
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
        private bool RunOnce = true;
        public Vector2 finalPosition;
        public override bool PreAI()
        {
            if (RunOnce)
                InitializeLaser();
            Player player = Main.player[Projectile.owner];
            if (player.channel || Main.myPlayer != Projectile.owner)
                Projectile.timeLeft = 2;
            else
            {
                Projectile.Kill();
            }
            int GunID = (int)Projectile.ai[0];
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.identity == GunID && proj.active && proj.type == ModContent.ProjectileType<MagmaBeam>())
                {
                    Projectile.velocity = proj.velocity;
                    Projectile.Center = proj.Center + new Vector2(22, 0).RotatedBy(proj.velocity.ToRotation());
                    break;
                }
            }
            if (Projectile.oldVelocity != Projectile.velocity || Projectile.oldPosition != Projectile.position)
                Projectile.netUpdate = true;
            return true;
        }
        public override void AI()
        {
            if (!RunOnce)
                InitializeLaser();
            RunOnce = false;
        }
        public void InitializeLaser()
        {
            Color color = ColorHelpers.EarthColor;
            color.A = 0;
            Vector2 startingPosition = Projectile.Center;
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
            for (int b = 0; b < 320; b++)
            {
                startingPosition += Projectile.velocity * 2.0f;
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
                bool extra = RunOnce;
                int chance = SOTS.Config.lowFidelityMode ? 36 : 12;
                if (Main.rand.NextBool(chance) || extra)
                {
                    Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color, 0.75f);
                    dust.noGravity = true;
                    if (!extra)
                    {
                        dust.velocity = dust.velocity * 0.25f;
                        dust.fadeIn = 17;
                    }
                    else
                    {
                        dust.velocity *= 1.25f;
                        dust.velocity += Projectile.velocity * Main.rand.NextFloat(5f, 8f);
                        dust.fadeIn = 12;
                        dust.scale *= 1.25f;
                    }
                }
            }
            for (int i = 3; i > 0; i--)
            {
                Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color, 1f);
                dust.noGravity = true;
                dust.velocity = dust.velocity * 0.2f + Projectile.velocity * Main.rand.NextFloat(0.1f, 1.0f);
                if (i == 2)
                    dust.velocity += new Vector2(0, -Main.rand.NextFloat(0.1f, 0.3f));
                dust.fadeIn = 7;
            }
        }
        public override void OnKill(int timeLeft)
        {

        }
    }
}