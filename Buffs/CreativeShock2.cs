using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class CreativeShock2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
		{
            player.noBuilding = true;
		}
    }
}