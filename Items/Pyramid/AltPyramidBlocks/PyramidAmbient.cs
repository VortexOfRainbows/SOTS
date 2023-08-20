using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid.AltPyramidBlocks
{
	public class PyramidAmbient : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 0;
			Item.consumable = true;
		}
		int type = 0;
        public override bool? UseItem(Player player)
        {
			int modU = type % 7;
			if(modU == 0)
			{
				Item.createTile = ModContent.TileType<PyramidAmbientTile1x1>();
			}
			else if (modU == 1)
			{
				Item.createTile = ModContent.TileType<PyramidAmbientTile1x1Curse>();
			}
			else if (modU == 2)
			{
				Item.createTile = ModContent.TileType<PyramidAmbientTile2x1Curse>();
			}
			else if (modU == 3)
			{
				Item.createTile = ModContent.TileType<PyramidAmbientTile2x1Curse>();
			}
			else if (modU == 4)
			{
				Item.createTile = ModContent.TileType<PyramidAmbientTile2x2>();
			}
			else if (modU == 5)
			{
				Item.createTile = ModContent.TileType<PyramidAmbientTile3x1Curse>();
			}
			else if (modU == 6)
			{
				Item.createTile = ModContent.TileType<PyramidAmbientTile3x2Curse>();
			}
			type++;
			return base.UseItem(player);
        }
    }	
	public class PyramidAmbientTile1x1 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileNoFail[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 18 };
			//TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.RandomStyleRange = 2;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(156, 137, 78));
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = 32;
			HitSound = SoundID.Dig;
			MineResist = 0.1f;
		}
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
			num = 3;
        }
	}
	public class PyramidAmbientTile1x1Curse : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileNoFail[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 18 };
			//TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.StyleWrapLimit = 3;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(78, 55, 108));
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = ModContent.DustType<CurseDust3>();
			HitSound = SoundID.NPCHit1;
			MineResist = 0.1f;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 3;
		}
	}
	public class PyramidAmbientTile2x1 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileNoFail[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 18 };
			//TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.StyleWrapLimit = 3;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(156, 137, 78));
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = 32;
			HitSound = SoundID.Dig;
			MineResist = 0.1f;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 3;
		}
	}
	public class PyramidAmbientTile2x1Curse : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileNoFail[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 18 };
			//TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.StyleWrapLimit = 3;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(78, 55, 108));
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = ModContent.DustType<CurseDust3>();
			HitSound = SoundID.NPCHit1;
			MineResist = 0.1f;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 3;
		}
	}
	public class PyramidAmbientTile3x1Curse : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileNoFail[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 18 };
			//TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleWrapLimit = 1;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(78, 55, 108));
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = ModContent.DustType<CurseDust3>();
			HitSound = SoundID.NPCHit1;
			MineResist = 0.1f;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 3;
		}
	}
	public class PyramidAmbientTile2x2 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileNoFail[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			//TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleWrapLimit = 1;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(156, 137, 78));
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = 32;
			HitSound = SoundID.Dig;
			MineResist = 0.1f;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 3;
		}
	}
	public class PyramidAmbientTile3x2Curse : ModTile
	{
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX != 54 && tile.TileFrameX != 108)
				return false;
			if (tile.TileFrameY != 0)
				return false;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 drawOffSet = Vector2.Zero;
			drawOffSet.Y += 2;
			Vector2 location = new Vector2(i * 16, j * 16) + drawOffSet;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Pyramid/AltPyramidBlocks/PyramidAmbientTile3x2CurseGlow").Value;
			float counter = Main.GlobalTimeWrappedHourly * 160;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			Rectangle frame = new Rectangle(tile.TileFrameX / 18 * 16, tile.TileFrameY / 18 * 16, 48, 32);
			for (int k = 0; k < 6; k++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (k)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 40, 0, 0);
						break;
					case 2:
						color = new Color(255, 80, 0, 0);
						break;
					case 3:
						color = new Color(255, 120, 0, 0);
						break;
					case 4:
						color = new Color(255, 160, 0, 0);
						break;
					case 5:
						color = new Color(255, 200, 0, 0);
						break;
				}
				Vector2 rotationAround2 = new Vector2(2 + mult, 0).RotatedBy(MathHelper.ToRadians(60 * k + counter));
				Main.spriteBatch.Draw(texture2, location + zero - Main.screenPosition + rotationAround2, frame, color, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
			}
			return false;
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Texture2D texture = Terraria.GameContent.TextureAssets.Tile[tile.TileType].Value;
			Color color2 = Lighting.GetColor(i, j, WorldGen.paintColor(tile.TileColor));
			Vector2 drawOffSet = Vector2.Zero;
			drawOffSet.Y += 2;
			Vector2 location = new Vector2(i * 16, j * 16) + drawOffSet;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			spriteBatch.Draw(texture, location + zero - Main.screenPosition, frame, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
        public override void SetStaticDefaults()
		{
			Main.tileNoFail[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.StyleWrapLimit = 3;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(78, 55, 108));
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = ModContent.DustType<CurseDust3>();
			HitSound = SoundID.NPCHit1;
			MineResist = 0.1f;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 3;
		}
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX == 54)
				yield return new Item(ModContent.ItemType<RoyalRubyShard>(), 3);
            if (tile.TileFrameX == 108)
                yield return new Item(ModContent.ItemType<RoyalRubyShard>(), 4);
        }
        public override bool KillSound(int i, int j, bool fail)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX >= 108)
			{
				Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
				SOTSUtils.PlaySound(SoundID.Item27, (int)pos.X, (int)pos.Y, 0.9f, 0.1f);
				return true;
			}
			if (tile.TileFrameX >= 54)
			{
				Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
				SOTSUtils.PlaySound(SoundID.Item27, (int)pos.X, (int)pos.Y, 0.9f, 0.1f);
				return true;
			}
			return true;
        }
    }
}