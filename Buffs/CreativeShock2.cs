using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class CreativeShock2 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Creative Shock");
			Description.SetDefault("You have lost the power of creation!");   
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