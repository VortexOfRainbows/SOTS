using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Master
{
    public class LuxBall : ModItem
    {
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.DefaultToGolfBall(ModContent.ProjectileType<LuxBallProjectile>());
            Item.master = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) => itemGroup = ContentSamples.CreativeHelper.ItemGroup.Golf;
    }
    public class LuxBallProjectile : ModProjectile
    {
        private Vector2[] Trail => Projectile.oldPos;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D TrailTexture = SOTSUtils.WhitePixel;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            float trailSize = 3.0f;
            Vector2 drawOrigin = new(0f, TrailTexture.Height * 0.5f);
            Vector2 previousPosition = Projectile.Center;
            for (int i = 0; i < Trail.Length; i++)
            {
                if (Trail[i] == Vector2.Zero)
                {
                    continue;
                }
                float percent = (Trail.Length - i) / (float)Trail.Length;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Color color = ColorHelper.Pastel(percent * MathF.PI * 4, true);
                color.A = 0;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Vector2 toPrev = previousPosition - center;
                Main.spriteBatch.Draw(TrailTexture, center - Main.screenPosition, null, color * perc, toPrev.ToRotation(), new Vector2(0, 1), new Vector2(toPrev.Length() / 2f, trailSize * perc), SpriteEffects.None, 0f);
                previousPosition = center;
            }
            return base.PreDraw(ref lightColor);
        }
        public override bool PreDrawExtras()
        {
            return base.PreDrawExtras();
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAGolfBall[Type] = true;
            ProjectileID.Sets.TrailingMode[Type] = 0;
            ProjectileID.Sets.TrailCacheLength[Type] = 60;
        }
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = Projectile.height = 7;
            Projectile.friendly = true; 
            Projectile.penetrate = -1;
            Projectile.aiStyle = 149;
            Projectile.tileCollide = false; 
        }
        public override bool PreAI()
        {
            if(MathF.Abs(Projectile.velocity.X) > 0)
            {
                Projectile.damage = (int)(25 + 15 * Projectile.velocity.Length());
                //Projectile.velocity.Y -= 0.2f;
            }
            else
            {
                Projectile.damage = 0; 
            }
            Lighting.AddLight(Projectile.Center, 238f / 255f, 145f / 255f, 219f / 255f);
            return base.PreAI();
        }
        public override void OnKill(int timeLeft)
        {
            Vector2 previous = Projectile.Center;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                for (float j = 0; j < 1; j += 0.2f)
                {
                    float percent = (Trail.Length - i - j) / (float)Trail.Length;
                    Color color = ColorHelper.Pastel(percent * MathF.PI * 4, true);
                    color.A = 0;
                    float perc = 1 - (i + j) / (float)Projectile.oldPos.Length;
                    Dust d = Dust.NewDustDirect(Vector2.Lerp(center, previous, j) - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), newColor: color);
                    d.velocity *= 0.1f * perc;
                    d.noGravity = true;
                    d.fadeIn = 7f;
                    d.scale = d.scale * 0.5f + 2.5f * perc;
                }
                previous = center;
            }
        }
    }
}