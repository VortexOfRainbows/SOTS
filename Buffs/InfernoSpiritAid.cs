using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class InfernoSpiritAid : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Spirit Aid");
			Description.SetDefault("Inferno Spirits assist you in combat");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Minions.InfernoSpirit>()] > 0) 
			{
				player.buffTime[buffIndex] = 6;
			}
		}
    }
}