using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.AbandonedVillage	
{    
    public class ExcavatorBolt : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 1;
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
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Color color = new Color(100, 100, 100, 0);
			Vector2 previous = Projectile.Center;
			if(Projectile.ai[0] >= 2)
            {
				Color c = ExcavatorOrb.Color;
				Texture2D textureTe = ModContent.Request<Texture2D>("SOTS/Assets/LongGradient").Value;
				Vector2 originT = new Vector2(0, textureTe.Height / 2);
				float alphaMult2 = Math.Clamp(MathF.Sin(MathF.Min(Projectile.ai[2] / 3f, 1) * MathF.PI), 0, 1);
                float length = Projectile.velocity.Length() * alphaMult2;
                Main.spriteBatch.Draw(textureTe, Projectile.Center - Main.screenPosition, null, c * alphaMult2 * 0.5f, Projectile.velocity.ToRotation(), originT, new Vector2(length / 6f, 1f + alphaMult2 * 0.5f), SpriteEffects.None, 0f);
            }
			if(Projectile.timeLeft < 480)
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2;
					float length = (drawPos - previous).Length();
					if(length < 300)
                    {
                        float trailMult = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                        Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, Projectile.GetAlpha(color) * trailMult * trailMult, Projectile.rotation, origin, new Vector2(length / 6f, 0.8f), SpriteEffects.None, 0.0f);
                        previous = drawPos;
                    }
				}
            if (outLine)
				for (int i = 0; i < 4; i++)
				{
					Vector2 circular = new Vector2(1f, 0).RotatedBy(i * MathHelper.PiOver2);
					Main.spriteBatch.Draw(texture, pos + circular - Main.screenPosition, null, Projectile.GetAlpha(color) * 0.5f * alphaMult * alphaMult, Projectile.rotation, origin, Projectile.scale * 0.8f, SpriteEffects.None, 0.0f);
				}
            Main.spriteBatch.Draw(texture, pos - Main.screenPosition, null, Projectile.GetAlpha(color) * alphaMult * alphaMult, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
		}
		public override void SetDefaults()
        {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.timeLeft = 480;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.ignoreWater = true;
		}
		private bool runOnce = true;
		public override void AI()
        {
            Color color = ExcavatorOrb.Color;
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
			if (Projectile.ai[1] == 0)
				Projectile.ai[1] = Projectile.velocity.Length();
			Projectile.ai[2] += 1 / 30f;
            Projectile.velocity = Projectile.velocity.SNormalize() * (Projectile.ai[1] + Projectile.ai[2]);
			if (Projectile.velocity.Length() > 1)
			{
				if (Main.rand.NextBool(3))
				{
					Dust dust = PixelDust.Spawn(Projectile.Center, 0, 0, Main.rand.NextVector2Circular(1, 1), color, 3);
					dust.velocity += Projectile.velocity * 0.1f;
					dust.scale = 1;
				}
			}
            Lighting.AddLight(Projectile.Center, color.ToVector3() * 0.5f);
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
        public override bool ShouldUpdatePosition()
        {
			return true;
        }
		public override void OnKill(int timeLeft)
		{
            Color color = ExcavatorOrb.Color;
			Vector2 prev = Projectile.Center;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				float percent = 1 - (float)k / Projectile.oldPos.Length;
                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2;
				for(float i = 0; i < 1; i += 0.34f)
				{
                    Dust dust = Dust.NewDustDirect(Vector2.Lerp(prev, drawPos, i) - new Vector2(4), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
                    dust.velocity *= 0.3f;
                    dust.velocity += Projectile.oldVelocity * Main.rand.NextFloat(1);
                    dust.noGravity = true;
                    dust.scale += 0.7f;
                    dust.scale *= 0.2f + 0.9f * percent;
                    dust.color = Color.Lerp(color, Color.Red, Main.rand.NextFloat() * Main.rand.NextFloat()) * percent;
                    dust.fadeIn = 0.1f;
                    dust.alpha = Projectile.alpha;
                }
				prev = drawPos;
            }
		}
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			VoidPlayer.VoidDamage(Mod, target, 5);
        }
    }
}