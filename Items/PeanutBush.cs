using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.Slime;
using SOTS.WorldgenHelpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items
{
	public class PeanutBush : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(34, 30);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<PeanutBushTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Acorn, 1).AddIngredient<Peanut>(10).AddIngredient<FragmentOfNature>(10).Register();
		}
	}
	public class PeanutBushTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			MineResist = 0.01f;
			Main.tileNoFail[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleDye);
			TileObjectData.newTile.CoordinateWidth = 52;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.CoordinateHeights = new int[1] { 40 };
			TileObjectData.newTile.DrawYOffset = -22;
			TileObjectData.newTile.DrawFlipHorizontal = false;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.addTile(Type);
			DustType = DustID.WoodFurniture;
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(113, 173, 37), name);
			ItemDrop = ModContent.ItemType<PeanutBush>();
		}
        public override bool CanPlace(int i, int j)
        {
			for(int i2 = i - 1; i2 <= i + 1; i2++)
			{
				for (int j2 = j - 1; j2 <= j + 1; j2++)
				{
					Tile tile = Framing.GetTileSafely(i2, j2);
					if(tile.TileType == Type)
                    {
						return false;
                    }
				}
			}
            return true;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
			if (Main.rand.NextBool(3))
				type = DustID.Grass;
            return base.CreateDust(i, j, ref type);
        }
	}
}