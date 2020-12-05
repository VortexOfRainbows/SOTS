using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class AuraBoost : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Aura Boost");
			Description.SetDefault("Void regen increased by 4, life regen by 4, defense by 4, and reduces damage taken by 5%");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.4f;
			player.lifeRegen += 4;
			player.statDefense += 4;
			player.endurance += 0.05f;
		}
    }
}