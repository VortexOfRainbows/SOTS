using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Base;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Localization;

namespace SOTS.Items.Furniture
{
	public abstract class BlastDoorClosed : ModTile //<TDrop, TOpen> : ModTile where TDrop : ModItem where TOpen : ModTile
	{
		public virtual int DoorItemID => ModContent.ItemType<Nature.NaturePlatingBlastDoor>();
		public virtual int OpenDoorTile => ModContent.TileType<Nature.NaturePlatingBlastDoorTileOpen>();
		public virtual string GetName()
		{
			return Language.GetTextValue("Mods.SOTS.Common.BlastDoor");
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void SetStaticDefaults()
		{
			TileID.Sets.OpenDoorID[Type] = OpenDoorTile;
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileBlockLight[Type] = true;
			TileID.Sets.HousingWalls[Type] = true;
			TileID.Sets.NotReallySolid[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			// name.SetDefault(GetName());
			AddMapEntry(new Color(191, 142, 111), name);
			DustType = -1;
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.ClosedDoor };
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
		{

		}
		public void UpdateDoor(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int top = j - tile.TileFrameY / 18;
			if (Collision.EmptyTile(i, top, true) && Collision.EmptyTile(i, top + 1, true) && Collision.EmptyTile(i, top + 2, true)) //make sure no NPC or Player is in the tile
			{
				for (int k = 0; k < 3; k++)
				{
					Tile targetTile = Main.tile[i, top + k];
					targetTile.TileType = (ushort)OpenDoorTile;
					targetTile.TileFrameX *= 3;
				}
				NetMessage.SendTileSquare(-1, i, top + 1, 3, TileChangeType.None);
				Projectile.NewProjectile(new EntitySource_Misc("SOTS:BlastDoor"), new Vector2(i, top + 1) * 16, Vector2.Zero, ModContent.ProjectileType<BlastDoorProj>(), 0, 0, Main.myPlayer, 0);
			}
		}
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
			return true;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = DoorItemID;
		}
	}
	public abstract class BlastDoorOpen : ModTile //<TDrop, TClosed> : ModTile where TDrop : ModItem where TClosed : ModTile
	{
		public virtual int DoorItemID => ModContent.ItemType<Nature.NaturePlatingBlastDoor>();
		public virtual int ClosedDoorTile => ModContent.TileType<Nature.NaturePlatingBlastDoorTileClosed>();
		public virtual string GetName()
		{
			return Language.GetTextValue("Mods.SOTS.Common.BlastDoor");
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void SetStaticDefaults()
		{
			TileID.Sets.CloseDoorID[Type] = ClosedDoorTile;
			Main.tileSolid[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileBlockLight[Type] = false;
			TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			AddMapEntry(new Color(191, 142, 111), name);
			DustType = -1;
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.OpenDoor };
			TileID.Sets.DrawsWalls[Type] = true;
			TileID.Sets.HousingWalls[Type] = true;
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
        {

        }
		public void UpdateDoor(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int top = j - tile.TileFrameY / 18;
			if (Collision.EmptyTile(i, top, true) && Collision.EmptyTile(i, top + 1, true) && Collision.EmptyTile(i, top + 2, true)) //make sure no NPC or Player is in the tile
			{
				for (int k = 0; k < 3; k++)
				{
					Tile targetTile = Main.tile[i, top + k];
					targetTile.TileType = (ushort)ClosedDoorTile;
					targetTile.TileFrameX = (short)(targetTile.TileFrameX / 3);
				}
				NetMessage.SendTileSquare(-1, i, top + 1, 3, TileChangeType.None);
				Projectile.NewProjectile(new EntitySource_Misc("SOTS:BlastDoor"), new Vector2(i, top + 1) * 16, Vector2.Zero, ModContent.ProjectileType<BlastDoorProj>(), 0, 0, Main.myPlayer, 1);
			}
		}
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = DoorItemID;
		}
	}
}