using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class IceShield : ModBuff
    {	
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Cryogen");
			Description.SetDefault("A holy levitating ice shield surrounds you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("IceShield"), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}