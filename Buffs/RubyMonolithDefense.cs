using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class RubyMonolithDefense : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ruby Monolith Defense");
			Description.SetDefault("Increases void regeneration speed by 10%\nReduces damage taken by 5%");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegenSpeed += 0.1f;
            player.endurance += 0.05f;
		}
    }
}