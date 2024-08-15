using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Diagnostics;

namespace SOTS.Projectiles.AbandonedVillage
{
	public class FizzleStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 1200;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.extraUpdates = 2;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
		}
        public int FrameSpeed
        {
            get
            {
                return Projectile.ai[2] == -1 ? 6 * (Projectile.extraUpdates + 1) : Projectile.ai[2] == -2 ? 7 * (Projectile.extraUpdates + 1) : 1;
            }
        }
        public int FrameCount
        {
            get
            {
                return Projectile.ai[2] == -1 ? 5 : Projectile.ai[2] == -2 ? 4 : 1;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            float TrailWidth = 4;
            if (Projectile.ai[2] == -1)
            {
                texture = ModContent.Request<Texture2D>("SOTS/Projectiles/AbandonedVillage/FizzleSpark").Value;
                TrailWidth = 1;
            }
            if (Projectile.ai[2] == -2)
            {
                texture = ModContent.Request<Texture2D>("SOTS/Projectiles/AbandonedVillage/FizzleSpark2").Value;
                TrailWidth = 1;
            }
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2 / FrameCount);
            Color color = Color.White;
            Texture2D pixel = ModContent.Request<Texture2D>("SOTS/Items/Secrets/WhitePixel").Value;
            Vector2 previous = Projectile.Center;
            int currentFrame = (counter / FrameSpeed);
            int frameY = (texture.Height / FrameCount) * currentFrame;
            if (FrameCount == 1)
                frameY = 0;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 toPrev = previous - center;
                Color c = ColorHelpers.VibrantColorAttempt(i * 9f, true);
                if (Projectile.ai[2] < 0)
                {
                    c = new Color(23, 41, 118);
                    TrailWidth = FrameCount - currentFrame;
                }
                Main.spriteBatch.Draw(pixel, center - Main.screenPosition, null, c * perc, toPrev.ToRotation(), new Vector2(0, 1), new Vector2(toPrev.Length() / 2f, TrailWidth * perc), SpriteEffects.None, 0f);
                previous = center;
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, frameY, texture.Width, texture.Height / FrameCount), color * (1f - (Projectile.alpha / 305f)), Projectile.rotation, drawOrigin, 1.15f, SpriteEffects.None, 0f);
            return false;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = height = 10;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}
		public override bool PreAI()
		{
			return base.PreAI();
		}
        int counter = 0;
		public override void AI()
        {
            counter++;
            if (Projectile.ai[2] >= 0)
            {
                if (Projectile.owner == Main.myPlayer && counter % 5 == 0)
                {
                    if (Projectile.velocity.Length() != 0f)
                    {
                        Projectile.velocity = new Vector2(Projectile.velocity.Length() + Main.rand.NextFloat(-0.4f, 0.5f), 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(Projectile.ai[1]));
                    }
                    Projectile.ai[1] = Main.rand.NextFloat(-12, 12);
                    Projectile.netUpdate = true;
                    if ((Main.rand.NextFloat(100) < counter && !Main.rand.NextBool(3)) || (Main.rand.NextBool(20) && counter > 20))
                    {
                        Projectile.Kill();
                    }
                }
            }
            else
            {
                if(counter / FrameSpeed >= FrameCount)
                {
                    Projectile.Kill();
                }
            }
			Projectile.velocity *= 0.9865f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[2] >= 0)
            {
                if(Main.myPlayer == Projectile.owner)
                {
                    int count = Main.rand.Next(3, 6);
                    for(int i = 0; i < count; i++)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.5f + Main.rand.NextVector2Circular(3, 3), Type, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 0, Main.rand.NextFromList(-1, -2));
                    }
                }
            }
        }
    }
}
		