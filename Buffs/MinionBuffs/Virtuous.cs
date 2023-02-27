using SOTS.Projectiles.Inferno;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.MinionBuffs
{
    public class Virtuous : ModBuff
    {
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SpectralWisp>()] > 0) 
			{
				player.buffTime[buffIndex] = 6;
			}
		}
    }
}