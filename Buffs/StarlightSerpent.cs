using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class StarlightSerpent : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Starlight Serpent");
			Description.SetDefault("A prismatic serpent assists with combat");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("StarlightSerpentHead")] > 0) 
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