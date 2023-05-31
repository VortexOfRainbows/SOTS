using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Dusts
{
	public class PixelDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, Main.rand.Next(3) * 10, 8, 8);
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return dust.color * ((255 - dust.alpha) / 255f);
		}
		public override bool MidUpdate(Dust dust)
		{
			return true;
		}
		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.velocity *= 0.96f - dust.fadeIn * 0.01f;
			dust.rotation = 0;
			dust.alpha += (int)dust.fadeIn;
			Lighting.AddLight(dust.position, GetAlpha(dust, Color.Transparent).Value.ToVector3() * 0.2f);
			if (dust.scale <= 0.1f || dust.alpha >= 255)
			{
				dust.active = false;
			}
			return false;
		}
	}
}