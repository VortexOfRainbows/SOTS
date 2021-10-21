using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class RubyMonolithDefense : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ruby Monolith Defense");
			Description.SetDefault("Increases void regen by 4\nReduces damage taken by 5%");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.4f;
            player.endurance += 0.05f;
		}
    }
}