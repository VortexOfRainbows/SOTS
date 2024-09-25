using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System;
using System.Collections.Generic;
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
            Projectile.DamageType = ModContent.GetInstance<VoidMagic>();
            Projectile.timeLeft = 30;
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
            for (int i = 0; i < 6; i++)
            {
                Vector2 c = new Vector2(1.0f, 0).RotatedBy(MathHelper.TwoPi / 6f * i + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
                Main.spriteBatch.Draw(textureGlow, drawPos + c, null, color * 0.7f, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction * Projectile.spriteDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
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
                Projectile.timeLeft = 32;
            }
            Vector2 center = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float offsetDist = 5 * Projectile.scale;
                Vector2 toMouse = Main.MouseWorld - center;
                toMouse = toMouse.SafeNormalize(Vector2.Zero) * offsetDist;
                if ((double)toMouse.X != Projectile.velocity.X || (double)toMouse.Y != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                if(counter == 0)
                    Projectile.velocity = toMouse;
                else
                {
                    Projectile.velocity = new Vector2(offsetDist, 0).RotatedBy(SOTSUtils.AngularLerp(Projectile.velocity.ToRotation(), toMouse.ToRotation(), 0.08f));
                }
            }
            Vector2 offset = new Vector2(0, -4 * player.gravDir * Projectile.direction).RotatedBy(Projectile.rotation);
            offset.X *= 0.5f;

            if (counter == (int)(Projectile.ai[0] / 3) || counter == (int)(2 * Projectile.ai[0] / 3))
            {
                if (counter == (int)(2 * Projectile.ai[0] / 3))
                    SOTSUtils.PlaySound(SoundID.Item15, Projectile.Center, 1.25f, 0.3f);
                else
                    SOTSUtils.PlaySound(SoundID.Item15, Projectile.Center, 1.2f, 0.15f);
                Color color = Color.Lerp(ColorHelpers.Inferno1, ColorHelpers.EarthColor, 0.3f);
                color.A = 0;
                float r = Projectile.velocity.ToRotation();
                for (int i = 0; i < 20; i++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(i / 10f * MathF.PI);
                    circular.X *= 0.6f;
                    circular = circular.RotatedBy(r);
                    Dust dust2 = Dust.NewDustDirect(Barrel - new Vector2(5, 5) + circular * 32, 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color);
                    dust2.scale = Main.rand.NextFloat(1f, 2f);
                    dust2.noGravity = true;
                    dust2.velocity *= 0.1f;
                    dust2.velocity += -0.5f * Projectile.velocity.SNormalize() - circular * 5;
                    dust2.fadeIn = 15;
                }
            }
            counter++;
            if (counter == (int)Projectile.ai[0])
            {
                if (Main.myPlayer == Projectile.owner)
                    for(int i = -1; i <= 1; i++)
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.SNormalize() * 32, Projectile.velocity.RotatedBy(i * MathHelper.PiOver4 / 2f), ModContent.ProjectileType<MagmaLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Projectile.identity, i);
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
        public Vector2 Barrel => Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(Projectile.direction * Projectile.spriteDirection * -1)) * 44;
    }
    public class MagmaLaser : ModProjectile
    {
        public float TimeLeftPercent => Projectile.timeLeft / 30f;
        private int Counter;
        private int Counter2;
        private int Counter3;
        private int Unique;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Counter);
            writer.Write(Unique);
            writer.Write(Ended);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Counter = reader.ReadInt32();
            Unique = reader.ReadInt32();
            Ended = reader.ReadBoolean();
        }
        public override void SetDefaults()
        {
            Projectile.height = 40;
            Projectile.width = 40;
            Projectile.DamageType = ModContent.GetInstance<VoidMagic>();
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.localNPCHitCooldown = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (RunOnce || points == null || points.Count <= 0)
                return false;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 pos = Projectile.Center - Projectile.velocity;
            Color c = Color.Lerp(ColorHelpers.Inferno1, ColorHelpers.EarthColor, 0.3f);
            c.A = 0;
            for (int i = 0; i < points.Count; i++)
            {
                float percent = MathF.Sqrt(MathF.Abs(MathF.Sin(MathF.PI * i / points.Count)));
                Vector2 toNextPoint = points[i] - pos;
                float scaleMod = 0.75f + 0.25f * MathF.Sin(MathHelper.ToRadians(i * 8 + SOTSWorld.GlobalCounter * -2f));
                float dist = toNextPoint.Length();
                Main.spriteBatch.Draw(texture, points[i] - Main.screenPosition, null, c * (0.5f + 0.5f * percent) * 0.6f * TimeLeftPercent, toNextPoint.ToRotation(), drawOrigin, new Vector2(MathF.Min(1f, dist), scaleMod) * (0.5f + 0.5f * percent) * (0.5f + Counter2 / 40f), SpriteEffects.None, 0f);
                pos = points[i];
            }
            DrawPlasma(finalPosition);
            return false;
        }
        public void DrawPlasma(Vector2 pos)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            float sin = 1.4f * MathF.Max(Counter2 / 20f, 0.25f) * TimeLeftPercent;
            float scale = Projectile.scale * sin;
            Color color = Color.Lerp(ColorHelpers.Inferno1, ColorHelpers.EarthColor, 0.25f) * 0.3f * TimeLeftPercent;
            color.A = 0;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            SOTS.GodrayShader.Parameters["distance"].SetValue(6 * sin);
            SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
            SOTS.GodrayShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/noise").Value);
            SOTS.GodrayShader.Parameters["rotation"].SetValue(Counter * MathF.PI / 180f);
            SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f);
            SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(texture, pos - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(texture, pos - Main.screenPosition, null, color * 6, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale * 0.5f, SpriteEffects.None, 0f);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            int size = (int)(MathF.Max(Counter2 / 20f, 0.25f) * 72);
            Rectangle projHit = new Rectangle((int)finalPosition.X - size / 2, (int)finalPosition.Y - size / 2, size, size);
            if (targetHitbox.Intersects(projHit))
            {
                return true;
            }
            return false;
        }
        private bool RunOnce = true;
        private bool Ended = false;
        public Vector2 finalPosition;
        public List<Vector2> points;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (RunOnce)
            {
                SOTSUtils.PlaySound(SoundID.Item94, Projectile.Center, 1.3f, -0.55f);
                Unique = (int)Projectile.ai[1];
                Counter = Unique * 120;
                InitializeLaser();
                if (player.HeldItem.ModItem is VoidItem vItem && Unique == 0)
                    vItem.DrainMana(player);
            }
            if (Counter2 < 20)
                Counter2++;
            if(Unique == 0 && !Ended)
            {
                if (Counter3 < 45)
                {
                    Counter3++;
                }
                else
                {
                    SOTSUtils.PlaySound(SoundID.Item15, Projectile.Center, 1.2f, -0.2f);
                    Counter3 = 0;
                    if (player.HeldItem.ModItem is VoidItem vItem)
                        vItem.DrainMana(player);
                }
            }
            if ((player.channel || Main.myPlayer != Projectile.owner) && !Ended)
                Projectile.timeLeft = 30;
            else
                Ended = true;
            int GunID = (int)Projectile.ai[0];
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.identity == GunID && proj.active && proj.type == ModContent.ProjectileType<MagmaBeam>())
                {
                    Projectile.velocity = proj.velocity.RotatedBy(Unique * MathHelper.PiOver4 / 2f);
                    Projectile.Center = proj.Center + new Vector2(32, 0).RotatedBy(proj.velocity.ToRotation());
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
        private Vector2 destination;
        private Vector2 newDestination;
        private void SetMousePos()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                Vector2 mouseWorld = Main.MouseWorld;
                Vector2 toMouseWorld = mouseWorld - player.Center;
                if (toMouseWorld.Length() > 600 * (0.9f * TimeLeftPercent + 0.1f))
                    mouseWorld = player.Center + toMouseWorld.SNormalize() * 600 * (0.9f * TimeLeftPercent + 0.1f);
                if (toMouseWorld.Length() < 160)
                    mouseWorld = player.Center + toMouseWorld.SNormalize() * 160;
                if (RunOnce)
                    mouseWorld = player.Center + toMouseWorld.SNormalize() * 60;
                Projectile.ai[1] = mouseWorld.X;
                Projectile.ai[2] = mouseWorld.Y;
                Projectile.netUpdate = true;
            }
        }
        public void InitializeLaser()
        {
            Color color = Color.Lerp(ColorHelpers.Inferno1, ColorHelpers.EarthColor, 0.3f);
            color.A = 0;
            Player player = Main.player[Projectile.owner];
            if (RunOnce)
            {
                SetMousePos();
                newDestination = new Vector2(Projectile.ai[1], Projectile.ai[2]);
                destination = newDestination;
                for(int i = 0; i < 50; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.Center - new Vector2(11, 11), 12, 12, ModContent.DustType<PixelDust>(), 0, 0, 0, color);
                    dust2.scale = Main.rand.NextFloat(1f, 2f);
                    dust2.noGravity = true;
                    dust2.velocity += Projectile.velocity.SNormalize() * Main.rand.NextFloat(1, 20);
                    dust2.fadeIn = 7;
                }
            }
            else
            {
                float lerpAmount = 0.085f;
                if(newDestination != new Vector2(Projectile.ai[1], Projectile.ai[2]))
                {
                    lerpAmount = 0.15f;
                }
                lerpAmount += (1 - TimeLeftPercent) * 0.2f;
                Vector2 toOld = destination - player.Center;
                Vector2 toNew = newDestination - player.Center;
                float newDist = float.Lerp(toOld.Length(), toNew.Length(), lerpAmount);
                float newAngle = SOTSUtils.AngularLerp(toOld.ToRotation(), toNew.ToRotation(), lerpAmount);
                destination = player.Center + new Vector2(newDist, 0).RotatedBy(newAngle);
            }
            SetMousePos();
            if (Projectile.ai[2] <= 0)
            {
                return;
            }
            newDestination = new Vector2(Projectile.ai[1], Projectile.ai[2]);
            Counter++;
            Vector2 startingPosition = Projectile.Center;
            Vector2 velo = Projectile.velocity.SafeNormalize(Vector2.Zero);
            points = new List<Vector2>();
            int size = 48;
            bool noMoreCollide = false;
            for (int b = 0; b < 240; b++)
            {
                if(b % 10 == 0)
                {
                    Lighting.AddLight(startingPosition, color.ToVector3());
                }
                points.Add(startingPosition);
                Vector2 nextPos = startingPosition + velo * 3.6f;
                int i = (int)nextPos.X / 16;
                int j = (int)nextPos.Y / 16;
                if (!WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(i, j))
                {
                    startingPosition = nextPos;
                }
                finalPosition = startingPosition;
                Rectangle projHit = new Rectangle((int)startingPosition.X - size / 2, (int)startingPosition.Y - size / 2, size, size);
                if(!noMoreCollide && !Ended)
                {
                    bool hasCollided = false;
                    Vector2 npcPos = Vector2.Zero;
                    for (int ID = 0; ID < Main.npc.Length; ID++)
                    {
                        NPC npc = Main.npc[ID];
                        if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                        {
                            if (npc.Hitbox.Intersects(projHit))
                            {
                                npcPos = npc.Center;
                                hasCollided = true;
                                break;
                            }
                        }
                    }
                    if (hasCollided)
                    {
                        newDestination = Vector2.Lerp(finalPosition + velo * 16.0f, npcPos, 0.9f);
                        noMoreCollide = true;
                    }
                }
                if(finalPosition.DistanceSQ(destination) < 256)
                {
                    finalPosition = destination;
                    break;
                }
                else
                {
                    float percent = MathF.Sqrt(MathF.Abs(MathF.Sin(MathF.PI * b / 200f)));
                    float distance = MathF.Max(0, 1 - (destination - finalPosition).Length() / 640f);
                    velo += (destination - finalPosition).SNormalize() * (0.1f + 0.1f * percent + 0.15f * b / 200f + 0.25f * distance);
                    velo += new Vector2(0, MathF.Sin(MathHelper.ToRadians(b * 4 - Counter * 4)) * (0.35f * percent - 0.1f * b / 200f)).RotatedBy(velo.ToRotation());
                    velo = velo.SNormalize();
                }
                int chance = SOTS.Config.lowFidelityMode ? 50 : 33;
                if(Main.rand.NextBool(chance))
                {
                    Dust dust2 = Dust.NewDustDirect(finalPosition - new Vector2(11, 11), 12, 12, ModContent.DustType<PixelDust>(), 0, 0, 0, color * TimeLeftPercent);
                    dust2.scale = Main.rand.NextFloat(0.75f, 1.25f);
                    dust2.noGravity = true;
                    dust2.velocity = (dust2.velocity * 0.55f + velo * 2.75f * Main.rand.NextFloat(1.0f)) * TimeLeftPercent;
                    dust2.fadeIn = 12;
                }
            }
            points.Add(finalPosition);
            Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(37, 37), 64, 64, ModContent.DustType<PixelDust>(), 0, 0, 0, color * TimeLeftPercent, Main.rand.NextFloat(1, 2f));
            dust.noGravity = true;
            dust.velocity = dust.velocity * 0.1f + velo * Main.rand.NextFloat(1.0f) + Main.rand.NextVector2Circular(8, 8) * TimeLeftPercent;
            dust.fadeIn = 7;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.OnFire, 300);
        }
    }
}