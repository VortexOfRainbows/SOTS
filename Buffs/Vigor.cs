using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Vigor : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer.ModPlayer(player).VigorActive = true;
            player.buffTime[buffIndex] = 3600;
            if(SOTSPlayer.ModPlayer(player).VigorDashes <= 0)
            {
                player.ClearBuff(Type);
            }
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            tip = Language.GetTextValue("Mods.SOTS.Buffs.Vigor.Description", SOTSPlayer.ModPlayer(Main.LocalPlayer).VigorDashes);
        }
    }
}