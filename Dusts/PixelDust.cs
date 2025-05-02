using Microsoft.Xna.Framework;
using SOTS.Buffs;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Dusts
{
	public class PixelDust : ModDust
    {
		public static Dust Spawn(Vector2 pos, int w, int h, Vector2 velo, Color c, int decaySpeed = 5)
		{
			Dust d = Dust.NewDustDirect(pos - new Vector2(5, 5), w, h, ModContent.DustType<PixelDust>());
			d.velocity = velo;
			d.fadeIn = decaySpeed;
			d.color = c;
			return d;
		}
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, Main.rand.Next(3) * 10, 8, 8);
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			if(dust.fadeIn < 0)
            {
                return lightColor.MultiplyRGBA(dust.color * ((255 - dust.alpha) / 255f));
            }
			return dust.color * ((255 - dust.alpha) / 255f);
		}
		public override bool MidUpdate(Dust dust)
		{
			return true;
		}
		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			float abs = MathF.Abs(dust.fadeIn);
			dust.velocity *= 0.96f - abs * 0.01f;
			dust.rotation = 0;
			dust.alpha += (int)abs;
			Lighting.AddLight(dust.position, GetAlpha(dust, Color.Transparent).Value.ToVector3() * 0.2f);
			if (dust.scale <= 0.1f || dust.alpha >= 255)
			{
				dust.active = false;
			}
			return false;
		}
	}
}