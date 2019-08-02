using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Sacrifice : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sacrifice");
			Description.SetDefault("You were bound to die anyway");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
		
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			
			player.statLife = 1;
			
		}

    }
}