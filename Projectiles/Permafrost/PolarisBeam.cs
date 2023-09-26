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
using SOTS.Buffs;
using Terraria.GameContent.ItemDropRules;

namespace SOTS.Projectiles.Permafrost	
{    
    public class PolarisBeam : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
        {
            Draw(1f, Projectile.Center, true);
			return false;
		}
		public void Draw(float alphaMult, Vector2 pos, bool outLine = true)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(24, texture.Height / 4);
			Color color = new Color(100, 100, 100, 0);
			Rectangle frame = new Rectangle(0, 0, 24, 10);
			if ((int)Projectile.ai[0] == 1)
                frame = new Rectangle(0, 12, 24, 10);
			Vector2 previous = Projectile.Center;
			if(Projectile.timeLeft < 480)
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2;
					float length = (drawPos - previous).Length();
					if(length < 300)
                    {
                        float trailMult = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                        Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, frame, Projectile.GetAlpha(color) * trailMult * trailMult, Projectile.rotation, origin, new Vector2(length / 6f, 1f), SpriteEffects.None, 0.0f);
                        previous = drawPos;
                    }
				}
            if (outLine)
				for (int i = 0; i < 4; i++)
				{
					Vector2 circular = new Vector2(1f, 0).RotatedBy(i * MathHelper.PiOver2);
					Main.spriteBatch.Draw(texture, pos + circular - Main.screenPosition, frame, Projectile.GetAlpha(color) * 0.5f * alphaMult * alphaMult, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
				}
            Main.spriteBatch.Draw(texture, pos - Main.screenPosition, frame, Projectile.GetAlpha(color) * alphaMult * alphaMult, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
		}
		public override void SetDefaults()
        {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.timeLeft = 480;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.ignoreWater = true;
		}
		bool runOnce = true;
		public override void AI()
        {
            Color color = Projectile.ai[0] == 1 ? new Color(100, 100, 250, 200) : new Color(250, 100, 100, 200);
            if (Projectile.timeLeft < 24)
			{
				Projectile.alpha += 20;
			}
			if (runOnce)
            {
				for(int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.Zero) * 16 - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
                    dust.velocity *= 0.85f;
                    dust.velocity += Projectile.velocity * 0.5f;
                    dust.noGravity = true;
                    dust.scale = dust.scale * 0.75f + 0.75f;
                    dust.color = color;
                    dust.fadeIn = 0.1f;
                    dust.alpha = Projectile.alpha;
                }
				runOnce = false;
            }
			Vector2 velo = Projectile.velocity * Projectile.ai[1];
			Projectile.position += velo;
			Lighting.AddLight(Projectile.Center, color.ToVector3() * 0.5f);
			if (Projectile.velocity.Length() > 1)
			{
				int chance = 20;
				chance -= (int)Projectile.velocity.Length();
				if (chance <= 1 || Main.rand.NextBool(chance))
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
					dust.velocity *= 0.1f;
					dust.velocity += Projectile.velocity * 0.1f;
					dust.noGravity = true;
					dust.scale += 0.5f;
					dust.color = color;
					dust.fadeIn = 0.1f;
					dust.alpha = Projectile.alpha;
				}
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			Projectile.ai[1] += 1 / 30f;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
	}
}