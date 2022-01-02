using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class GoodVibes : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Good Vibes");
			Description.SetDefault("Increased stats while not moving");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			modPlayer.attackSpeedMod += 0.05f;
			if(Math.Abs(player.velocity.X) < 0.1f && Math.Abs(player.velocity.Y) < 0.1f)
			{	
				modPlayer.attackSpeedMod += 0.15f;
				player.lifeRegen += 4;
			}
		}
    }
}