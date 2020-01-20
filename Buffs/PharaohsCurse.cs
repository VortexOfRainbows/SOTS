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
			Description.SetDefault("Something is watching you, spawn rates increased");   
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
					if(Main.rand.Next(2000) == 0)
					{
						if(Main.netMode != 1)
						{
							int npc1 = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y + 1000, mod.NPCType("DeadlyFragment"));
							Main.npc[npc1].netUpdate = true;
						}
					}
				}
				else if(Main.rand.Next(12000) == 0)
				{
					if(Main.netMode != 1)
					{
						int npc1 = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y + 1000, mod.NPCType("DeadlyFragment"));
						Main.npc[npc1].netUpdate = true;
					}
				}
				if(modPlayer.weakerCurse && player.statLife > 100)
				{
					player.lifeRegen -= 4;
				}
				else if(!modPlayer.weakerCurse)
				{
					player.lifeRegen -= 50;
				}
			}
			modPlayer.weakerCurse = false;
		}

    }
}