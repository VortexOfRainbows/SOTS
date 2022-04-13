using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;

namespace SOTS.Projectiles.Earth
{    
    public class MinersPickaxe : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miner's Pickaxe");
		}
        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28; 
            projectile.timeLeft = 180;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = true; 
            projectile.melee = true; 
			projectile.alpha = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            DrawOutline(spriteBatch, lightColor);
            spriteBatch.Draw(texture, drawPos + new Vector2(0, player.gfxOffY), null, lightColor, projectile.rotation, drawOrigin, projectile.scale, projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public void DrawOutline(SpriteBatch spriteBatch, Color drawColor)
        {
            Player player = Main.player[projectile.owner];
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Earth/MinersPickaxeOutline");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            float scale = 1 - counter / (float)(timeToLaunch - 8);
            if (scale > 1)
                scale = 1;
            else if (scale < 0)
                scale = 0;
            for (int i = 0; i < 6; i++)
            {
                Vector2 addition = new Vector2(0, 2 + 12 * scale).RotatedBy(MathHelper.ToRadians(i * 60 + 180 * scale * scale));
                spriteBatch.Draw(texture, drawPos + addition + new Vector2(0, player.gfxOffY), null, Color.White * (1 - scale) * (1 - scale), projectile.rotation, drawOrigin, projectile.scale, projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Player player = Main.player[projectile.owner];
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Earth/MinersPickaxeGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            for (int i = 0; i < 4; i++)
            {
                Vector2 addition = Main.rand.NextVector2Circular(1, 1);
                spriteBatch.Draw(texture, drawPos + addition + new Vector2(0, player.gfxOffY), null, new Color(100, 100, 100, 0), projectile.rotation, drawOrigin, projectile.scale, projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return counter >= timeToLaunch;
        }
        Vector2 ogVelo = Vector2.Zero;
        public const int itemUseTime = 16;
        public const float timeToLaunch = 40;
        int counter = 0;
        public override void AI()
		{
            Player player = Main.player[projectile.owner];
            counter++;
            Vector2 overrideDirection = projectile.velocity;
            if(counter < timeToLaunch)
            {
                if(counter == 1)
                    Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 0.8f, -0.2f);
                if (projectile.owner == Main.myPlayer)
                {
                    projectile.ai[0] = Main.MouseWorld.X;
                    projectile.ai[1] = Main.MouseWorld.Y;
                    projectile.netUpdate = true;
                }
                Vector2 cursorPos = new Vector2(projectile.ai[0], projectile.ai[1]);
                player.direction = cursorPos.X > player.Center.X ? 1 : -1;
                Vector2 toCenter = new Vector2(22 * -player.direction, -22);
                Vector2 center = player.MountedCenter + toCenter;
                projectile.Center = center;
                overrideDirection = toCenter;
                Vector2 toCursor = cursorPos - projectile.Center;
                projectile.velocity = toCursor.SafeNormalize(Vector2.Zero) * projectile.velocity.Length();
                if (player.itemTime < itemUseTime)
                {
                    player.itemTime = itemUseTime;
                    player.itemAnimation = itemUseTime;
                }
            }
            else
            {
                if(counter == timeToLaunch)
                    Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 19, 1.0f, -0.1f);
                if (player.itemTime > itemUseTime)
                {
                    player.itemTime = itemUseTime;
                    player.itemAnimation = itemUseTime;
                }
                projectile.rotation += MathHelper.ToRadians(7 * (float)Math.Sqrt(projectile.velocity.Length()));
                projectile.velocity *= 0.98f;
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X - 4, projectile.position.Y - 4), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.scale *= 1.2f;
                dust.fadeIn = 0.1f;
                dust.color = new Color(97, 200, 225);
                dust.alpha = 100;
            }
            projectile.direction = overrideDirection.X > 0 ? 1 : -1;
            Lighting.AddLight(projectile.Center, new Color(97, 200, 225).ToVector3() * 0.5f);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 8;
            height = 8;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = oldVelocity;
            return counter > timeToLaunch + 2;
        }
        public void HitTiles()
        {
            Player player = Main.player[projectile.owner];
            for(int k = -2; k <= 2; k++)
            {
                for (int h = -2; h <= 2; h++)
                {
                    Vector2 center = projectile.Center;
                    int i = (int)center.X / 16 + k;
                    int j = (int)center.Y / 16 + h;
                    if (SOTSWorldgenHelper.CanExplodeTile(i, j))
                    {
                        WorldGen.KillTile(i, j, false, false, false);
                        if (Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
                    }
                    else if (Main.myPlayer == projectile.owner)
                        player.PickTile(i, j, 70);
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            HitTiles();
            Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 62, 0.7f, 0.4f);
            for (int i = 0; i < 360; i += 24)
            {
                Vector2 circularLocation = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(i));
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
                dust.noGravity = true;
                dust.velocity *= 1.5f;
                dust.velocity += circularLocation * 0.3f;
                dust.scale *= 1.4f;
                dust.fadeIn = 0.1f;
                dust.color = new Color(97, 200, 225);
                dust.alpha = 100;
            }
        }
    }
}
	
