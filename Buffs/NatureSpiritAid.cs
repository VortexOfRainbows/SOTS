using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class NatureSpiritAid : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Spirit Aid");
			Description.SetDefault("Nature Spirits assist you in combat");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("NatureSpirit")] > 0) 
			{
				player.buffTime[buffIndex] = 18000;
			}
			else 
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
    }
}