using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class DiamondSkin : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
		{
			player.endurance += 0.08f;
			player.moveSpeed += 0.08f;
			player.statDefense += 8;
		}
    }
}