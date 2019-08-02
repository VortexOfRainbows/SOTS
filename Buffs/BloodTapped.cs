using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class BloodTapped : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blood Tapped");
			Description.SetDefault("Your blood has been seperated from your soul, Your max health has been lowered");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
		
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			
			if(Main.expertMode)
			{
				
			player.statLifeMax2 = 25;
			}
			else{
				
			player.statLifeMax2 = 70;
			
			}
		}

    }
}