using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Buffs
{
    public class AbyssalInferno : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Inferno");
			Description.SetDefault("'No more skin!'");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            for(int i = 0; i < 24; i++)
            {
                if(Main.rand.NextBool(30))
                {
                    Vector2 fromPos = new Vector2(32, 0).RotatedBy(MathHelper.ToRadians(i * 15));
                    Vector2 velo = fromPos.SafeNormalize(Vector2.Zero) * -6f;
                    int dust3 = Dust.NewDust(player.Center + fromPos - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
                    Dust dust4 = Main.dust[dust3];
                    dust4.velocity *= 0.3f;
                    dust4.velocity += velo;
                    dust4.color = new Color(100, 255, 100, 0);
                    dust4.noGravity = true;
                    dust4.fadeIn = 0.1f;
                    dust4.scale *= 2.25f;
                }
            }
            base.Update(player, ref buffIndex);
        }
    }
}