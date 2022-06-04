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
            Projectile.width = 22;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 100;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 0.8f;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/Starshot");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = Color.White;
            Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
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
            Projectile.ai[0] += 0.225f;
            if (speed <= 0.1f && Projectile.ai[0] < 100)
            {
                Projectile.ai[0] *= 1.045f;
            }
            if (Projectile.alpha > 0)
                Projectile.alpha -= 5;
            for(int i = 0; i < 2; i++)
            {
                dist += speed;
                speed *= 0.9f;
            }
            Player player = Main.player[Projectile.owner];
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 toCursor = Main.MouseWorld - player.Center;
                Vector2 fromPlayer = new Vector2(dist, 0).RotatedBy(toCursor.ToRotation());
                Projectile.Center = fromPlayer + player.Center;
                Projectile.velocity = toCursor.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
                Projectile.netUpdate = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            if (!player.channel) ended = true;
            if (ended && speed <= 0.1f)
            {
                SOTSUtils.PlaySound(SoundID.Item9, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.75f);
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/StarshotTrail").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/Starshot").Value;
            float compression = (100f - Projectile.ai[0]) / 100f;
            if (compression < 0)
                compression = 0;
            for (int d = -2; d < 3; d++)
            {
                Vector2 drawPos = Projectile.Center;
                Color color = new Color(100, 100, 100, 0) * ((100f - Projectile.alpha) / 255f);
                float rotation = 0f;
                Vector2 angle = Projectile.velocity.RotatedBy(MathHelper.ToRadians(d * 72 * compression));
                rotation = angle.ToRotation();
                for(int k = 0; k < 200; k++)
                {
                    drawPos += new Vector2(texture.Height, 0).RotatedBy(angle.ToRotation());
                    int i = (int)drawPos.X / 16;
                    int j = (int)drawPos.Y / 16;
                    Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color, rotation + MathHelper.ToRadians(90), new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0f);
                    if (!WorldGen.InWorld(i, j, 20) || Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true)
                    {
                        break;
                    }
                }
            }
            float distance = 32 * ((100f - Projectile.ai[0]) / 100f);
            if (distance < 0) distance = 0;

            for (int i = 0; i < 5; i++)
            {
                Vector2 drawPos = Projectile.Center;
                Color color = new Color(90, 90, 90, 0) * ((100f - Projectile.alpha) / 255f);
                float angle = Projectile.rotation + MathHelper.ToRadians(Projectile.ai[0] * 5);
                drawPos += new Vector2(distance, 0).RotatedBy(angle + MathHelper.ToRadians(72 * i));
                for (int j = 0; j < 4; j++)
                {
                    float x = Main.rand.Next(-10, 11) * 0.5f * (1 - compression);
                    float y = Main.rand.Next(-10, 11) * 0.5f * (1 - compression);
                    Main.spriteBatch.Draw(texture2, drawPos - Main.screenPosition + new Vector2(x, y), null, color, Projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), Projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            float compression = ((100f - Projectile.ai[0]) / 100f);
            if (compression < 0)
                compression = 0;
            for (int i = -2; i < 3; i++)
            {
                Vector2 angle = Projectile.velocity.RotatedBy(MathHelper.ToRadians(i * 72 * compression));
                if (Projectile.owner == Main.myPlayer)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, angle, (int)Projectile.ai[1], Projectile.damage, Projectile.knockBack, Main.myPlayer);
            }

            for (int i = 0; i < 30; i++)
            {
                int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
                Dust dust = Main.dust[num1];
                dust.velocity *= 0.2f;
                dust.noGravity = true;
                dust.scale += 0.1f;
                dust.color = Main.rand.NextBool(2) ? new Color(0, 200, 220, 100) : new Color(220, 200, 30, 100);
                dust.fadeIn = 0.1f;
                dust.scale *= 1.6f;
                dust.alpha = Projectile.alpha;
            }

            base.Kill(timeLeft);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = true;
            return true;
        }
    }
}