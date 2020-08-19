using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class CyberneticEnhancements : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Cybernetic Enhancements");
			Description.SetDefault("Increased void damage by 10%");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
		
        }
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
            voidPlayer.voidDamage += 0.1f;
		}
    }
}