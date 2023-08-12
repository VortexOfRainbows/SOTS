using Microsoft.Xna.Framework;
using SOTS.NPCs.Boss.Curse;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid
{
	public class SarcophagusTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			MinPick = 250; 
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = 10;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(255, 215, 10), name);
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if(frameX == 0)
			{
			   Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 96, 48, ModContent.ItemType<Sarcophagus>());
			}
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			if(SOTSWorld.downedCurse)
				return true;
			return false;
		}
		public override bool RightClick(int i, int j)
		{
			int left = i - (Main.tile[i, j].TileFrameX / 18);
			int top = j - (Main.tile[i, j].TileFrameY / 18);
			Main.mouseRightRelease = true;
            Player player = Main.LocalPlayer;
			if(!NPC.AnyNPCs(ModContent.NPCType<PharaohsCurse>()))
			{
				Projectile.NewProjectile(player.GetSource_TileInteraction(i, j), new Vector2(left * 16, top * 16) + new Vector2(48, 16), Vector2.Zero, ModContent.ProjectileType<ReleaseWallMimic>(), 0, 0, Main.myPlayer, -1);
				return true;
			}
			return true;
		}  
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.cursorItemIconID = ModContent.ItemType<Sarcophagus>();
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}
		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}
	}
}