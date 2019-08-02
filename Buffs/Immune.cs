using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Immune : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Immune");
			Description.SetDefault("Damage is cancelled");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
		
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			player.immune = true;
			player.noFallDmg = true;
		}

    }
}