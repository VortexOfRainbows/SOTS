using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{    
    public class EarthshakerPickaxe : ModProjectile 
    {	
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50; 
            projectile.timeLeft = 110;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.melee = true; 
			projectile.alpha = 0;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/EarthshakerPickaxeGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            for(int i = 0; i < 4; i++)
            {
                Vector2 addition = Main.rand.NextVector2Circular(1, 1);
                spriteBatch.Draw(texture, drawPos + addition, null, new Color(110, 105, 100, 0), projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
        }
        public const float maxRange = 160f;
        bool hasStopped = false;
        int counter = 0;
        public void HitTiles()
        {
            Player player = Main.player[projectile.owner];
            int i = (int)projectile.Center.X / 16;
            int j = (int)projectile.Center.Y / 16;
            for(int k = i - 1; k <= i + 1; k++)
            {
                for (int l = j - 1; l <= j + 1; l++)
                {
                    player.PickTile(k, l, 18);
                }
            }
        }
        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            projectile.rotation += player.direction * 0.36f;
            Vector2 toPlayer = player.Center - projectile.Center;
            float distance = toPlayer.Length();
            player.itemTime = 2;
            player.itemAnimation = 2;
            if (projectile.timeLeft <= 90 && (!player.channel || hasStopped))
            {
                projectile.velocity *= 0.5f;
                hasStopped = true;
                projectile.velocity += new Vector2(-10, 0).RotatedBy(Math.Atan2(projectile.Center.Y - player.Center.Y, projectile.Center.X - player.Center.X));
                projectile.tileCollide = false;
                if (distance < 32)
                {
                    projectile.Kill();
                }
                return false;
            }
            else
            {
                if(projectile.timeLeft < 90)
                    projectile.timeLeft = 90;
            }

            Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.6f / 255f, (255 - projectile.alpha) * 0.45f / 255f, 0);
            if (Main.myPlayer == projectile.owner && counter % 3 == 0)
            {
                projectile.ai[0] = Main.MouseWorld.X;
                projectile.ai[1] = Main.MouseWorld.Y;
                projectile.netUpdate = true;
            }
            if(projectile.ai[0] != 0 && projectile.ai[1] != 0)
            {
                Vector2 add = new Vector2(projectile.ai[0], projectile.ai[1]);
                if(Vector2.Distance(add, player.Center) > maxRange)
                {
                    add = player.Center + (add - player.Center).SafeNormalize(Vector2.Zero) * maxRange;
                }
                add -= projectile.Center;
                float toPlayerL = (add - player.Center).Length() + 16;
                if (toPlayerL > 0)
                {
                    float dist = 2400f / (float)Math.Pow(toPlayerL, 0.5);
                    if(dist > add.Length())
                    {
                        dist = add.Length();
                    }
                    add = add.SafeNormalize(Vector2.Zero) * dist;
                    projectile.velocity = add;
                }
            };
            counter++;
            if(counter % 6 == 0)
            {
                if(counter > 24 && Main.myPlayer == projectile.owner)
                    HitTiles();
            }
            return false;
        }
        bool runOnce = true;
        Vector2[] trailPos = new Vector2[20];
        public void SetUpTrails()
        {
            Player player = Main.player[projectile.owner];
            Vector2 fromPlayer = new Vector2(projectile.ai[0], projectile.ai[1]) - player.Center;
            Vector2 center = player.Center + fromPlayer.SafeNormalize(Vector2.Zero) * 20;
            runOnce = false;
            for(int i = 0; i < 20; i++)
            {
                float radius = (float)Math.Sin(MathHelper.ToRadians(i * 9)) * 3f;
                Vector2 pos = Vector2.Lerp(projectile.Center, center, i / 20f);
                trailPos[i] = pos + Main.rand.NextVector2CircularEdge(radius, radius);
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void PostAI()
        {
            projectile.Center += projectile.velocity;
            SetUpTrails();
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            hitDirection = player.direction;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (runOnce)
                return true;
            Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Earth/EarthenRing").Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 previousPosition = trailPos[0];
            if (previousPosition == Vector2.Zero)
            {
                return true;
            }
            for (int k = 1; k < trailPos.Length; k++)
            {
                float scale = 0.4f + 0.6f * projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
                scale *= 0.9f;
                if (trailPos[k] == Vector2.Zero)
                {
                    return true;
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
            return true;
        }
    }
}
	
