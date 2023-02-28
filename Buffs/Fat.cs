using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Fat : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
		
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			player.statLifeMax2 += 30;
			player.endurance += 0.17f;
			player.moveSpeed = 0.35f;
			player.statDefense += 9;
		}

    }
}