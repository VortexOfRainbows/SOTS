using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class PoweredUp : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Unknown Enigma");
			Description.SetDefault("Energy flows through your veins");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
		
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			player.statLife += player.statLifeMax2 - player.statLife;
			player.meleeDamage += 4f;
			player.thrownDamage += 4f;
			player.magicDamage += 4f;
			player.rangedDamage += 4f;
			player.manaCost -= 1;
		}

    }
}