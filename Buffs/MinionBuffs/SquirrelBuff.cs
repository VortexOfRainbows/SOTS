using SOTS.Projectiles.Nature;
using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.MinionBuffs
{
    public class SquirrelBuff : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Squirrel");
			Description.SetDefault("Squirrels assist you in combat");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SquirrelMinion>()] > 0) 
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