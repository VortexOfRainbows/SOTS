using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Roughskin : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Roughskin");
			Description.SetDefault("4 increased defense and 4% increased damage");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
		
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			player.allDamage += 0.04f;
			player.statDefense += 4;
		}

    }
}