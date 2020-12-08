using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Dusts
{
	public class AcedianDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, Main.rand.Next(5) * 10, 6, 8);
			dust.velocity.Y = Main.rand.Next(-10, 11) * 0.2f;
			dust.velocity.X = Main.rand.Next(-10, 11) * 0.2f;
			dust.scale *= Main.rand.Next(5, 10) * 0.15f;
		}
		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noLight)
			{
				float strength = dust.scale * 2.5f;
				if (strength > 1f)
				{
					strength = 1f;
				}
				Lighting.AddLight(dust.position, 1.8f * strength * 0.1f, 0.8f * strength * 0.1f, 1.7f * strength * 0.1f);
			}
			return true;
		}
	}
}