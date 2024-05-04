using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Buffs
{
    public class Assassination : ModBuff
    {
        public override void SetStaticDefaults()
        { 
            Main.buffNoTimeDisplay[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            modPlayer.assassinateNum *= 0.9f;
            modPlayer.assassinate = true;
            modPlayer.assassinateFlat += 20;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            for(int i = 0; i < 3; i++)
            {
                Vector2 rotate = new Vector2(npc.width / 2 + 4, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 120 + npc.whoAmI * 7 + 120 * i));
                int num1 = Dust.NewDust(new Vector2(npc.Center.X + rotate.X - 4, npc.Center.Y + rotate.Y - 4), 0, 0, DustID.LifeDrain);
                Main.dust[num1].noGravity = true;
                Main.dust[num1].velocity *= 0.1f;
            }
        }
    }
}