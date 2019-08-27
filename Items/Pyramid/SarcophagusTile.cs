using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid
{
	public class SarcophagusTile : ModTile
	{
		public override void SetDefaults()
		{
			minPick = 100; 
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 10;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Sarcophagus");		
			AddMapEntry(new Color(255, 215, 10), name);
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if(frameX == 0)
			{
			   Item.NewItem(i * 16, j * 16, 96, 48, mod.ItemType("Sarcophagus"));
			}
		}
		public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("SarcophagusTile"))
			{
				return false;
			}
			return false;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			if(SOTSWorld.downedCurse)
			return true;
		
			return false;
		}
		public override void RightClick(int i, int j)
        {
			int xlocation = i * 16 - 32;
			int ylocation = j * 16 + 8;
            Player player = Main.LocalPlayer;
			
				if(!NPC.AnyNPCs(mod.NPCType("ShardKing")))
				{
					NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("ShardKing"));
					
					for(int king = 0; king < 200; king++)
					{
						NPC npc = Main.npc[king];
						if(npc.type == mod.NPCType("ShardKing"))
						{
						npc.position.X = xlocation;
						npc.position.Y = ylocation - 1200;
						}
					}
				}
		}  
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			
			player.showItemIcon2 = mod.ItemType("Sarcophagus");
			//player.showItemIconText = "";
			player.noThrow = 2;
			player.showItemIcon = true;
		}
		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.showItemIconText == "")
			{
				player.showItemIcon = false;
				player.showItemIcon2 = 0;
			}
		}
	}
}