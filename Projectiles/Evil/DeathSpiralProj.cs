using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
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
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
            projectile.ownerHitCheck = true;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        bool runOnce = true;
        float lightningCounter = 0;
        float normalAICounter = 0;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                Item item = player.HeldItem;
                firingSpeed = projectile.velocity.Length();
                firingAnimation = Item.useAnimation;
                firingTime = Item.useTime;
                lightningCounter = Main.rand.Next(360);
            }
            else
                lightningCounter += 7f;
            if(projectile.ai[0] != -1)
            {
                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                player.itemTime = 2;
                player.itemAnimation = 2;
                player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(-90f);
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = projectile.rotation;
            }
            float direction = 1f;
            Vector2 rotation = (direction * (normalAICounter / firingAnimation * MathHelper.ToRadians(360f) + MathHelper.ToRadians(-90f))).ToRotationVector2();
            rotation.Y *= (float)Math.Sin(projectile.ai[1]);

            rotation = rotation.RotatedBy(projectile.localAI[0]);

            normalAICounter += 1f;
            if (normalAICounter < firingTime)
            {
                if(Main.rand.NextBool(4))
                {
                    Dust dust = Dust.NewDustDirect(projectile.Center + projectile.velocity * Main.rand.NextFloat(1) - new Vector2(5) + Main.rand.NextVector2Circular(12, 12), 0, 0, ModContent.DustType<CopyDust4>());
                    dust.velocity *= 0.2f;
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.color = Color.Lerp(new Color(110, 15, 0, 0), new Color(200, 32, 0, 0), Main.rand.NextFloat(1));
                    dust.scale *= 1.6f;
                    dust.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * 1.1f;
                }
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
                float radius = sin * 6f;
                Vector2 pos = Vector2.Lerp(from, End, i / 40f);
                Vector2 sinusoid = new Vector2(0, (float)Math.Cos(MathHelper.ToRadians(i * 8 + lightningCounter)) * 12 * sin).RotatedBy(projectile.velocity.ToRotation());
                trailPos[i] = pos + Main.rand.NextVector2CircularEdge(radius, radius) + sinusoid;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            Texture2D texture = Main.projectileTexture[projectile.type];
            Texture2D texture1 = Mod.Assets.Request<Texture2D>("Projectiles/Evil/DeathSpiralBlade").Value;
            Color color = lightColor;
            // Some rectangle presets for different parts of the chain.

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
            float rotation = MathHelper.ToRadians(135) + projectile.rotation;
            if (projectile.ai[0] != -1)
                spriteBatch.Draw(texture, startPosition - Main.screenPosition + yOffset, null, color, rotation, texture.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
            Vector2 chainEnd = projectile.Center + projectile.velocity;
            spriteBatch.Draw(texture1, chainEnd - Main.screenPosition + yOffset, null, Lighting.GetColor((int)chainEnd.X / 16, (int)chainEnd.Y / 16), rotation, texture1.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public void DrawLightning()
        {
            if (runOnce)
                return;
            Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Evil/DeathSpiralTrail").Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 previousPosition = trailPos[0];
            if (previousPosition == Vector2.Zero)
            {
                return;
            }
            float lifetimeMult = 0.1f + (float)Math.Sin(normalAICounter / firingAnimation * Math.PI);
            for (int k = 1; k < trailPos.Length; k++)
            {
                float multiplier = (float)Math.Sin(MathHelper.ToRadians(k * (180f / trailPos.Length)));
                float scale = 0.5f + 0.6f * (0.5f + 0.5f * multiplier);
                scale *= 0.9f * lifetimeMult;
                if (trailPos[k] == Vector2.Zero)
                {
                    return;
                }
                Vector2 drawPos = trailPos[k] - Main.screenPosition;
                Vector2 currentPos = trailPos[k];
                Vector2 betweenPositions = previousPosition - currentPos;
                Color color = new Color(160, 100, 100, 0) * multiplier * 0.5f;
                float amountMult = 2;
                if (SOTS.Config.lowFidelityMode)
                    amountMult = 4;
                float max = betweenPositions.Length() / (amountMult * scale);
                for (int i = 0; i < max; i++)
                {
                    drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
                    for (int j = 0; j < 2; j++)
                    {
                        float x = Main.rand.NextFloat(-4, 4f) * scale * j;
                        float y = Main.rand.NextFloat(-4, 4f) * scale * j;
                        if (trailPos[k] != projectile.Center)
                            Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
                    }
                }
                previousPosition = currentPos;
            }
        }
    }
}