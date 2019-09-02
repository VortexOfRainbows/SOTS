using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class CursedBlade : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Cursed Blade");
			Description.SetDefault("");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
			
			player.buffTime[buffIndex] = 18000;
			bool projectileNotSpawned = true;
			if (player.ownedProjectileCounts[mod.ProjectileType("CursedBlade")] > 0)
			{
				projectileNotSpawned = false;
			}
			if (projectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
                player.DelBuff(buffIndex);
                buffIndex--;
			}
        }
    }
}