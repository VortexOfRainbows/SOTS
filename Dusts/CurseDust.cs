using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Dusts
{
	public class CurseDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(1, 1 + Main.rand.Next(3) * 8, 8, 8);
			dust.velocity.Y = Main.rand.Next(-10, 11) * 0.2f;
			dust.velocity.X = Main.rand.Next(-10, 11) * 0.2f;
			dust.scale *= Main.rand.Next(5, 10) * 0.1f;
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
				Lighting.AddLight(dust.position, 0.5f * strength, 0.25f * strength, 0.5f * strength);
			}
			return true;
		}
	}
	public class CurseDust2 : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(1, 1 + Main.rand.Next(3) * 8, 8, 8);
			dust.velocity.Y = Main.rand.Next(-10, 11) * 0.2f;
			dust.velocity.X = Main.rand.Next(-10, 11) * 0.2f;
			dust.scale *= Main.rand.Next(9, 11) * 0.1f;
			dust.noGravity = true;
		}
		public override bool MidUpdate(Dust dust)
		{
			dust.scale *= 0.97f;
			dust.alpha += 3;
			if (dust.scale <= 0.1f || dust.alpha >= 255)
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
				Lighting.AddLight(dust.position, 0.5f * strength * ((245 - dust.alpha) / 255), 0.25f * strength * ((245 - dust.alpha) / 255), 0.5f * strength * ((245 - dust.alpha) / 255));
			}
			return true;
		}
	}
	public class CurseDust3 : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(1, 1 + Main.rand.Next(3) * 8, 8, 8);
			dust.velocity.Y = Main.rand.Next(-10, 11) * 0.2f;
			dust.velocity.X = Main.rand.Next(-10, 11) * 0.2f;
			dust.scale *= Main.rand.Next(9, 11) * 0.2f;
			dust.noGravity = false;
		}
		public override bool MidUpdate(Dust dust)
		{
			dust.scale *= 0.96f;
			dust.alpha += 4;
			if (dust.scale <= 0.1f || dust.alpha >= 255)
			{
				dust.active = false;
			}
			return true;
		}
	}
	public class ShortlivedCurseDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(1, 1 + Main.rand.Next(3) * 8, 8, 8);
			dust.velocity.Y = Main.rand.Next(-10, 11) * 0.2f;
			dust.velocity.X = Main.rand.Next(-10, 11) * 0.2f;
			dust.scale *= Main.rand.Next(9, 11) * 0.1f;
			dust.noGravity = true;
		}
		public override bool MidUpdate(Dust dust)
		{
			dust.scale *= 0.9f;
			dust.alpha += 5;
			if (dust.scale <= 0.1f || dust.alpha >= 255)
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
				Lighting.AddLight(dust.position, 0.5f * strength * ((245 - dust.alpha) / 255), 0.25f * strength * ((245 - dust.alpha) / 255), 0.5f * strength * ((245 - dust.alpha) / 255));
			}
			return true;
		}
	}
}