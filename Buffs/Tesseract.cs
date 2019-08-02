using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Tesseract : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Tesseract");
			Description.SetDefault("Help from another dimension");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
            if (player.ownedProjectileCounts[mod.ProjectileType("Tesseract")] > 0)
            {
                modPlayer.Tesseract = true;
            }
            if (!modPlayer.Tesseract)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}