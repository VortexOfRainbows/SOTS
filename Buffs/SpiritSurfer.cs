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
            player.mount.SetMount(ModContent.MountType<Mounts.SpiritSurfer>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
    public class UnholyGrailBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mounts.UnholyGrailMount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}