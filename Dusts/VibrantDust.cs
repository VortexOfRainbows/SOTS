using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Dusts
{
	public class VibrantDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(1, 1 + Main.rand.Next(3) * 10, 8, 8);
			dust.velocity.Y = Main.rand.Next(-10, 11) * 0.2f;
			dust.velocity.X = Main.rand.Next(-10, 11) * 0.2f;
			dust.scale *= Main.rand.Next(8, 12) * 0.1f;
		}
        public override bool Update(Dust dust)
        {
			Lighting.AddLight(dust.position, new Vector3(0.135f, 0.165f, 0.075f));
            return true;
        }
    }
}