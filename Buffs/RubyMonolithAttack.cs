using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class RubyMonolithAttack : ModBuff
    {
        public override void SetStaticDefaults()
        { 
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.flatVoidRegen -= 6.0f;
            player.GetDamage<VoidGeneric>() += 0.1f;
		}
    }
}