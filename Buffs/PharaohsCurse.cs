using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class PharaohsCurse : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Pharaoh's Curse");
			Description.SetDefault("Something is watching you, spawn rates increased");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
		{
			bool update = true;
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (SOTSWorld.downedBoss2)
				modPlayer.weakerCurse = true;
			if (SOTSWorld.downedCurse)
			{
				update = false;
			}
			if(update)
			{
				if(modPlayer.weakerCurse && player.statLife > 100)
				{
					player.lifeRegen -= 4;
				}
				if(modPlayer.weakerCurse && player.statLife > 200)
				{
					player.lifeRegen -= 2;
				}
				if(!modPlayer.weakerCurse)
				{
					player.lifeRegen -= 50;
				}
			}
			modPlayer.weakerCurse = false;
		}

    }
}