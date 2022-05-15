using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Dusts
{
	public class CopyDust4 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, Main.rand.Next(3) * 10, 8, 8);
        }
        public override void SetStaticDefaults()
        {
            updateType = 267;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            int al = 0;
            return new Color(dust.color.R, dust.color.G, dust.color.B, al) * (1f - (dust.alpha / 255f));
        }
    }
}