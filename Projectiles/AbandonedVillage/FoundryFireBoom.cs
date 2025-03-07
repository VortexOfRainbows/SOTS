using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using XPT.Core.Audio.MP3Sharp.Decoding.Decoders.LayerIII;
using SOTS.Dusts;

namespace SOTS.Projectiles.AbandonedVillage
{
    public class FoundryFireBoom : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D ProjTexture = TextureAssets.Projectile[Type].Value;

            Vector2 drawOrigin = new(ProjTexture.Width * 0.5f, ProjTexture.Height * 0.5f);

            Vector2 vector = new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
            Rectangle rectangle = new(0, ProjTexture.Height / Main.projFrames[Projectile.type] * Projectile.frame, ProjTexture.Width, ProjTexture.Height / Main.projFrames[Projectile.type]);

            Color color1 = Projectile.GetAlpha( new Color(125, 125, 125, 0).MultiplyRGBA(Color.OrangeRed));
            Color color2 = Projectile.GetAlpha( new Color(125, 125, 125, 0).MultiplyRGBA(Color.Red));

            for (int i = 0; i < 360; i += 90)
            {
                Vector2 circular = new Vector2(Main.rand.NextFloat(1f, 2f), Main.rand.NextFloat(1f, 2f)).RotatedBy(MathHelper.ToRadians(i));

                Main.EntitySpriteDraw(ProjTexture, vector + circular, rectangle, color1, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(ProjTexture, vector + circular, rectangle, color2, Projectile.rotation, drawOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0);
            }

            return false;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }
        public override void AI()
        {
            if (Projectile.ai[0] < 9)
            {
                Projectile.alpha = 0;
                if (Projectile.ai[0] == 0)
                {
                    for (int i = 30; i > 0; i--)
                    {
                        Color c = Color.Lerp(Color.Red, Color.OrangeRed, Main.rand.NextFloat(1));
                        c.A = 0;
                        PixelDust.Spawn(Projectile.Center, 0, 0, Main.rand.NextVector2Circular(4, 4), c, Main.rand.Next(4, 6)).scale = Main.rand.NextFloat(1, 2f);
                    }
                    Projectile.scale = 0;
                }
                Projectile.ai[0]++;
                if(Projectile.scale < 1)
                {
                    Projectile.scale += 0.05f;
                    Projectile.scale *= 1.15f;
                }
            }
            else
            {
                Projectile.alpha += 22;
                if (Projectile.alpha >= 255 || Projectile.scale <= 0)
                {
                    Projectile.Kill();
                }
            }
        }
    }
}