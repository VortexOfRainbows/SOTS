using SOTS.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class PermafrostSpiritAid : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Permafrost Spirit Aid");
			Description.SetDefault("Permafrost Spirits assist you in combat");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<PermafrostSpirit>()] > 0) 
			{
				player.buffTime[buffIndex] = 6;
			}
		}
    }
}