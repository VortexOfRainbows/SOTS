using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Items.Invidia;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;
using SOTS.Dusts;

namespace SOTS.Items.Invidia
{
	public class OvergrownEvostoneBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.rare = ItemRarityID.LightPurple;
			Item.createTile = ModContent.TileType<OvergrownEvostoneBrickTile>();
			Item.height += 2;
		}
	}
	public class OvergrownEvostoneBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<EvostoneBrickTile>();
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(112, 82, 122));
			MineResist = 1.5f;
            HitSound = SoundID.Tink;
			DustType = ModContent.DustType<InvidiaGrassDust>();
        }
		public override void RandomUpdate(int i, int j)
		{
			if (!Main.rand.NextBool(5))
				if (!Main.tile[i, j - 1].HasTile)
				{
					Tile tile = Main.tile[i, j - 1];
					WorldGen.PlaceTile(i, j - 1, ModContent.TileType<OvergrowthGrass>(), true, false, -1, Main.rand.Next(12));
					tile.TileColor = Main.tile[i, j].TileColor;
					NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
				}
				else if (Main.rand.NextBool(8))
					GrowCurseVine(i, j);
			base.RandomUpdate(i, j);
		}
		public static void GrowCurseVine(int i, int j)
		{
			if (!Main.tile[i, j + 1].HasTile && Main.tile[i, j + 1].LiquidAmount == 0)
			{
				var flag9 = false;
				for (var VineY = j; VineY > j - 10; VineY--)
				{
					if (Main.tile[i, VineY].BottomSlope)
					{
						flag9 = false;
						break;
					}
					if (Main.tile[i, VineY].HasTile && !Main.tile[i, VineY].BottomSlope)
					{
						flag9 = true;
						break;
					}
				}

				if (flag9)
				{
					var num47 = i;
					var num48 = j + 1;
					if (Main.tile[num47, num48].LiquidAmount == 0)
					{
						Tile tile = Main.tile[num47, num48];
						tile.TileType = (ushort)ModContent.TileType<OvergrowthVine>();
						tile.HasTile = true;
						tile.TileColor = Main.tile[i, j].TileColor;
						WorldGen.SquareTileFrame(num47, num48, true);
						if (Main.netMode == NetmodeID.Server)
						{
							NetMessage.SendTileSquare(-1, num47, num48, 3, TileChangeType.None);
						}
					}
				}
			}
		}
		public override bool CanExplode(int i, int j)
		{
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return true;
		}
	}
	public class OvergrowthGrass : ModTile
	{
		private Texture2D glow;
        public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(112, 82, 122));
			HitSound = SoundID.Grass;
			DustType = ModContent.DustType<InvidiaGrassDust>();
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.CoordinateHeights = [20];
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num--;
			base.NumDust(i, j, fail, ref num);
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (glow == null)
                glow = ModContent.Request<Texture2D>("SOTS/Items/Invidia/OvergrowthGrassGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            int frameX = Main.tile[i, j].TileFrameX / 18;
			if(frameX >= 5 || frameX <= 8)
			{
				for(int a = 0; a < 6; a ++)
				{
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, new Color(60, 50, 50, 0), new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(a * 60 + SOTSWorld.GlobalCounter)));
                }
				SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, Color.White, Vector2.Zero);
            }
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            int frameX = Main.tile[i, j].TileFrameX / 18;
            if (frameX >= 5 || frameX <= 8)
			{
				r = .5f;
				g = .2f;
				b = .25f;
			}
        }
    }
	public class OvergrowthVine : ModTile
	{
		private Texture2D glow;
		public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = false;
			Main.tileLavaDeath[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(112, 82, 122));
			HitSound = SoundID.Grass;
			DustType = ModContent.DustType<InvidiaGrassDust>();
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = [16];
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.AnchorAlternateTiles =
            [
                ModContent.TileType<OvergrownEvostoneBrickTile>(),
				ModContent.TileType<OvergrowthVine>(),
			];
			TileObjectData.addTile(Type);
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num--;
			base.NumDust(i, j, fail, ref num);
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			if (!Main.tile[i, j - 1].HasTile || !(Main.tile[i, j - 1].TileType == ModContent.TileType<OvergrowthVine>() || Main.tile[i, j - 1].TileType == ModContent.TileType<OvergrownEvostoneBrickTile>()))
				WorldGen.KillTile(i, j, false, false, false);
			return base.TileFrame(i, j, ref resetFrame, ref noBreak);
		}
		public override void RandomUpdate(int i, int j)
		{
			if (Main.rand.NextBool(5))
                OvergrownEvostoneBrickTile.GrowCurseVine(i, j);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (glow == null)
                glow = ModContent.Request<Texture2D>("SOTS/Items/Invidia/OvergrowthVineGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            {
                for (int a = 0; a < 6; a++)
                {
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, new Color(60, 50, 50, 0), new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(a * 60 + SOTSWorld.GlobalCounter)));
                }
				SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, Color.White, Vector2.Zero);
            }
        }
		public bool IsGlowingTile(int i, int j)
		{
            int frameX = Main.tile[i, j].TileFrameX / 18;
            int frameY = Main.tile[i, j].TileFrameY / 18;
			return (frameY == 0 && (frameX == 9 || frameX == 12)) ||
                (frameY == 1 && (frameX == 3 || frameX == 8)) ||
                (frameY == 2 && (frameX == 0 || frameX == 3 || frameX == 4 || frameX == 5) || frameX == 8 || frameX == 10 || frameX == 11) ||
                (frameY == 3 && (frameX == 8 || frameX == 11)) ||
                (frameY == 4 && (frameX == 4 || frameX == 5 || frameX == 8));
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = .5f;
            g = .2f;
            b = .25f;
        }
    }
}