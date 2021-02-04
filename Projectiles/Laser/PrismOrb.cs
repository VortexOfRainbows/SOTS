using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Laser
{
    public class PrismOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starshot");
        }
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = false;
            projectile.ranged = true;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.light = 0.8f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/Laser/PrismLaser");
            Texture2D texture2 = Main.projectileTexture[projectile.type];
            float compression = (100f - projectile.ai[0]) / 100f;
            if (compression < 0)
                compression = 0;
            Vector2 drawPos;
            Color color;
            int i = 0;
            float counter = 160 + Main.GlobalTime;
            float scale = projectile.scale * 0.75f - 0.25f * compression;
            for (int d = -3; d < 4; d++)
            {
                drawPos = projectile.Center;
                color = Color.White;
                
                switch (i)
                {
                    case 0:
                        color = new Color(255, 0, 0, 0);
                        break;
                    case 1:
                        color = new Color(255, 140, 0, 0);
                        break;
                    case 2:
                        color = new Color(255, 255, 0, 0);
                        break;
                    case 3:
                        color = new Color(0, 255, 0, 0);
                        break;
                    case 4:
                        color = new Color(0, 255, 255, 0);
                        break;
                    case 5:
                        color = new Color(0, 0, 255, 0);
                        break;
                    case 6:
                        color = new Color(140, 0, 255, 0);
                        break;
                }
                Vector2 dynamicAddition = new Vector2(Main.rand.NextFloat(2, 4) * (1 - 1f * compression), 0).RotatedBy(MathHelper.ToRadians(360f / 7f * i) + counter);
                i++;
                color *= ((100f - projectile.alpha) / 255f);
                float rotation = 0f;
                Vector2 angle = projectile.velocity.RotatedBy(MathHelper.ToRadians(d * 45 * compression));
                rotation = angle.ToRotation();
                for (int a = 0; a < 200; a++)
                {
                    drawPos += new Vector2(texture.Height * scale, 0).RotatedBy(angle.ToRotation());
                    int k = (int)drawPos.X / 16;
                    int j = (int)drawPos.Y / 16;
                    spriteBatch.Draw(texture, drawPos - Main.screenPosition + dynamicAddition, null, color, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
                    if (!WorldGen.InWorld(k, j, 20) || Main.tile[k, j].active() && Main.tileSolidTop[Main.tile[k, j].type] == false && Main.tileSolid[Main.tile[k, j].type] == true)
                    {
                        break;
                    }
                }
            }
            float distance = 32 * ((100f - projectile.ai[0]) / 100f);
            if (distance < 0) distance = 0;

            for (i = 0; i < 7; i++)
            {
                drawPos = projectile.Center;
                color = Color.White;
                switch (i)
                {
                    case 0:
                        color = new Color(255, 0, 0, 0);
                        break;
                    case 1:
                        color = new Color(255, 140, 0, 0);
                        break;
                    case 2:
                        color = new Color(255, 255, 0, 0);
                        break;
                    case 3:
                        color = new Color(0, 255, 0, 0);
                        break;
                    case 4:
                        color = new Color(0, 255, 255, 0);
                        break;
                    case 5:
                        color = new Color(0, 0, 255, 0);
                        break;
                    case 6:
                        color = new Color(140, 0, 255, 0);
                        break;
                }
                Vector2 dynamicAddition = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(360f / 7f * i) + counter);
                color *= ((100f - projectile.alpha) / 255f);
                float angle = projectile.rotation + MathHelper.ToRadians(projectile.ai[0] * 5);
                drawPos += new Vector2(distance, 0).RotatedBy(angle + MathHelper.ToRadians(360f / 7f * i));
                float x = Main.rand.Next(-10, 11) * 0.5f * (1 - compression);
                float y = Main.rand.Next(-10, 11) * 0.5f * (1 - compression);
                spriteBatch.Draw(texture2, drawPos - Main.screenPosition + new Vector2(x, y) + dynamicAddition, null, color, projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), projectile.scale, SpriteEffects.None, 0f);
            }
            // Vector2 drawOrigin = new Vector2(texture2.Width / 2, texture2.Height / 2);
            // drawPos = projectile.Center - Main.screenPosition;
            // color = Color.White;
            // spriteBatch.Draw(texture2, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        float dist = 44;
        int ai2 = 0;
        bool ended = false;
        public override void AI()
        {
            projectile.spriteDirection = projectile.direction;
            projectile.ai[0] += 0.35f;
            if (projectile.ai[0] < 100)
            {
                projectile.ai[0] *= 1.035f;
            }
            else
            {
                projectile.ai[0] = 100;
                ai2++;
            }
            Player player = Main.player[projectile.owner];
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 toCursor = Main.MouseWorld - player.Center;
                Vector2 fromPlayer = new Vector2(dist, 0).RotatedBy(toCursor.ToRotation());
                projectile.Center = fromPlayer + player.Center;
                projectile.position += new Vector2(0, 4 * projectile.spriteDirection).RotatedBy(toCursor.ToRotation());
                projectile.velocity = toCursor.SafeNormalize(Vector2.Zero) * projectile.velocity.Length();
                projectile.netUpdate = true;
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            if (!player.channel && projectile.ai[0] >= 100 && ai2 >= 5) ended = true;
            if (ended)
            {
                Main.PlaySound(SoundID.Item94, (int)(projectile.Center.X), (int)(projectile.Center.Y));
                projectile.Kill();
            }
        }
        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer == projectile.owner)
                for (int i = 0; i < 7; i++)
                {
                    Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<PrismLaser>(), projectile.damage, projectile.knockBack, Main.myPlayer, i);
                }
            base.Kill(timeLeft);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = true;
            return true;
        }
    }
}