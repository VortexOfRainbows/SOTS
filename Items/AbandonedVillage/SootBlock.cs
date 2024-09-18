using Microsoft.Xna.Framework;
using SOTS.Common.GlobalTiles;
using SOTS.Dusts;
using SOTS.Items.Earth;
using SOTS.Items.Tools;
using System.Security.Permissions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static SOTS.Items.AbandonedVillage.CorruptionSoot;
using static SOTS.Items.AbandonedVillage.CrimsonSoot;

namespace SOTS.Items.AbandonedVillage
{
	public class SootBlockTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			DustType = ModContent.DustType<SootDust>(); 
			AddMapEntry(new Color(57, 50, 44));
		}
	}
	public class SootBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ExtractinatorMode[Type] = Type;
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<SootBlockTile>();
		}
        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
			if(resultType == ItemID.CopperCoin || resultType == ItemID.SilverCoin || resultType == ItemID.GoldCoin || resultType == ItemID.PlatinumCoin
				|| resultType == ItemID.CopperOre || resultType == ItemID.TinOre || resultType == ItemID.IronOre || resultType == ItemID.LeadOre || resultType == ItemID.SilverOre || resultType == ItemID.TungstenOre || resultType == ItemID.AmberMosquito)
			{
				return;
            }
			else
            {
				resultStack = 1;
				if (Main.rand.NextBool(7))
				{
					resultType = ModContent.ItemType<OldKey>();
				}
				else if(Main.rand.NextBool(6))
				{
					resultType = ModContent.ItemType<MinersPickaxe>();
					resultStack = Main.rand.Next(3) + 1;
				}
				else if(Main.rand.NextBool(5))
				{
					resultType = ItemID.MusketBall;
					resultStack = 10 + Main.rand.Next(15) + Main.rand.Next(15) + Main.rand.Next(15) + Main.rand.Next(15) + Main.rand.Next(15);
				}
				else if(Main.rand.NextBool(4))
				{
					resultType = ItemID.Vertebrae;
					if (Main.rand.NextBool(2))
						resultType = ItemID.RottenChunk;
					resultStack = 1 + Main.rand.Next(2);
					if(Main.rand.NextBool(4))
                    {
						resultStack += 2 + Main.rand.Next(2);
                    }
				}
				else
				{
					resultType = ModContent.ItemType<CharredWood>();
					resultStack = 5 + Main.rand.Next(11);
					if (Main.rand.NextBool(6))
					{
						resultStack += 2 + Main.rand.Next(8);
					}
					if (Main.rand.NextBool(2))
					{
						resultType = ItemID.Book;
						resultStack = resultStack / 4 + 1;
					}
				}
            }
		}
    }
	public class SootWallTile : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<SootDust>();
			AddMapEntry(new Color(34, 29, 24));
		}
	}
	public class SootWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<SootWallTile>();
		}
    }
    public class SootSlabTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileBrick[Type] = true;
            Main.tileSolid[Type] = true;
            //Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            DustType = ModContent.DustType<SootDust>();
			HitSound = SoundID.Tink;
            AddMapEntry(new Color(101, 98, 95));
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
			tileFrameX += (short)((i + j) % 3 * 288);
			tileFrameY += (short)(j % 2 * 270);
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            SOTS.MergeWithFrame(i, j, Type, ModContent.TileType<SootBlockTile>(), resetFrame);
            return false;
        }
    }
    public class SootSlab : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(100);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.createTile = ModContent.TileType<SootSlabTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(5).AddIngredient<SootBlock>(1).AddIngredient(ItemID.StoneBlock, 1).AddTile(TileID.Furnaces).Register();
        }
    }
	public class CorruptionSoot : BasicBlock<CorruptionSootTile>
	{
		public class CorruptionSootTile : BasicTile
        {
            public override void SafeSetStaticDefaults()
            {
                DustType = ModContent.DustType<SootDust>();
                HitSound = SoundID.Tink;
                AddMapEntry(new Color(101, 98, 95));
            }
            public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
            {
                SOTS.MergeWithFrame(i, j, Type, ModContent.TileType<SootBlockTile>(), resetFrame: resetFrame);
                return false;
            }
        }
    }
    public class CrimsonSoot : BasicBlock<CrimsonSootTile>
    {
        public class CrimsonSootTile : BasicTile
        {
            public override void SafeSetStaticDefaults()
            {
                DustType = ModContent.DustType<SootDust>();
                HitSound = SoundID.Tink;
                AddMapEntry(new Color(101, 98, 95));
            }
            public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
            {
                SOTS.MergeWithFrame(i, j, Type, ModContent.TileType<SootBlockTile>(), resetFrame: resetFrame);
                return false;
            }
        }
    }
}