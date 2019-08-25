using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.IceStuff
{
	public class FrostArtifactTile : ModTile
	{
		int rotatingDecor = 0;
		public override void SetDefaults()
		{
			minPick = 210; 
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 67;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Frost Artifact");		
			AddMapEntry(new Color(150, 240, 255), name);
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
		}
		public override void NearbyEffects(int i, int j, bool closer) 
		{
			if(closer && Main.tile[i, j + 1].type == mod.TileType("FrostArtifactTile") && Main.tile[i - 1, j + 1].type == mod.TileType("FrostArtifactTile") && Main.tile[i + 1, j + 1].type == mod.TileType("FrostArtifactTile"))
			{
				int xlocation = i * 16 + 8;
				int ylocation = j * 16 + 8;
				ylocation -= 40;
				rotatingDecor += 10;
					
				Vector2 circularLocation = new Vector2(-28, 0).RotatedBy(MathHelper.ToRadians(rotatingDecor));
				Vector2 circularLocation2 = new Vector2(28, 0).RotatedBy(MathHelper.ToRadians(rotatingDecor));
				Vector2 circularLocation3 = new Vector2(0, -28).RotatedBy(MathHelper.ToRadians(rotatingDecor));
				Vector2 circularLocation4 = new Vector2(0, 28).RotatedBy(MathHelper.ToRadians(rotatingDecor));
				
				int num1 = Dust.NewDust(new Vector2(xlocation + circularLocation.X - 4, ylocation + circularLocation.Y - 4), 4, 4, dustType);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				
				num1 = Dust.NewDust(new Vector2(xlocation + circularLocation2.X - 4, ylocation + circularLocation2.Y - 4), 4, 4, dustType);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				
				num1 = Dust.NewDust(new Vector2(xlocation + circularLocation3.X - 4, ylocation + circularLocation3.Y - 4), 4, 4, dustType);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				
				num1 = Dust.NewDust(new Vector2(xlocation + circularLocation4.X - 4, ylocation + circularLocation4.Y - 4), 4, 4, dustType);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
			
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if(frameX == 0)
			{
			   Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("FrostArtifact"));
			}
		}
		public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("FrostArtifactTile"))
			{
				return false;
			}
			return false;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return true;
			
		}
		public override void RightClick(int i, int j)
        {
			int xlocation = i * 16 - 8;
			int ylocation = j * 16 + 8;
            Player player = Main.LocalPlayer;
			for(int k = 0; k < 50; k++)
			{
				Item item = player.inventory[k];
				if(item.type == mod.ItemType("FrostedKey") && !NPC.AnyNPCs(mod.NPCType("ShardKing")))
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
					break;
				}
			}
		}  
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 0;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			
			player.showItemIcon2 = mod.ItemType("FrostedKey");
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