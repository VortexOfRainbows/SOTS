using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.WorldgenHelpers;
using SOTS.Helpers;

namespace SOTS.Projectiles.Earth
{    
    public class EarthBreakerPickaxe : ModProjectile 
    {	
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44; 
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true; 
            Projectile.DamageType = DamageClass.Melee; 
			Projectile.alpha = 0;
            Projectile.scale = 0.9f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            DrawOutline(Main.spriteBatch, lightColor);
            Main.spriteBatch.Draw(texture, drawPos + new Vector2(0, player.gfxOffY), null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public void DrawOutline(SpriteBatch spriteBatch, Color drawColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/EarthBreakerPickaxeOutline");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float scale = 1 - counter / (float)(timeToLaunch - 8);
            if (scale > 1)
                scale = 1;
            else if (scale < 0)
                scale = 0;
            scale *= Projectile.scale;
            for (int i = 0; i < 6; i++)
            {
                Vector2 addition = new Vector2(0, 2 + 12 * scale).RotatedBy(MathHelper.ToRadians(i * 60 + 180 * scale * scale));
                spriteBatch.Draw(texture, drawPos + addition + new Vector2(0, player.gfxOffY), null, Color.White * (1 - scale) * (1 - scale), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/EarthBreakerPickaxeGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            for (int i = 0; i < 4; i++)
            {
                Vector2 addition = Main.rand.NextVector2Circular(1, 1);
                Main.spriteBatch.Draw(texture, drawPos + addition + new Vector2(0, player.gfxOffY), null, new Color(100, 100, 100, 0), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return counter >= timeToLaunch;
        }
        public const int itemUseTime = 30;
        public const float timeToLaunch = 30;
        int counter = 0;
        public override void AI()
		{
            Player player = Main.player[Projectile.owner];
            counter++;
            Vector2 overrideDirection = Projectile.velocity;
            if(counter < timeToLaunch)
            {
                if(counter == 1)
                    SOTSUtils.PlaySound(SoundID.Item15, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.8f, -0.2f);
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.ai[0] = Main.MouseWorld.X;
                    Projectile.ai[1] = Main.MouseWorld.Y;
                    Projectile.netUpdate = true;
                }
                Vector2 cursorPos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                player.direction = cursorPos.X > player.Center.X ? 1 : -1;
                Vector2 toCenter = new Vector2(22 * -player.direction, -22);
                Vector2 center = player.MountedCenter + toCenter;
                Projectile.Center = center;
                overrideDirection = toCenter;
                Vector2 toCursor = cursorPos - Projectile.Center;
                Projectile.velocity = toCursor.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
                if (player.itemTime < itemUseTime)
                {
                    player.itemTime = itemUseTime;
                    player.itemAnimation = itemUseTime;
                }
            }
            else
            {
                if(counter == timeToLaunch)
                    SOTSUtils.PlaySound(SoundID.Item19, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.0f, -0.1f);
                if (player.itemTime > itemUseTime)
                {
                    player.itemTime = itemUseTime;
                    player.itemAnimation = itemUseTime;
                }
                Projectile.rotation += MathHelper.ToRadians(7 * (float)Math.Sqrt(Projectile.velocity.Length()));
                Projectile.velocity *= 0.98f;
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X + 4, Projectile.position.Y + 4), Projectile.width - 8, Projectile.height - 8, ModContent.DustType<PixelDust>());
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.scale = 1.5f;
                dust.fadeIn = 6f;
                dust.color = ColorHelper.EarthColor;
                dust.color.A = 0;
                Projectile.velocity.Y += 0.1f;
            }
            Projectile.direction = overrideDirection.X > 0 ? 1 : -1;
            Lighting.AddLight(Projectile.Center, ColorHelper.EarthColor.ToVector3() * 0.5f);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 8;
            height = 8;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool die = counter > timeToLaunch + 2;
            Projectile.velocity = oldVelocity;
            return die;
        }
        public void HitTiles()
        {
            Player player = Main.player[Projectile.owner];
            for(int k = -2; k <= 2; k++)
            {
                for (int h = -2; h <= 2; h++)
                {
                    Vector2 center = Projectile.Center;
                    int i = (int)center.X / 16 + k;
                    int j = (int)center.Y / 16 + h;
                    if (Projectile.CanExplodeTile(i, j))
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            WorldGen.KillTile(i, j, false, false, false);
                        if (Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
                    }
                    else if (Main.myPlayer == Projectile.owner && NPC.downedBoss2 && Framing.GetTileSafely(i, j).TileType != TileID.DemonAltar)
                        player.PickTile(i, j, 59);
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            HitTiles();
            SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.7f, 0.4f);
            for (int i = 0; i < 360; i += 20)
            {
                Vector2 circularLocation = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(i));
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<PixelDust>());
                dust.noGravity = true;
                dust.velocity *= 1.6f;
                dust.velocity += circularLocation * 0.3f;
                dust.scale = 1.5f;
                dust.fadeIn = 4f;
                dust.color = ColorHelper.EarthColor;
                dust.color.A = 0;
            }
        }
    }
}
	
