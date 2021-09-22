using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class TidalSpiritAid : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tidal Spirit Aid");
			Description.SetDefault("Tidal Spirits assist you in combat");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("TidalSpirit")] > 0) 
			{
				player.buffTime[buffIndex] = 6;
			}
		}
    }
}