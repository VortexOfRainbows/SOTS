using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using SOTS.Items.Planetarium.Furniture;

namespace SOTS.Projectiles.Permafrost
{
	public class PolarisLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
		}
		public override void SetDefaults() 
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.timeLeft = 30;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            VoidPlayer.VoidDamage(Mod, target, 15);
        }
        public override bool ShouldUpdatePosition()
        {
			return false;
		}
		Vector2 finalPosition = Vector2.Zero;
		bool hasInit = false;
		float scaleMult = 1f;
		public override void AI() 
		{
			if(!hasInit)
			{
				SOTSUtils.PlaySound(SoundID.Item92, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.1f, -0.4f);
				for (int i = 20; i > 0; i--)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(12, 4), 20, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, DustColor, 1.4f);
					dust.noGravity = true;
					dust.velocity *= 1.5f;
					dust.velocity += Projectile.velocity * Main.rand.NextFloat(1f, 2f);
					dust.fadeIn = 0.2f;
                }
                InitializeLaser();
            }
		}
		public Color DustColor
		{
			get
			{
				if (Projectile.ai[0] == 0 || (Main.rand.NextBool(2) && (Projectile.ai[0] == 1 || Projectile.ai[0] == 3)))
					return new Color(250, 100, 100, 200);
                return new Color(100, 100, 250, 200);
            } 
		}
		public void InitializeLaser()
		{
			Vector2 startingPosition = Projectile.Center;
			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
			for (int b = 0; b < 800; b++)
			{
				startingPosition += Projectile.velocity * 2.5f;
				finalPosition = startingPosition;
				int i = (int)startingPosition.X / 16;
				int j = (int)startingPosition.Y / 16;
				if (WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(i, j))
				{
					break;
				}
                Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, DustColor, 1.0f);
                dust.noGravity = true;
                dust.velocity *= 1.0f;
                dust.velocity += Projectile.velocity * Main.rand.NextFloat(4f, 7f);
                dust.scale *= 1.2f;
                dust.fadeIn = 0.2f;
            }
            for (int i = 20; i > 0; i--)
            {
                Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(12, 4), 20, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, DustColor, 1.75f);
                dust.noGravity = true;
                dust.velocity *= 1.0f;
                dust.velocity += Projectile.velocity * Main.rand.NextFloat(3f, 5f);
                dust.fadeIn = 0.2f;
            }
            hasInit = true;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			if(Projectile.timeLeft > 25)
			{
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, finalPosition, 24f * scaleMult * Projectile.scale, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
		}
		public static void Draw(SpriteBatch spriteBatch, Vector2 start, Vector2 end, int Type, float alphaScale, float progressMultiplier = 1f)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<PolarisLaser>()].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 4);
			Vector2 toEnd = end - start;
			float maxLength = toEnd.Length() / texture.Height * 4;
			Color color;
			float xScale = 1f;
			if (progressMultiplier != 1)
			{
                xScale = 1f / texture.Width * 4f;
            }
			for (int j = -1; j <= 1; j += 2)
            {
                Rectangle frame = new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2);
                if (Type == 0)
                {
                    frame = new Rectangle(0, 0, texture.Width, texture.Height / 2);
                }
                if (Type == 1)
                {
                    if (j == -1)
                        frame = new Rectangle(0, 0, texture.Width, texture.Height / 2);
                    else
                        frame = new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2);
                }
                if (Type == 2)
                {
                    frame = new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2);
                }
                if (Type == 3)
                {
                    if (j == -1)
                        frame = new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2);
                    else
                        frame = new Rectangle(0, 0, texture.Width, texture.Height / 2);
                }
                Vector2 offset = Vector2.Zero;
				float percent = 0f;
				color = new Color(100, 100, 100, 0);
				for (float i = 0; i < maxLength * progressMultiplier; i++)
				{
					if (percent < 1)
						percent += 0.1f;
					if (!SOTS.Config.lowFidelityMode || (int)(i % 2) == 0)
					{
						Vector2 position = Vector2.Lerp(start, end, i / maxLength);
						Vector2 sinusoid = j * new Vector2(0, 8 * (float)Math.Sin(MathHelper.ToRadians(i * 4))).RotatedBy(toEnd.ToRotation()) * percent;
						Vector2 drawPos = position - Main.screenPosition + sinusoid * progressMultiplier;
						spriteBatch.Draw(texture, drawPos + offset, frame, color * alphaScale * percent * progressMultiplier, toEnd.ToRotation() + MathHelper.PiOver2, origin, new Vector2(xScale, 2f), SpriteEffects.None, 0f);
					}
				}
			}
			return;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            if (!hasInit)
                return false;
            float alphaScale = Projectile.timeLeft / 30f;
            Draw(Main.spriteBatch, Projectile.Center, finalPosition, (int)Projectile.ai[0], alphaScale);
			return false;
		}
    }
}