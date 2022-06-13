using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Roughskin : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Roughskin");
			Description.SetDefault("4 increased defense and 4% increased damage");   
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
		
        }
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetDamage(DamageClass.Generic) += 0.04f;
			player.statDefense += 4;
		}
    }
}