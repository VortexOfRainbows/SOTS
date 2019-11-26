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
			Description.SetDefault("Void regen increased by 5, life regen by 5, defense by 5, and reduces damage taken by 10%");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.5f;
			player.lifeRegen += 5;
			player.statDefense += 5;
			player.endurance += 0.1f;
		}
    }
}