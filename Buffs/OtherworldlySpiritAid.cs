using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class OtherworldlySpiritAid : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Spirit Aid");
			Description.SetDefault("Otherworldly Spirits assist you in combat");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("OtherworldlySpirit")] > 0) 
			{
				player.buffTime[buffIndex] = 6;
			}
		}
    }
}