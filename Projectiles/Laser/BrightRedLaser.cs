using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Projectiles.Laser
{
	public class BrightRedLaser : ModProjectile
	{
		public override void SetDefaults() 
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.usesLocalNPCImmunity = true;
		}
		public void DustScatter(Vector2 position, float mult = 1f)
        {
            Vector2 start = Projectile.Center;
            Vector2 destination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Vector2 toDestination = destination - start;
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(position - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.0f);
                dust.scale *= 0.2f;
                dust.scale += 1.4f;
                dust.velocity *= 0.45f;
                dust.velocity += Main.rand.NextFloat() * mult * toDestination.SafeNormalize(Vector2.Zero);
                dust.noGravity = true;
                dust.color = Color.Lerp(coreColor, Color.White, Main.rand.NextFloat(0.5f));
                dust.fadeIn = 0.2f;
            }
        }
		public override void AI() 
		{
			if ((int)Projectile.localAI[0] == 0)
			{
                if(Projectile.knockBack <= 0)
				    SOTSUtils.PlaySound(SoundID.Item12, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.7f, -0.2f);
                else
                    SOTSUtils.PlaySound(SoundID.Item33, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.5f, 0.5f);
                Vector2 start = Projectile.Center;
                DustScatter(start, -0.2f);
				Vector2 destination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                DustScatter(destination, 1.2f);
                float length = start.Distance(destination);
                float increment = 4f / length;
                for (float f = 0; f < 1; f += increment)
                {
                    Vector2 position = Vector2.Lerp(start, destination, f);
                    Dust dust = Dust.NewDustDirect(position - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.0f);
					dust.scale *= 0.1f;
					dust.scale += 0.9f;
					dust.velocity *= 0.6f;
                    dust.noGravity = true;
					dust.color = Color.Lerp(coreColor, Color.White, Main.rand.NextFloat(0.6f));
                    dust.fadeIn = 0.2f;
                }
			}
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 5f) 
			{
				Projectile.friendly = false;
			}
			if (Projectile.localAI[0] > 15f) 
			{
				Projectile.Kill();
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			Vector2 endPoint = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 20f, ref point))
            {
                return true;
            }
            return false;
            //return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
        }
        public Color coreColor => Projectile.knockBack > 0 ? new Color(187, 31, 96, 0) : new Color(255, 10, 10, 0);
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(0, texture.Height / 2);
            Vector2 start = Projectile.Center;
			Vector2 destination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			float length = start.Distance(destination);
			float rotation = (destination - start).ToRotation();
			float progress = Projectile.localAI[0] / 15f;
			float increment = 2f / length;
            length /= texture.Width;
            for (float f = 0; f < 1; f += increment)
			{
				Vector2 position = Vector2.Lerp(start, destination, f);
                for (int i = -2; i <= 2; i++)
                {
                    Vector2 offset = new Vector2(0, i * 3 * progress * (float)Math.Sin(f * MathHelper.Pi)).RotatedBy(rotation);
                    Main.spriteBatch.Draw(texture, position + offset - Main.screenPosition, null, coreColor * (1 - progress), rotation, origin, new Vector2(length * increment, 0.5f * (1 - progress)), SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(texture, position + offset - Main.screenPosition, null, new Color(100, 100, 100, 0) * (1 - progress), rotation, origin, new Vector2(length * increment, 0.35f * (1 - progress)), SpriteEffects.None, 0f);
                }
            }
            return false;
        }
    }
}