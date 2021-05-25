using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Dusts
{
	public class CurseDust2 : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(2, Main.rand.Next(3) * 12, 10, 12);
			dust.velocity.Y = Main.rand.Next(-10, 11) * 0.2f;
			dust.velocity.X = Main.rand.Next(-10, 11) * 0.2f;
			dust.scale *= Main.rand.Next(9, 11) * 0.1f;
			dust.noGravity = true;
		}
		public override bool MidUpdate(Dust dust)
		{
			dust.scale *= 0.97f;
			dust.alpha += 3;
			if(dust.scale <= 0.1f || dust.alpha >= 255)
			{
				dust.active = false;
			}
			if (!dust.noLight)
			{
				float strength = dust.scale * 2.5f;
				if (strength > 1f)
				{
					strength = 1f;
				}
				Lighting.AddLight(dust.position, 0.5f * strength * ((245 - dust.alpha) /255), 0.25f * strength * ((245 - dust.alpha) / 255), 0.5f * strength * ((245 - dust.alpha) / 255));
			}
			return true;
		}
	}
}