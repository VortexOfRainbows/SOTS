using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class SpiritSurfer : ModBuff
    {	
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Spirit Surfer");
			Description.SetDefault("Surf across the interdimensional plane!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("SpiritSurfer"), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}