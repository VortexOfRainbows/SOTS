using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using static SOTS.Items.AbandonedVillage.FamishedBlockCorruption;
using static SOTS.Items.AbandonedVillage.FamishedBlockCrimson;

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
    public class SootSlabWall : SootWall
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.createWall = ModContent.WallType<SootSlabWallWall>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient<SootSlab>(1).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class CorruptionSootWall : SootWall
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.createWall = ModContent.WallType<CorruptionSootWallWall>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient<CorruptionSoot>(1).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class CrimsonSootWall : SootWall
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.createWall = ModContent.WallType<CrimsonSootWallWall>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient<CrimsonSoot>(1).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class CorruptionSootWallWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            WallID.Sets.Corrupt[Type] = true;
            DustType = ModContent.DustType<CorruptionSootDust>();
            AddMapEntry(new Color(63, 50, 70));
        }
    }
    public class CrimsonSootWallWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            WallID.Sets.Crimson[Type] = true;
            DustType = ModContent.DustType<CrimsonSootDust>();
            AddMapEntry(new Color(75, 42, 42));
        }
    }
    public class SootSlabWallWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<SootDust>();
            AddMapEntry(new Color(34, 29, 24));
        }
    }
    public class UnsafeCorruptionSootWall : ModWall
    {
        public override string Texture => "SOTS/Items/AbandonedVillage/CorruptionSootWallWall";
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            WallID.Sets.Corrupt[Type] = true;
            DustType = ModContent.DustType<CorruptionSootDust>();
            AddMapEntry(new Color(63, 50, 70));
        }
    }
    public class UnsafeCrimsonSootWall : ModWall
    {
        public override string Texture => "SOTS/Items/AbandonedVillage/CrimsonSootWallWall";
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            WallID.Sets.Crimson[Type] = true;
            DustType = ModContent.DustType<CrimsonSootDust>();
            AddMapEntry(new Color(75, 42, 42));
        }
    }
    public class UnsafeSootSlabWall : ModWall
    {
        public override string Texture => "SOTS/Items/AbandonedVillage/SootSlabWallWall";
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<SootDust>();
            AddMapEntry(new Color(34, 29, 24));
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
            AddMapEntry(new Color(57, 50, 44));
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
            CreateRecipe(1).AddIngredient<SootSlabWall>(4).AddTile(TileID.WorkBenches).Register();
        }
    }
	public class CorruptionSoot : BasicBlock<CorruptionSootTile>
    {
        public override void SafeSetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<CorruptionSootWall>(4).AddTile(TileID.WorkBenches).Register();
        }
        public class CorruptionSootTile : BasicTile
        {
            public override void SafeSetStaticDefaults()
            {
				TileID.Sets.AddCorruptionTile(Type, 1);
                Main.tileBrick[Type] = true;
                DustType = ModContent.DustType<CorruptionSootDust>();
                HitSound = SoundID.Tink;
                AddMapEntry(new Color(125, 100, 140));
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
        public override void SafeSetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<CrimsonSootWall>(4).AddTile(TileID.WorkBenches).Register();
        }
        public class CrimsonSootTile : BasicTile
        {
            public override void SafeSetStaticDefaults()
            {
				TileID.Sets.AddCrimsonTile(Type, 1);
				Main.tileBrick[Type] = true;
                DustType = ModContent.DustType<CrimsonSootDust>();
                HitSound = SoundID.Tink;
                AddMapEntry(new Color(150, 84, 84));
            }
            public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
            {
                SOTS.MergeWithFrame(i, j, Type, ModContent.TileType<SootBlockTile>(), resetFrame: resetFrame);
                return false;
            }
        }
    }
    public class FamishedBlockCrimson : BasicBlock<FamishedTileCrimson>
    {
        public override void SafeSetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
        }
        public class FamishedTileCrimson : BasicTile
        {
            public override string Texture => "SOTS/NPCs/AbandonedVillage/TheFamishedCrimsonVersion";
            public override void SafeSetStaticDefaults()
            {
                //TileID.Sets.AddCrimsonTile(Type, 1);
                Main.tileBrick[Type] = true;
                DustType = ModContent.DustType<FamishedDustCrimson>();
                HitSound = SoundID.NPCHit1;
                MineResist = 0.1f;
                AddMapEntry(new Color(228, 131, 134));
            }
            private static bool FramingPreventRepitions = false;
            public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
            {
                if (!FramingPreventRepitions)
                {
                    FramingPreventRepitions = true;
                    try
                    {
                        WorldGen.TileFrame(i, j, resetFrame, noBreak);
                        if (Main.tile[i, j].TileFrameY < 90 && WorldGen.genRand.NextBool(2))
                        {
                            Main.tile[i, j].TileFrameY += 90;
                        }
                        FramingPreventRepitions = false;
                        return false;
                    }
                    catch
                    {

                    }
                    FramingPreventRepitions = false;
                }
                return true;
            }
            public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
            {
                if (Main.tile[i, j].TileFrameY >= 90)
                {
                    Vector2 lookToPlayer = Main.LocalPlayer.Center - new Vector2(i * 16 + 8, j * 16 + 8);
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, (Texture2D)ModContent.Request<Texture2D>(Texture + "Glow"), Color.White, lookToPlayer.SNormalize() * 2f, false);
                }
            }
        }
    }
    public class FamishedBlockCorruption : BasicBlock<FamishedTileCorruption>
    {
        public override void SafeSetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
        }
        public class FamishedTileCorruption : BasicTile
        {
            public override string Texture => "SOTS/NPCs/AbandonedVillage/TheFamishedCorruptionVersion";
            public override void SafeSetStaticDefaults()
            {
                //TileID.Sets.AddCorruptionTile(Type, 1);
                Main.tileBrick[Type] = true;
                DustType = ModContent.DustType<FamishedDustCorruption>();
                HitSound = SoundID.NPCHit1;
                MineResist = 0.1f;
                AddMapEntry(new Color(169, 139, 126));
            }
            private static bool FramingPreventRepitions = false;
            public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
            {
                if (!FramingPreventRepitions)
                {
                    FramingPreventRepitions = true;
                    try
                    {
                        WorldGen.TileFrame(i, j, resetFrame, noBreak);
                        if (Main.tile[i, j].TileFrameY < 90 && WorldGen.genRand.NextBool(2))
                        {
                            Main.tile[i, j].TileFrameY += 90;
                        }
                        FramingPreventRepitions = false;
                        return false;
                    }
                    catch
                    {

                    }
                    FramingPreventRepitions = false;
                }
                return true;
            }
            public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
            {
                if (Main.tile[i, j].TileFrameY >= 90)
                {
                    Vector2 lookToPlayer = Main.LocalPlayer.Center - new Vector2(i * 16 + 8, j * 16 + 8);
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, (Texture2D)ModContent.Request<Texture2D>(Texture + "Glow"), Color.White, lookToPlayer.SNormalize() * 2f, false);
                }
            }
        }
    }
}