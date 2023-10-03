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
            // DisplayName.SetDefault("Starshot");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 0.8f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Laser/PrismLaser").Value;
            Texture2D texture2 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            float compression = (100f - Projectile.ai[0]) / 100f;
            if (compression < 0)
                compression = 0;
            Vector2 drawPos;
            Color color;
            int i = 0;
            float counter = 160 + Main.GlobalTimeWrappedHourly;
            float scale = Projectile.scale * 0.75f - 0.25f * compression;
            for (int d = -3; d < 4; d++)
            {
                drawPos = Projectile.Center;
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
                color *= ((100f - Projectile.alpha) / 255f);
                float rotation = 0f;
                Vector2 angle = Projectile.velocity.RotatedBy(MathHelper.ToRadians(d * 45 * compression));
                rotation = angle.ToRotation();
                for (int a = 0; a < 200; a++)
                {
                    drawPos += new Vector2(texture.Height * scale, 0).RotatedBy(angle.ToRotation());
                    int k = (int)drawPos.X / 16;
                    int j = (int)drawPos.Y / 16;
                    Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + dynamicAddition, null, color, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
                    if (!WorldGen.InWorld(k, j, 20) || Main.tile[k, j].HasTile && Main.tileSolidTop[Main.tile[k, j].TileType] == false && Main.tileSolid[Main.tile[k, j].TileType] == true)
                    {
                        break;
                    }
                }
            }
            float distance = 32 * ((100f - Projectile.ai[0]) / 100f);
            if (distance < 0) distance = 0;

            for (i = 0; i < 7; i++)
            {
                drawPos = Projectile.Center;
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
                color *= ((100f - Projectile.alpha) / 255f);
                float angle = Projectile.rotation + MathHelper.ToRadians(Projectile.ai[0] * 5);
                drawPos += new Vector2(distance, 0).RotatedBy(angle + MathHelper.ToRadians(360f / 7f * i));
                float x = Main.rand.Next(-10, 11) * 0.5f * (1 - compression);
                float y = Main.rand.Next(-10, 11) * 0.5f * (1 - compression);
                Main.spriteBatch.Draw(texture2, drawPos - Main.screenPosition + new Vector2(x, y) + dynamicAddition, null, color, Projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), Projectile.scale, SpriteEffects.None, 0f);
            }
            // Vector2 drawOrigin = new Vector2(texture2.Width / 2, texture2.Height / 2);
            // drawPos = Projectile.Center - Main.screenPosition;
            // color = Color.White;
            // spriteBatch.Draw(texture2, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
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
            Projectile.spriteDirection = Projectile.direction;
            Projectile.ai[0] += 0.35f;
            if (Projectile.ai[0] < 100)
            {
                Projectile.ai[0] *= 1.035f;
            }
            else
            {
                Projectile.ai[0] = 100;
                ai2++;
            }
            Player player = Main.player[Projectile.owner];
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 toCursor = Main.MouseWorld - player.Center;
                Vector2 fromPlayer = new Vector2(dist, 0).RotatedBy(toCursor.ToRotation());
                Projectile.Center = fromPlayer + player.Center;
                Projectile.position += new Vector2(0, 4 * Projectile.spriteDirection).RotatedBy(toCursor.ToRotation());
                Projectile.velocity = toCursor.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
                Projectile.netUpdate = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            if (!player.channel && Projectile.ai[0] >= 100 && ai2 >= 5) ended = true;
            if (ended)
            {
                SOTSUtils.PlaySound(SoundID.Item94, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
                Projectile.Kill();
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
                for (int i = 0; i < 7; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<PrismLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, i);
                }   
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = true;
            return true;
        }
    }
}