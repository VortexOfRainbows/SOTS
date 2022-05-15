using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Base;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture
{
	public abstract class BlastDoorClosed<TDrop, TOpen> : ModTile where TDrop : ModItem where TOpen : ModTile
	{
		public virtual string GetName()
		{
			return "Blast Door";
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void SetStaticDefaults()
		{
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
			ModTranslation name = CreateMapEntryName();
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			name.SetDefault(GetName());
			AddMapEntry(new Color(191, 142, 111), name);
			DustType = -1;
			disableSmartCursor = true;
			AdjTiles = new int[] { TileID.ClosedDoor };
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
		{

		}
		public override bool RightClick(int i, int j)
		{
			UpdateDoor(i, j, true);
			return true;
        }
        public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int top = j - tile.TileFrameY / 18;
			Wiring.SkipWire(i, top);
			Wiring.SkipWire(i, top + 1);
			Wiring.SkipWire(i, top + 2);
			UpdateDoor(i, j, false);
			base.HitWire(i, j);
        }
		public void UpdateDoor(int i, int j, bool client)
		{
			Tile tile = Main.tile[i, j];
			int top = j - tile.TileFrameY / 18;
			if (Collision.EmptyTile(i, top, true) && Collision.EmptyTile(i, top + 1, true) && Collision.EmptyTile(i, top + 2, true))
			{
				for (int k = 0; k < 3; k++)
				{
					Tile targetTile = Main.tile[i, top + k];
					targetTile.TileType = (ushort)ModContent.TileType<TOpen>();
					targettile.TileFrameX *= 3;
				}
				NetMessage.SendTileSquare(client ? Main.myPlayer : -1, i, top + 1, 3, TileChangeType.None);
				Projectile.NewProjectile(new Vector2(i, top + 1) * 16, Vector2.Zero, ModContent.ProjectileType<BlastDoorProj>(), 0, 0, Main.myPlayer, 0);
			}
		}
        public override bool HasSmartInteract()
		{
			return true;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<TDrop>());
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<TDrop>();
		}
	}
	public abstract class BlastDoorOpen<TDrop, TClosed> : ModTile where TDrop : ModItem where TClosed : ModTile
	{
		public virtual string GetName()
		{
			return "Blast Door";
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void SetStaticDefaults()
		{
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
			ModTranslation name = CreateMapEntryName();
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			name.SetDefault(GetName());
			AddMapEntry(new Color(191, 142, 111), name);
			DustType = -1;
			disableSmartCursor = true;
			AdjTiles = new int[] { TileID.OpenDoor };
			TileID.Sets.DrawsWalls[Type] = true;
			TileID.Sets.HousingWalls[Type] = true;
			closeDoorID = ModContent.TileType<TClosed>();
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
        {

        }
		public override bool RightClick(int i, int j)
		{
			UpdateDoor(i, j, true);
			return true;
		}
		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int top = j - tile.TileFrameY / 18;
			Wiring.SkipWire(i, top);
			Wiring.SkipWire(i, top + 1);
			Wiring.SkipWire(i, top + 2);
			UpdateDoor(i, j, false);
			base.HitWire(i, j);
		}
		public void UpdateDoor(int i, int j, bool client)
		{
			Tile tile = Main.tile[i, j];
			int top = j - tile.TileFrameY / 18;
			if (Collision.EmptyTile(i, top, true) && Collision.EmptyTile(i, top + 1, true) && Collision.EmptyTile(i, top + 2, true))
			{
				for (int k = 0; k < 3; k++)
				{
					Tile targetTile = Main.tile[i, top + k];
					targetTile.TileType = (ushort)ModContent.TileType<TClosed>();
					targettile.TileFrameX /= 3;
				}
				NetMessage.SendTileSquare(client ? Main.myPlayer : -1, i, top + 1, 3, TileChangeType.None);
				Projectile.NewProjectile(new Vector2(i, top + 1) * 16, Vector2.Zero, ModContent.ProjectileType<BlastDoorProj>(), 0, 0, Main.myPlayer, 1);
			}
		}
		public override bool HasSmartInteract()
		{
			return true;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<TDrop>());
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<TDrop>();
		}
	}
}