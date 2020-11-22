using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Otherworld
{
    public class Starshot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starshot");
        }
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.alpha = 100;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.light = 0.8f;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage /= 2;
            damage += 1;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        float speed = 7;
        float dist = 0;
        bool ended = false;
        public override void AI()
        {
            projectile.ai[0] += 0.225f;
            if (speed <= 0.1f && projectile.ai[0] < 100)
            {
                projectile.ai[0] *= 1.045f;
            }
            if (projectile.alpha > 0)
                projectile.alpha -= 5;
            for(int i = 0; i < 2; i++)
            {
                dist += speed;
                speed *= 0.9f;
            }
            Player player = Main.player[projectile.owner];
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 toCursor = Main.MouseWorld - player.Center;
                Vector2 fromPlayer = new Vector2(dist, 0).RotatedBy(toCursor.ToRotation());
                projectile.Center = fromPlayer + player.Center;
                projectile.velocity = toCursor.SafeNormalize(Vector2.Zero) * projectile.velocity.Length();
                projectile.netUpdate = true;
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            if (!player.channel) ended = true;
            if (ended && speed <= 0.1f)
            {
                Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 9, 0.75f);
                projectile.Kill();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/Otherworld/StarshotTrail");
            Texture2D texture2 = mod.GetTexture("Projectiles/Otherworld/Starshot");
            float compression = (100f - projectile.ai[0]) / 100f;
            if (compression < 0)
                compression = 0;
            for (int d = -2; d < 3; d++)
            {
                Vector2 drawPos = projectile.Center;
                Color color = new Color(100, 100, 100, 0) * ((100f - projectile.alpha) / 255f);
                float rotation = 0f;
                Vector2 angle = projectile.velocity.RotatedBy(MathHelper.ToRadians(d * 72 * compression));
                rotation = angle.ToRotation();
                for(int k = 0; k < 200; k++)
                {
                    drawPos += new Vector2(texture.Height, 0).RotatedBy(angle.ToRotation());
                    int i = (int)drawPos.X / 16;
                    int j = (int)drawPos.Y / 16;
                    spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color, rotation + MathHelper.ToRadians(90), new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale, SpriteEffects.None, 0f);
                    if (!WorldGen.InWorld(i, j, 20) || Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
                    {
                        break;
                    }
                }
            }
            float distance = 32 * ((100f - projectile.ai[0]) / 100f);
            if (distance < 0) distance = 0;

            for (int i = 0; i < 5; i++)
            {
                Vector2 drawPos = projectile.Center;
                Color color = new Color(90, 90, 90, 0) * ((100f - projectile.alpha) / 255f);
                float angle = projectile.rotation + MathHelper.ToRadians(projectile.ai[0] * 5);
                drawPos += new Vector2(distance, 0).RotatedBy(angle + MathHelper.ToRadians(72 * i));
                for (int j = 0; j < 4; j++)
                {
                    float x = Main.rand.Next(-10, 11) * 0.5f * (1 - compression);
                    float y = Main.rand.Next(-10, 11) * 0.5f * (1 - compression);
                    spriteBatch.Draw(texture2, drawPos - Main.screenPosition + new Vector2(x, y), null, color, projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            float compression = ((100f - projectile.ai[0]) / 100f);
            if (compression < 0)
                compression = 0;
            for (int i = -2; i < 3; i++)
            {
                Vector2 angle = projectile.velocity.RotatedBy(MathHelper.ToRadians(i * 72 * compression));
                if (projectile.owner == Main.myPlayer)
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, angle.X, angle.Y, (int)projectile.ai[1], projectile.damage, projectile.knockBack, Main.myPlayer);
            }

            for (int i = 0; i < 30; i++)
            {
                int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("CopyDust4"));
                Dust dust = Main.dust[num1];
                dust.velocity *= 0.2f;
                dust.noGravity = true;
                dust.scale += 0.1f;
                dust.color = Main.rand.NextBool(2) ? new Color(0, 200, 220, 100) : new Color(220, 200, 30, 100);
                dust.fadeIn = 0.1f;
                dust.scale *= 1.6f;
                dust.alpha = projectile.alpha;
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