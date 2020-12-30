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
using Terraria.World.Generation;
using Terraria.Enums;
using System.Diagnostics.Contracts;

namespace SOTS.Projectiles.Otherworld
{
	public class SupernovaHammer : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova Hammer");
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Otherworld/SupernovaHammerGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            Color color = Color.White;
            spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            //projectile.aiStyle = 140;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.alpha = (int)byte.MaxValue;
            projectile.hide = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 12;
            projectile.ownerHitCheck = true;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if ((double)projectile.ai[0] >= 36f) projectile.localAI[1] = 1.0f;
            if (crit)
            {
                Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 14, 0.6f);
                if (projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                        Projectile.NewProjectile(target.Center.X, target.Center.Y, circular.X, circular.Y, mod.ProjectileType("Seeker"), projectile.damage, projectile.knockBack, Main.myPlayer, Main.rand.Next(360), target.whoAmI);
                    }
                }
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
            {
                Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 14, 0.6f);
                if (projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                        Projectile.NewProjectile(target.Center.X, target.Center.Y, circular.X, circular.Y, mod.ProjectileType("Seeker"), (int)(0.7f * projectile.damage) + 1, projectile.knockBack, Main.myPlayer, Main.rand.Next(360), target.whoAmI);
                    }
                }
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override bool? CanCutTiles()
        {
            var num5 = 30f;
            var f = projectile.rotation - 0.7853982f * (float)Math.Sign(projectile.velocity.X);
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center + f.ToRotationVector2() * -num5, projectile.Center + f.ToRotationVector2() * num5, (float)projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CutTiles));
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            var f = projectile.rotation - 0.7853982f * (float)Math.Sign(projectile.velocity.X);
            var collisionPoint = 0.0f;
            var num = 50f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + f.ToRotationVector2() * -num, projectile.Center + f.ToRotationVector2() * num, 23f * projectile.scale, ref collisionPoint))
                return true;
            return false;
        }
        public override void AI()
        {
            var num1 = 50f;
            var num2 = 2f;
            var num3 = 20f;
            var player = Main.player[projectile.owner];
            var num4 = -0.7853982f;
            var vector2_1 = player.RotatedRelativePoint(player.MountedCenter, true);
            var vector2_2 = Vector2.Zero;
            if (player.dead)
            {
                projectile.Kill();
            }
            else
            {
                var Damage = projectile.damage * 1.5f;
                var num5 = Math.Sign(projectile.velocity.X);
                projectile.velocity = new Vector2((float)num5, 0.0f);
                if ((double)projectile.ai[0] == 0.0)
                {
                    projectile.rotation = (float)((double)new Vector2((float)num5, -player.gravDir).ToRotation() +
                                             (double)num4 + 3.14159274101257);
                    if ((double)projectile.velocity.X < 0.0)
                        projectile.rotation -= 1.570796f;
                }

                projectile.alpha -= 128;
                if (projectile.alpha < 0)
                    projectile.alpha = 0;
                var num6 = projectile.ai[0] / num1;
                ++projectile.ai[0];
                projectile.rotation += 6.283185f * num2 / num1 * (float)num5;
                var flag = (double)projectile.ai[0] == (double)(int)((double)num1 / 2.0);
                if ((double)projectile.ai[0] >= (double)num1 || flag && !player.controlUseItem)
                {
                    projectile.Kill();
                    player.reuseDelay = 10;
                }
                else if (flag)
                {
                    var mouseWorld = Main.MouseWorld;
                    var dir = (double)player.DirectionTo(mouseWorld).X > 0.0 ? 1 : -1;
                    if ((double)dir != (double)projectile.velocity.X)
                    {
                        player.ChangeDir(dir);
                        projectile.velocity = new Vector2((float)dir, 0.0f);
                        projectile.netUpdate = true;
                        projectile.rotation -= 3.141593f;
                    }
                }

                var num7 = projectile.rotation - 0.7853982f * (float)num5;
                vector2_2 = (num7 + (num5 == -1 ? 3.141593f : 0.0f)).ToRotationVector2() * (projectile.ai[0] / num1) *
                            num3;
                var vec = projectile.Center + (num7 + (num5 == -1 ? 3.141593f : 0.0f)).ToRotationVector2() * 30f;

                var dust = Dust.NewDustDirect(vec - new Vector2(5f), 10, 10, mod.DustType("CopyDust4"), player.velocity.X, player.velocity.Y, 200, new Color(), 1f);
                dust.velocity = projectile.DirectionTo(dust.position) * 0.1f + dust.velocity * 0.1f;
                dust.color = new Color(250, 250, 250, 120);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= 1.4f;

                if (num6 >= 0.75)
                {
                    dust = Dust.NewDustDirect(vec - new Vector2(5f), 10, 10, mod.DustType("CopyDust4"), player.velocity.X, player.velocity.Y, 50, new Color(), 1f);
                    dust.velocity = projectile.DirectionTo(dust.position) * 0.1f + dust.velocity * 0.1f;
                    dust.noGravity = true;
                    dust.color = new Color(0, 200, 220, 100);
                    dust.fadeIn = 0.1f;
                    dust.scale *= 1.6f;
                }

                if (projectile.ai[0] >= num1 - 8.0 && projectile.ai[0] < num1 - 2.0)
                {
                    for (var index = 0; index < 5; ++index)
                    {
                        dust = Dust.NewDustDirect(vec - new Vector2(5f), 10, 10, mod.DustType("CopyDust4"), player.velocity.X, player.velocity.Y, 50, new Color(), 1f);
                        dust.velocity *= 1f;
                        dust.noGravity = true;
                        dust.scale += 0.1f;
                        dust.color = new Color(0, 200, 220, 100);
                        dust.fadeIn = 0.1f;
                        dust.scale *= 1.6f;
                    }
                }

                if (projectile.ai[0] == num1 - 3.0 && projectile.owner == Main.myPlayer)
                {
                    if (projectile.localAI[1] != 1.0)
                    {
                        Point result;
                        if (!WorldUtils.Find(vec.ToTileCoordinates(), Searches.Chain((GenSearch)new Searches.Down(4), (GenCondition)new Conditions.IsSolid()), out result))
                        {
                            Main.PlayTrackedSound((SoundStyle)SoundID.DD2_MonkStaffGroundMiss, projectile.Center);
                            return;
                        }
                    }

                    Projectile.NewProjectile(vec + new Vector2((float)(num5 * 20), -60f), Vector2.Zero, mod.ProjectileType("SupernovaSmash"), (int)Damage, 0.0f, projectile.owner, 0.0f, 0.0f);
                    Main.PlayTrackedSound((SoundStyle)SoundID.DD2_MonkStaffGroundImpact, projectile.Center);
                }
                projectile.position = vector2_1 - projectile.Size / 2f;
                projectile.position = projectile.position + vector2_2;
                projectile.spriteDirection = projectile.direction;
                projectile.timeLeft = 2;
                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                player.itemTime = 2;
                player.itemAnimation = 2;
                player.itemRotation = MathHelper.WrapAngle(projectile.rotation);
            }
        }
    }
}

