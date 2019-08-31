using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class PharaohsCurse : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Pharaoh's Curse");
			Description.SetDefault("Something is watching you");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
		
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			bool update = true;
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			if(SOTSWorld.downedCurse)
			{
				update = false;
			}
			if(update)
			{
				if(!NPC.AnyNPCs(mod.NPCType("DeadlyFragment")))
				{
					if(Main.rand.Next(1000) == 0)
					NPC.NewNPC((int)player.Center.X, (int)player.Center.Y + 1000, mod.NPCType("DeadlyFragment"));
				}
				else if(Main.rand.Next(9000) == 0)
				{
					NPC.NewNPC((int)player.Center.X, (int)player.Center.Y + 1000, mod.NPCType("DeadlyFragment"));
				}
				if(modPlayer.weakerCurse)
				{
					player.lifeRegen -= 5;
				}
				else
				{
					player.lifeRegen -= 50;
				}
			}
			modPlayer.weakerCurse = false;
		}

    }
}