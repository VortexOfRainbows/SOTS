using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class SpiritSurfer : ModBuff
    {	
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(Mod.Find<ModMount>("SpiritSurfer").Type, player);
            player.buffTime[buffIndex] = 10;
        }
    }
}