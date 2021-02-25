using Terraria;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Buffs
{
    public class Frenzy : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Frenzy");
			Description.SetDefault("Melee Speed increased by 100%");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
		{
            if (player.HeldItem.melee)
                SOTSPlayer.ModPlayer(player).attackSpeedMod += 1;
		}
    }
}