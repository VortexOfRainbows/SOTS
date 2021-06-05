using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Virtuous : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Virtuous");
			Description.SetDefault("'Spiritual Companionship'");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("SpectralWisp")] > 0) 
			{
				player.buffTime[buffIndex] = 6;
			}
		}
    }
}