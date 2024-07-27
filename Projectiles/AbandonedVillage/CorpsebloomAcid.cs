using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;

namespace SOTS.Projectiles.AbandonedVillage
{
    public class CorpsebloomAcid : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1800;
			Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = 0;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 6;
            height = 6;
            return true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Poisoned, 300);
        }
        private bool RunOnce = true;
        public override bool PreAI()
        {
            if(RunOnce)
            {
                RunOnce = false;
                Projectile.velocity *= 0.5f;
            }
            return base.PreAI();
        }
        public override void AI()       
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.rotation += 0f * (float)Projectile.direction;

            Projectile.velocity.Y += 0.02f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Texture2D textureT = ModContent.Request<Texture2D>("SOTS/Projectiles/BiomeChest/PlagueBeam").Value;
            Color color = ColorHelpers.ToothAcheLime;
            color.A = 0;
            SpriteEffects effects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
            Vector2 drawOriginT = new Vector2(0, textureT.Height / 2);
            int trailLen = Projectile.oldPos.Length;
            Vector2 oldPos = Projectile.Center;  
            for (int i = 0; i < trailLen; i++)
            {
                if (Projectile.oldPos[i] != Vector2.Zero)
                {
                    float scale = Projectile.scale * (trailLen - i) / Projectile.oldPos.Length * 1f;
                    Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2;
                    Vector2 toOldPosition = oldPos - drawPos;
                    Main.EntitySpriteDraw(textureT, drawPos - Main.screenPosition, null, color * scale * 0.5f, toOldPosition.ToRotation(), drawOriginT, new Vector2(toOldPosition.Length() / textureT.Width * 1.0f, scale * 1.5f), SpriteEffects.None, 0f);
                    oldPos = Projectile.oldPos[i] + Projectile.Size / 2;
                }
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(ColorHelpers.ToothAcheLime, Color.Black, 0.5f), Projectile.rotation, drawOrigin, 0.75f, effects, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, color * 0.75f, Projectile.rotation, drawOrigin, 1f, effects, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, color * 0.5f, Projectile.rotation, drawOrigin, 0.75f, effects, 0);
            return false;
        }

		public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float scale = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2;
                Dust dust = Dust.NewDustDirect(drawPos + new Vector2(-5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, ColorHelpers.ToothAcheLime * scale);
                dust.noGravity = true;
                dust.scale = 2f * scale;
                dust.velocity *= 0.3f;
                dust.velocity += Projectile.oldVelocity * Main.rand.NextFloat();
                dust.fadeIn = 0.1f;
                dust.color.A = 0;
            }
            for (int i = 12; i > 0; i--)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, ColorHelpers.ToothAcheLime, 1f);
                dust.noGravity = true;
                dust.velocity = dust.velocity * Main.rand.NextFloat(0.75f) + Projectile.velocity * Main.rand.NextFloat(1f);
                dust.fadeIn = 5;
                dust.scale = Main.rand.Next(4, 9) / 4f;
                dust.color.A = 0;
            }
        }
    }
}