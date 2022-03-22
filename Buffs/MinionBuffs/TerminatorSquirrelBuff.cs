using SOTS.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.MinionBuffs
{
    public class TerminatorSquirrelBuff : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mechanical Squirrel");
			Description.SetDefault("Squirrels assist you in combat");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<TerminatorSquirrel>()] > 0) 
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