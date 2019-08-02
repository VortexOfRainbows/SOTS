using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Siphoned : ModBuff
    {	int timeleft = 0;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Soul Siphoned");
			Description.SetDefault("Your soul is cut in two\nEnables spectator mode\nDisable by disabling the buff and rejoining the game");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = false;
        }
 
        public override void Update(Player player, ref int buffIndex)
		{



		player.ghost = true;
        }
    }
}