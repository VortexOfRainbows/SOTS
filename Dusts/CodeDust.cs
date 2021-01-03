using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Dusts
{
	public class CodeDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, Main.rand.Next(2) * 10, 10, 10);
			dust.velocity.Y = Main.rand.Next(-10, 11) * 0.2f;
			dust.velocity.X = Main.rand.Next(-10, 11) * 0.2f;
			dust.scale *= Main.rand.Next(9, 11) * 0.1f;
			dust.noGravity = true;
		}
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color(200, 255, 200, dust.alpha);
		}
        public override bool MidUpdate(Dust dust)
		{
			return true;
        }
        public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.velocity *= 0.94f;
			dust.rotation = 0;
			dust.scale *= 0.95f;
			dust.scale -= 0.0125f;
			dust.alpha += 5;
			float strength = 1f;
			Lighting.AddLight(dust.position, 0.5f * strength * ((255 - dust.alpha) / 255f), 0.75f * strength * ((255 - dust.alpha) / 255f), 0.45f * strength * ((255 - dust.alpha) / 255f));
			if (dust.scale <= 0.1f || dust.alpha >= 255)
			{
				dust.active = false;
			}
			return false;
		}
    }
}