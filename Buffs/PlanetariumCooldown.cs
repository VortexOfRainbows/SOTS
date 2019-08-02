using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class PlanetariumCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Planetarium Cooldown");
			Description.SetDefault("On cooldown");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
		
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			
		}

    }
}