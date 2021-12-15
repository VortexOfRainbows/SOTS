using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Evil
{
    public class DeathSpiralProj : ModProjectile //I must credit pyroknight for creating examplesolareruption. 
    {
        public Vector2 chainHeadPosition;
        public float firingSpeed;
        public float firingAnimation;
        public float firingTime;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death Spiral");
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        bool runOnce = true;
        float lightningCounter = 0;
        public override void AI()
        {
            if (runOnce)
            {
                lightningCounter = Main.rand.Next(360);
            }
            else
                lightningCounter += 7f;
            Player player = Main.player[projectile.owner];
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(-90f);
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = projectile.rotation;
            }
            float direction = (projectile.localAI[0].ToRotationVector2().X >= 0f).ToDirectionInt();

            Vector2 rotation = (direction * (projectile.ai[0] / firingAnimation * MathHelper.ToRadians(360f) + MathHelper.ToRadians(-90f))).ToRotationVector2();
            rotation.Y *= (float)Math.Sin(projectile.ai[1]);

            rotation = rotation.RotatedBy(projectile.localAI[0]);

            projectile.ai[0] += 1f;
            if (projectile.ai[0] < firingTime)
            {
                projectile.velocity += (firingSpeed * rotation).RotatedBy(MathHelper.ToRadians(90f));
            }
            else
            {
                projectile.Kill();
            }
            projectile.Center = player.RotatedRelativePoint(player.MountedCenter) + projectile.velocity.SafeNormalize(Vector2.Zero) * 12;
            chainHeadPosition = projectile.Center + projectile.velocity;
            SetUpTrails();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int cooldown = 3;
            projectile.localNPCImmunity[target.whoAmI] = 6;
            target.immune[projectile.owner] = cooldown;
        }
        public override bool? CanCutTiles()
        {
            return true;
        }
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity, (projectile.width + projectile.height) * 0.5f * projectile.scale, DelegateMethods.CutTiles);
        }
        // Plot a line from the start of the Solar Eruption to the end of it, and check if any hitboxes are intersected by it for the entity collision logic. (Don't change this.)
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Custom collision so all chains across the flail can cause impact.
            float collisionPoint = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity, (projectile.width + projectile.height) * 0.5f * projectile.scale, ref collisionPoint))
            {
                return true;
            }
            return false;
        }
        Vector2[] trailPos = new Vector2[40];
        public void SetUpTrails()
        {
            Vector2 from = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * 6;
            Vector2 End = projectile.Center + projectile.velocity;
            runOnce = false;
            for (int i = 0; i < trailPos.Length; i++)
            {
                float sin = (float)Math.Sin(MathHelper.ToRadians(i * (180f / trailPos.Length)));
                float radius = sin * 3f;
                Vector2 pos = Vector2.Lerp(from, End, i / 40f);
                Vector2 sinusoid = new Vector2(0, (float)Math.Cos(MathHelper.ToRadians(i * 8 + lightningCounter)) * 12 * sin).RotatedBy(projectile.velocity.ToRotation());
                trailPos[i] = pos + Main.rand.NextVector2CircularEdge(radius, radius) + sinusoid;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            Texture2D texture = Main.projectileTexture[projectile.type];
            Color color = lightColor;

            // Some rectangle presets for different parts of the chain.
            Rectangle chainHandle = new Rectangle(0, 2, texture.Width, 40);
            Rectangle chainLinkEnd = new Rectangle(0, 68, texture.Width, 18);
            Rectangle chainLink = new Rectangle(0, 46, texture.Width, 18);
            Rectangle chainHead = new Rectangle(0, 90, texture.Width, texture.Height - 90);

            // If the chain isn't moving, stop drawing all of its components.
            if (projectile.velocity == Vector2.Zero)
            {
                return false;
            }

            // These fields / pre-draw logic have been taken from the vanilla source code for the Solar Eruption.
            // They setup distances, directions, offsets, and rotations all so the chain faces correctly.
            DrawLightning();
            Vector2 startPosition = projectile.Center;
            Vector2 yOffset = new Vector2(0, player.gfxOffY);
            spriteBatch.Draw(texture, startPosition - Main.screenPosition + yOffset, chainHandle, color, projectile.rotation, chainHandle.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
            Vector2 chainEnd = projectile.Center + projectile.velocity;
            spriteBatch.Draw(texture, chainEnd - Main.screenPosition + yOffset, chainHead, Lighting.GetColor((int)chainEnd.X / 16, (int)chainEnd.Y / 16), projectile.rotation, chainHead.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public void DrawLightning()
        {
            if (runOnce)
                return;
            Texture2D texture = mod.GetTexture("Projectiles/Evil/DeathSpiralTrail");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 previousPosition = trailPos[0];
            if (previousPosition == Vector2.Zero)
            {
                return;
            }
            for (int k = 1; k < trailPos.Length; k++)
            {
                float scale = 0.5f + 0.6f * (0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(k * (180f / trailPos.Length))));
                scale *= 0.9f;
                if (trailPos[k] == Vector2.Zero)
                {
                    return;
                }
                Vector2 drawPos = trailPos[k] - Main.screenPosition;
                Vector2 currentPos = trailPos[k];
                Vector2 betweenPositions = previousPosition - currentPos;
                Color color = new Color(130, 140, 100, 0) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
                float max = betweenPositions.Length() / (4 * scale);
                for (int i = 0; i < max; i++)
                {
                    drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
                    for (int j = 0; j < 4; j++)
                    {
                        float x = Main.rand.NextFloat(-4, 4f) * scale;
                        float y = Main.rand.NextFloat(-4, 4f) * scale;
                        if (j <= 1)
                        {
                            x = 0;
                            y = 0;
                        }
                        if (trailPos[k] != projectile.Center)
                            Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
                    }
                }
                previousPosition = currentPos;
            }
        }
    }
}