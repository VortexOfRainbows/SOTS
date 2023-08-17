using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.WorldBuilding;
using Terraria.Enums;
using System.Diagnostics.Contracts;

namespace SOTS.Projectiles.Otherworld
{
	public class SupernovaHammer : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Supernova Hammer");
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/SupernovaHammerGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = Color.White;
            Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
        {
            Projectile.width = 78;
            Projectile.height = 78;
            //Projectile.aiStyle = 140;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.alpha = (int)byte.MaxValue;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.ownerHitCheck = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if ((double)Projectile.ai[0] >= 36f) Projectile.localAI[1] = 1.0f;
            if (hit.Crit)
            {
                SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f);
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X, target.Center.Y, circular.X, circular.Y, ModContent.ProjectileType<Seeker>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.Next(360), target.whoAmI);
                    }
                }
            }
            if (target.life <= 0)
            {
                SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f);
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X, target.Center.Y, circular.X, circular.Y, ModContent.ProjectileType<Seeker>(), (int)(0.7f * Projectile.damage) + 1, Projectile.knockBack, Main.myPlayer, Main.rand.Next(360), target.whoAmI);
                    }
                }
            }
        }
        public override bool? CanCutTiles()
        {
            var num5 = 32f;
            var f = Projectile.rotation - 0.7853982f * (float)Math.Sign(Projectile.velocity.X);
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(Projectile.Center + f.ToRotationVector2() * -num5, Projectile.Center + f.ToRotationVector2() * num5, (float)Projectile.width * Projectile.scale, new Utils.TileActionAttempt(DelegateMethods.CutTiles));
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            var f = Projectile.rotation - 0.7853982f * (float)Math.Sign(Projectile.velocity.X);
            var collisionPoint = 0.0f;
            var num = 64f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + f.ToRotationVector2() * -num, Projectile.Center + f.ToRotationVector2() * num, 23f * Projectile.scale, ref collisionPoint))
                return true;
            return false;
        }
        public override void AI()
        {
            var num1 = 50f;
            var num2 = 2f;
            var num3 = 20f;
            var player = Main.player[Projectile.owner];
            var num4 = -0.7853982f;
            var vector2_1 = player.RotatedRelativePoint(player.MountedCenter, true);
            var vector2_2 = Vector2.Zero;
            if (player.dead)
            {
                Projectile.Kill();
            }
            else
            {
                var Damage = Projectile.damage * 1.5f;
                var num5 = Math.Sign(Projectile.velocity.X);
                Projectile.velocity = new Vector2((float)num5, 0.0f);
                if ((double)Projectile.ai[0] == 0.0)
                {
                    Projectile.rotation = (float)((double)new Vector2((float)num5, -player.gravDir).ToRotation() +
                                             (double)num4 + 3.14159274101257);
                    if ((double)Projectile.velocity.X < 0.0)
                        Projectile.rotation -= 1.570796f;
                }

                Projectile.alpha -= 128;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
                var num6 = Projectile.ai[0] / num1;
                ++Projectile.ai[0];
                Projectile.rotation += 6.283185f * num2 / num1 * (float)num5;
                var flag = (double)Projectile.ai[0] == (double)(int)((double)num1 / 2.0);
                if ((double)Projectile.ai[0] >= (double)num1 || flag && !player.controlUseItem)
                {
                    Projectile.Kill();
                    player.reuseDelay = 10;
                }
                else if (flag)
                {
                    var mouseWorld = Main.MouseWorld;
                    var dir = (double)player.DirectionTo(mouseWorld).X > 0.0 ? 1 : -1;
                    if ((double)dir != (double)Projectile.velocity.X)
                    {
                        player.ChangeDir(dir);
                        Projectile.velocity = new Vector2((float)dir, 0.0f);
                        Projectile.netUpdate = true;
                        Projectile.rotation -= 3.141593f;
                    }
                }

                var num7 = Projectile.rotation - 0.7853982f * (float)num5;
                vector2_2 = (num7 + (num5 == -1 ? 3.141593f : 0.0f)).ToRotationVector2() * (Projectile.ai[0] / num1) *
                            num3;
                var vec = Projectile.Center + (num7 + (num5 == -1 ? 3.141593f : 0.0f)).ToRotationVector2() * 42f;

                var dust = Dust.NewDustDirect(vec - new Vector2(5f), 10, 10, ModContent.DustType<Dusts.CopyDust4>(), player.velocity.X, player.velocity.Y, 200, new Color(), 1f);
                dust.velocity = Projectile.DirectionTo(dust.position) * 0.1f + dust.velocity * 0.1f;
                dust.color = new Color(250, 250, 250, 120);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= 1.4f;

                if (num6 >= 0.75)
                {
                    dust = Dust.NewDustDirect(vec - new Vector2(5f), 10, 10, ModContent.DustType<Dusts.CopyDust4>(), player.velocity.X, player.velocity.Y, 50, new Color(), 1f);
                    dust.velocity = Projectile.DirectionTo(dust.position) * 0.1f + dust.velocity * 0.1f;
                    dust.noGravity = true;
                    dust.color = new Color(0, 200, 220, 100);
                    dust.fadeIn = 0.1f;
                    dust.scale *= 1.6f;
                }

                if (Projectile.ai[0] >= num1 - 8.0 && Projectile.ai[0] < num1 - 2.0)
                {
                    for (var index = 0; index < 5; ++index)
                    {
                        dust = Dust.NewDustDirect(vec - new Vector2(5f), 10, 10, ModContent.DustType<Dusts.CopyDust4>(), player.velocity.X, player.velocity.Y, 50, new Color(), 1f);
                        dust.velocity *= 1f;
                        dust.noGravity = true;
                        dust.scale += 0.1f;
                        dust.color = new Color(0, 200, 220, 100);
                        dust.fadeIn = 0.1f;
                        dust.scale *= 1.6f;
                    }
                }

                if (Projectile.ai[0] == num1 - 3.0 && Projectile.owner == Main.myPlayer)
                {
                    if (Projectile.localAI[1] != 1.0)
                    {
                        Point result;
                        if (!WorldUtils.Find(vec.ToTileCoordinates(), Searches.Chain((GenSearch)new Searches.Down(4), (GenCondition)new Conditions.IsSolid()), out result))
                        {
                            SOTSUtils.PlaySound(SoundID.DD2_MonkStaffGroundMiss, Projectile.Center);
                            return;
                        }
                    }
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), vec + new Vector2((float)(num5 * 20), -60f), Vector2.Zero, ModContent.ProjectileType<SupernovaSmash>(), (int)Damage, 0.0f, Projectile.owner, 0.0f, 0.0f);
                    SOTSUtils.PlaySound(SoundID.DD2_MonkStaffGroundImpact, Projectile.Center);
                }
                Projectile.position = vector2_1 - Projectile.Size / 2f;
                Projectile.position = Projectile.position + vector2_2;
                Projectile.spriteDirection = Projectile.direction;
                Projectile.timeLeft = 2;
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemTime = 2;
                player.itemAnimation = 2;
                player.itemRotation = MathHelper.WrapAngle(Projectile.rotation);
            }
        }
    }
}

