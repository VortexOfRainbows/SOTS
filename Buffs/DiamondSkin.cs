using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class DiamondSkin : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Diamond Skin");
			Description.SetDefault("15 increased defense, 15% reduced damage taken, and 15% increased movement speed");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
		
        }
		public override void Update(Player player, ref int buffIndex)
		{
			player.endurance += 0.15f;
			player.moveSpeed += 0.15f;
			player.statDefense += 15;
		}
    }
}