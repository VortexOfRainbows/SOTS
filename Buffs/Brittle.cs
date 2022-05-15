using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Brittle : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brittle");
			Description.SetDefault("Getting hit surrounds you with ice shards");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            modPlayer.bonusShardDamage += 10;
            modPlayer.shardOnHit += 1;
        }
    }
}