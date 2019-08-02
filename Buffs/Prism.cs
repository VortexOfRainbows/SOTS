using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Prism : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Prism");
			Description.SetDefault("Refraction");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
            if (player.ownedProjectileCounts[mod.ProjectileType("Prism")] > 0)
            {
                modPlayer.Prism = true;
            }
            if (!modPlayer.Prism)
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