using Microsoft.Xna.Framework.Input;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class DoubleVision : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer.ModPlayer(player).DoubleVisionActive = true;
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            tip = Language.GetTextValue("Mods.SOTS.Buffs.DoubleVision.Description", SOTSPlayer.ModPlayer(Main.LocalPlayer).BonusFishingLines);
        }
    }
}