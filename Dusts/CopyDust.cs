using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Dusts
{
	public class CopyDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, Main.rand.Next(3) * 10, 8, 8);
        }
        public override void SetDefaults()
        {
            updateType = 267;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            int al = 0;
            for(int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                Rectangle rect = new Rectangle((int)player.position.X - 2 + player.direction, (int)player.position.Y + 4, player.width + 4, player.height);
                if(rect.Contains((int)dust.position.X, (int)dust.position.Y))
                {
                    return new Color(0, 0, 0, 0);
                }
            }
            return new Color(dust.color.R, dust.color.G, dust.color.B, al) * (1f - (dust.alpha / 255f));
        }
    }
}