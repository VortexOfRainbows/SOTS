using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Dusts
{
	public class BigEarthDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, Main.rand.Next(4) * 18, 18, 18);
			dust.velocity.Y = Main.rand.Next(-10, 11) * 0.2f;
			dust.velocity.X = Main.rand.Next(-10, 11) * 0.2f;
			dust.noGravity = true;
			dust.position.X -= 2;
			dust.position.Y += 2;
		}
		public override bool MidUpdate(Dust dust)
		{
			dust.scale -= 0.005f;
			dust.alpha += 3;
			if(dust.scale < 0.4f || dust.alpha > 255)
			{
				dust.active = false;
			}
			if (!dust.noLight)
			{
				float strength = dust.scale * 1.5f;
				if (strength > 1f)
				{
					strength = 1f;
				}
				Lighting.AddLight(dust.position, 0.69f * strength, 0.66f * strength, 0.45f * strength);
			}
			return true;
		}
	}
}