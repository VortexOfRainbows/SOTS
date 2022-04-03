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


namespace SOTS.Items.Furniture.Nature
{
	public class NaturePlatingBlastDoor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Plating Blast Door");
			Tooltip.SetDefault("Cannot be opened by NPCs");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Blue;
			item.width = 16;
			item.height = 32;
			item.createTile = ModContent.TileType<NaturePlatingBlastDoorTileClosed>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NaturePlating>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class NaturePlatingBlastDoorTileClosed : ModTile
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = ModContent.GetTexture(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
		public override void SetDefaults()
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
			name.SetDefault("Nature Plating Blast Door");
			AddMapEntry(new Color(191, 142, 111), name);
			dustType = -1;
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.ClosedDoor };
		}
        public override bool NewRightClick(int i, int j)
		{
			UpdateDoor(i, j, true);
			return true;
        }
        public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int top = j - tile.frameY / 18;
			Wiring.SkipWire(i, top);
			Wiring.SkipWire(i, top + 1);
			Wiring.SkipWire(i, top + 2);
			UpdateDoor(i, j, false);
			base.HitWire(i, j);
        }
		public void UpdateDoor(int i, int j, bool client)
		{
			Tile tile = Main.tile[i, j];
			int top = j - tile.frameY / 18;
			if (Collision.EmptyTile(i, top, true) && Collision.EmptyTile(i, top + 1, true) && Collision.EmptyTile(i, top + 2, true))
			{
				for (int k = 0; k < 3; k++)
				{
					Tile targetTile = Main.tile[i, top + k];
					targetTile.type = (ushort)ModContent.TileType<NaturePlatingBlastDoorTileOpen>();
					targetTile.frameX *= 3;
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
			Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<NaturePlatingBlastDoor>());
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ModContent.ItemType<NaturePlatingBlastDoor>();
		}
	}
	public class NaturePlatingBlastDoorTileOpen : ModTile
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = ModContent.GetTexture(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
		public override void SetDefaults()
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
			name.SetDefault("Nature Plating Blast Door");
			AddMapEntry(new Color(191, 142, 111), name);
			dustType = -1;
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.OpenDoor };
			TileID.Sets.DrawsWalls[Type] = true;
			TileID.Sets.HousingWalls[Type] = true;
			closeDoorID = ModContent.TileType<NaturePlatingBlastDoorTileClosed>();
		}
		public override bool NewRightClick(int i, int j)
		{
			UpdateDoor(i, j, true);
			return true;
		}
		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int top = j - tile.frameY / 18;
			Wiring.SkipWire(i, top);
			Wiring.SkipWire(i, top + 1);
			Wiring.SkipWire(i, top + 2);
			UpdateDoor(i, j, false);
			base.HitWire(i, j);
		}
		public void UpdateDoor(int i, int j, bool client)
		{
			Tile tile = Main.tile[i, j];
			int top = j - tile.frameY / 18;
			if (Collision.EmptyTile(i, top, true) && Collision.EmptyTile(i, top + 1, true) && Collision.EmptyTile(i, top + 2, true))
			{
				for (int k = 0; k < 3; k++)
				{
					Tile targetTile = Main.tile[i, top + k];
					targetTile.type = (ushort)ModContent.TileType<NaturePlatingBlastDoorTileClosed>();
					targetTile.frameX /= 3;
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
			Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<NaturePlatingBlastDoor>());
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ModContent.ItemType<NaturePlatingBlastDoor>();
		}
	}
}