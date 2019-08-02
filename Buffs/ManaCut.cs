using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class ManaCut : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Mana Cut");
			Description.SetDefault("Life for mana");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			player.statManaMax2 -= modPlayer.megSetDamage;
			
                modPlayer.megSet = true;
				
            if (!modPlayer.megSet || modPlayer.megSetDamage <= 0)
            {
				
                modPlayer.megSetDamage = 0;
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}