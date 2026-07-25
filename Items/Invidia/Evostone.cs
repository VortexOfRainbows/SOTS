using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Invidia
{
	public class EvostoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			Main.tileMerge[Type][TileID.Marble] = true;
			Main.tileMerge[TileID.Marble][Type] = true;
			Main.tileMerge[Type][TileID.Mud] = true;
			Main.tileMerge[TileID.Mud][Type] = true;
			Main.tileMerge[Type][TileID.MushroomGrass] = true;
			Main.tileMerge[TileID.MushroomGrass][Type] = true;
			Main.tileMerge[Type][ModContent.TileType<EvostoneBrickTile>()] = true;
			Main.tileMerge[ModContent.TileType<EvostoneBrickTile>()][Type] = true;

            DustType = ModContent.DustType<EvostoneDust>(); //obsidian
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Evostone>();
			AddMapEntry(new Color(31, 39, 57));
			HitSound = SoundID.Tink;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			SOTS.MergeWithFrame(i, j, Type, TileID.Marble, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
	}
	public class Evostone : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(100);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<EvostoneTile>();
		}
	}
	public class EvostoneBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			Main.tileBrick[Type] = true;
			DustType = ModContent.DustType<EvostoneDust>(); //obsidian
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<EvostoneBrick>();
			AddMapEntry(ColorHelper.Evostone);
			HitSound = SoundID.Tink;
		}
	}
	public class EvostoneBrick : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(100);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<EvostoneBrickTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Evostone>(), 2).AddTile(TileID.Hellforge).Register();
		}
	}
	public class EvostoneBrickWallTile : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<EvostoneDust>();
			AddMapEntry(new Color(25, 38, 49));
			HitSound = SoundID.Tink;
		}
	}
	public class EvostoneBrickWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<EvostoneBrickWallTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<EvostoneBrick>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<EvostoneBrick>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class DarkShinglesTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			DustType = ModContent.DustType<EvostoneDust>(); //obsidian
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<DarkShingles>();
			AddMapEntry(new Color(82, 56, 103));
			HitSound = SoundID.Tink;
		}
	}
	public class DarkShingles : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(100);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<DarkShinglesTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<EvostoneBrick>(), 2).AddTile(TileID.Hellforge).Register();
		}
    }
    public class RunicEvostoneBrickTile : ModTile
    {
        public override string Texture => "SOTS/Items/Invidia/EvostoneBrickTile";
		public static Texture2D rune = null;
        public static void ModLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile t = Main.tile[i, j];
            int tFrameX = t.TileFrameX / 18;
            int tFrameY = t.TileFrameY / 18;
            bool valid = (tFrameX >= 1 && tFrameX <= 3 && tFrameY == 1) ||
                (tFrameX >= 6 && tFrameX <= 8 && (tFrameY == 1 || tFrameY == 2)) ||
                (tFrameY >= 0 && tFrameY <= 2 && (tFrameX == 10 || tFrameX == 11));
            if (!valid)
                return;
            float fillPercent = SOTSWorld.MoonPhasePercent * SOTSWorld.MoonPhasePercent * 0.9f + 0.1f * SOTSTile.PlanetariumLightingColorMultiplier(i, j) * SOTSWorld.MoonPhasePercent;
            float mult = fillPercent * 1.5f;
            r = .168f * mult;
            g = .443f * mult;
            b = .196f * mult;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            ModLight(i, j, ref r, ref g, ref b);
        }
        public static void DrawRune(int i, int j, Texture2D runeTex, float colorMult = 1f)
        {
            if (runeTex == null)
                return;
            Tile t = Main.tile[i, j];
            bool valid;
            if (colorMult == 1)
            {
                int tFrameX = t.TileFrameX / 18;
                int tFrameY = t.TileFrameY / 18;
                valid = (tFrameX >= 1 && tFrameX <= 3 && tFrameY == 1) ||
                    (tFrameX >= 6 && tFrameX <= 8 && (tFrameY == 1 || tFrameY == 2)) ||
                    (tFrameY >= 0 && tFrameY <= 2 && (tFrameX == 10 || tFrameX == 11));
            }
            else
                valid = true;
            if (!valid)
                return;
            int frame = (i * 3 + j * 11) % 20;
            int x = frame % 4;
            int y = frame / 4;
            Color lC = Lighting.GetColor(i, j);
            SOTSTile.DrawSlopedGlowMask(i, j, t.TileType, runeTex, lC, Vector2.Zero, 2 + 18 * x, 2 + 18 * y);
            float fillPercent = SOTSWorld.MoonPhasePercent * SOTSWorld.MoonPhasePercent * 0.9f + 0.1f * SOTSTile.PlanetariumLightingColorMultiplier(i, j) * SOTSWorld.MoonPhasePercent;
            Color runeColor = Color.Lerp(lC, Color.White, fillPercent) * fillPercent;
            runeColor.A = 0;
            runeColor = Color.Lerp(Color.Black, runeColor, colorMult);
            if(!SOTS.Config.SanctuaryLagReduction)
            {
                int c = SOTS.Config.lowFidelityMode ? 3 : 6;
                int d = SOTS.Config.lowFidelityMode ? 120 : 60;
                for (int a = 0; a < c; a++)
                    SOTSTile.DrawSlopedGlowMask(i, j, t.TileType, runeTex, runeColor * 0.23f * fillPercent, new Vector2(.4f + 1.8f * SOTSWorld.MoonPhasePercent, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter + a * d)), 74 + 18 * x, 2 + 18 * y);
                SOTSTile.DrawSlopedGlowMask(i, j, t.TileType, runeTex, runeColor, Vector2.Zero, 74 + 18 * x, 2 + 18 * y);
            }
            else
                SOTSTile.DrawSlopedGlowMask(i, j, t.TileType, runeTex, runeColor * 1.25f, Vector2.Zero, 74 + 18 * x, 2 + 18 * y);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
		    rune ??= ModContent.Request<Texture2D>("SOTS/Items/Invidia/Runes", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            DrawRune(i, j, rune);
        }
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBrick[Type] = true;
            DustType = ModContent.DustType<EvostoneDust>();
            AddMapEntry(Color.Lerp(new Color(14, 53, 4), new Color(46, 63, 77), 0.6f));
            HitSound = SoundID.Tink;
        }
    }
    public class RunicEvostone : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(100);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.rare = ItemRarityID.Green;
            Item.createTile = ModContent.TileType<RunicEvostoneTile>();
        }
    }
    public class RunicEvostoneTile : RunicEvostoneBrickTile
    {
        public override string Texture => "SOTS/Items/Invidia/EvostoneTile";
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<EvostoneTile>()] = true;
            Main.tileMerge[ModContent.TileType<EvostoneTile>()][Type] = true;
            Main.tileMerge[Type][TileID.Marble] = true;
            Main.tileMerge[TileID.Marble][Type] = true;
            Main.tileMerge[Type][TileID.Mud] = true;
            Main.tileMerge[TileID.Mud][Type] = true;
            Main.tileMerge[Type][TileID.MushroomGrass] = true;
            Main.tileMerge[TileID.MushroomGrass][Type] = true;
            Main.tileMerge[Type][ModContent.TileType<EvostoneBrickTile>()] = true;
            Main.tileMerge[ModContent.TileType<EvostoneBrickTile>()][Type] = true;
            DustType = ModContent.DustType<EvostoneDust>();
            AddMapEntry(Color.Lerp(new Color(14, 53, 4), new Color(31, 39, 57), 0.6f));
            HitSound = SoundID.Tink;
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            SOTS.MergeWithFrame(i, j, Type, TileID.Marble, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
            return false;
        }
    }
    public class RunicEvostoneBrick : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(100);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.rare = ItemRarityID.Green;
            Item.createTile = ModContent.TileType<RunicEvostoneBrickTile>();
        }
    }
    public class InvidiaPlating : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(100);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.rare = ItemRarityID.LightRed;
            Item.createTile = ModContent.TileType<InvidiaPlatingTile>();
        }
    }
    public class InvidiaPlatingTile : ModTile
    {
        public static Texture2D glow = null;
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float fillPercent = SOTSWorld.MoonPhasePercent * SOTSWorld.MoonPhasePercent * 0.6f + 0.4f * SOTSTile.PlanetariumLightingColorMultiplier(i, j) * SOTSWorld.MoonPhasePercent;
            float mult = fillPercent;
            r = .168f * mult;
            g = .443f * mult;
            b = .196f * mult;
        }
        public bool ValidTile(int i, int j)
        {
            Tile t = Main.tile[i, j];
            int type = t.TileType;
            return t.HasTile && Main.tileSolid[type] && 
                !Main.tileSolidTop[type] && (Main.tileBrick[type] || Main.tileBlendAll[type]);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tileTexture = TextureAssets.Tile[Type].Value;
            glow ??= ModContent.Request<Texture2D>("SOTS/Items/Invidia/InvidiaPlatingTileGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Tile t = Main.tile[i, j];
            Color lC = Lighting.GetColor(i, j);
            float fillPercent = SOTSWorld.MoonPhasePercent * SOTSWorld.MoonPhasePercent * 0.6f + 0.4f * SOTSTile.PlanetariumLightingColorMultiplier(i, j) * SOTSWorld.MoonPhasePercent;
            Color color = Color.Lerp(lC, Color.White, fillPercent) * fillPercent;
            color.A = 0;
            int c = SOTS.Config.SanctuaryLagReduction || SOTS.Config.lowFidelityMode ? 3 : 6;
            int d = SOTS.Config.SanctuaryLagReduction || SOTS.Config.lowFidelityMode ? 120 : 60;
			float moonDist = .4f + 1.8f * SOTSWorld.MoonPhasePercent;
            bool top = ValidTile(i, j - 1) && (Main.tile[i, j - 1].Slope == 0 || Main.tile[i, j - 1].TopSlope);
            bool bot = ValidTile(i, j + 1) && (Main.tile[i, j + 1].Slope == 0 || Main.tile[i, j + 1].BottomSlope);
            bool left = ValidTile(i - 1, j) && (Main.tile[i - 1, j].Slope == 0 || Main.tile[i - 1, j].LeftSlope);
            bool right = ValidTile(i + 1, j) && (Main.tile[i + 1, j].Slope == 0 || Main.tile[i - 1, j].RightSlope);
            bool topLeft = ValidTile(i - 1, j - 1) && (Main.tile[i - 1, j - 1].Slope == SlopeType.SlopeDownRight || Main.tile[i - 1, j - 1].Slope == 0);
            bool topRight = ValidTile(i + 1, j - 1) && (Main.tile[i + 1, j - 1].Slope == SlopeType.SlopeDownLeft || Main.tile[i + 1, j - 1].Slope == 0);
            bool botLeft = ValidTile(i - 1, j + 1) && (Main.tile[i - 1, j + 1].Slope == SlopeType.SlopeUpRight || Main.tile[i - 1, j + 1].Slope == 0);
            bool botRight = ValidTile(i + 1, j + 1) && (Main.tile[i + 1, j + 1].Slope == SlopeType.SlopeUpLeft || Main.tile[i + 1, j + 1].Slope == 0);
            bool drawTopLeft = top && left && !topLeft;
            bool drawTopRight = top && right && !topRight;
            bool drawBotLeft = bot && left && !botLeft;
            bool drawBotRight = bot && right && !botRight;
            if (drawTopLeft && drawTopRight && drawBotLeft && drawBotRight)
            {
                SOTSTile.DrawSlopedGlowMask(i, j, Type, tileTexture, lC, Vector2.Zero, 216, 72);
                for (int a = 0; a < c; a++)
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color * 0.23f * fillPercent, new Vector2(moonDist, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter + a * d)), 216, 72);
                SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color, Vector2.Zero, 216, 72);
            }
            else
            {
                if (drawTopLeft)
                {
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, tileTexture, lC, Vector2.Zero, 162, 72);
                    for (int a = 0; a < c; a++)
                        SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color * 0.23f * fillPercent, new Vector2(moonDist, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter + a * d)), 162, 72);
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color, Vector2.Zero, 162, 72);
                }
                if (drawTopRight)
                {
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, tileTexture, lC, Vector2.Zero, 180, 72);
                    for (int a = 0; a < c; a++)
                        SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color * 0.23f * fillPercent, new Vector2(moonDist, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter + a * d)), 180, 72);
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color, Vector2.Zero, 180, 72);
                }
                if (drawBotLeft)
                {
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, tileTexture, lC, Vector2.Zero, 198, 72);
                    for (int a = 0; a < c; a++)
                        SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color * 0.23f * fillPercent, new Vector2(moonDist, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter + a * d)), 198, 72);
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color, Vector2.Zero, 198, 72);
                }
                if (drawBotRight)
                {
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, tileTexture, lC, Vector2.Zero, 216, 54);
                    for (int a = 0; a < c; a++)
                        SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color * 0.23f * fillPercent, new Vector2(moonDist, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter + a * d)), 216, 54);
                    SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color, Vector2.Zero, 216, 54);
                }
            }
            for (int a = 0; a < c; a++)
                SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color * 0.23f * fillPercent, new Vector2(moonDist, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter + a * d)));
            SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color, Vector2.Zero);
        }
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBrick[Type] = true;
            DustType = ModContent.DustType<EvostoneDust>();
            AddMapEntry(new Color(14, 53, 4));
            HitSound = SoundID.Tink;
        }
    }
    public class EvostoneGrandPillarWall : ModWall
    {
        public static Texture2D PillarTexture;
        public override string Texture => "SOTS/Items/Invidia/EvostoneBrickWallTile";
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<EvostoneDust>();
            AddMapEntry(new Color(20, 31, 41));
            HitSound = SoundID.Dig;
        }
        public override bool CanExplode(int i, int j) => false;
        public override void KillWall(int i, int j, ref bool fail) => fail = false;
        public override bool CanPlace(int i, int j) => true;
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            TileColorCache colorCache = tile.WallColorAndCoating();
            if (colorCache.Invisible && !Main.LocalPlayer.CanSeeInvisibleBlocks)
                return false;
            Color paint = WorldGen.paintColor(colorCache.Color);
            int frameX = tile.WallFrameX / 36;
            int frameY = tile.WallFrameY / 36;
            frameX += frameY * 8;
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
            PillarTexture ??= ModContent.Request<Texture2D>("SOTS/Items/Invidia/SanctuaryPillar", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 drawPos = new Vector2(i, j) * 16 + zero - Main.screenPosition;
            Vector3[] slices = new Vector3[4];
            if (colorCache.FullBright)
                for (int x = 0; x < 4; ++x)
                    slices[x] = Vector3.One;
            else
                Lighting.GetColor4Slice(i, j, ref slices);
            Vector3 vector = Lighting.GetColor(i, j).ToVector3();
            Vector3 tileLight;
            Vector2 position;
            Color color = new();
            Rectangle value = new();
            for (int a = 0; a < 4; a++)
            {
                value.X = 0;
                value.Y = 0;
                value.Width = 8;
                value.Height = 8;
                switch (a)
                {
                    case 1:
                        value.X = 8;
                        break;
                    case 2:
                        value.Y = 8;
                        break;
                    case 3:
                        value.Y = value.X = 8;
                        break;
                }
                //value.Y += glowOffset.Y;
                position.X = drawPos.X + value.X;
                position.Y = drawPos.Y + value.Y;
                value.X += frameX * 16;
                value.Y += (j % 4) * 16;
                tileLight.X = (slices[a].X + vector.X) * 0.5f;
                tileLight.Y = (slices[a].Y + vector.Y) * 0.5f;
                tileLight.Z = (slices[a].Z + vector.Z) * 0.5f;
                int num = (int)(tileLight.X * 255f);
                int num2 = (int)(tileLight.Y * 255f);
                int num3 = (int)(tileLight.Z * 255f);
                if (num > 255)
                    num = 255;
                if (num2 > 255)
                    num2 = 255;
                if (num3 > 255)
                    num3 = 255;
                num3 <<= 16;
                num2 <<= 8;
                color.PackedValue = (uint)(num | num2 | num3) | 0xFF000000u;
                Terraria.Graphics.VertexColors c = new(color.MultiplyRGB(paint));
                //Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors colors, Vector2 origin, float scale, SpriteEffects effects)
                Main.tileBatch.Draw(PillarTexture, position, value, c, Vector2.Zero, 1f, SpriteEffects.None);
                //spriteBatch.Draw(PillarTexture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override bool WallFrame(int i, int j, bool randomizeFrame, ref int style, ref int frameNumber)
        {
            FrameWall(i, j, true);
            return false;
        }
        public void FrameWall(int i, int j, bool recursive = false)
        {
            Tile t = Main.tile[i, j];
            t.WallFrameX = 0;
            t.WallFrameY = 0;
            int frameNumber = 0;
            int tilesLeft = 0, tilesRight = 0;
            int span = 39;
            bool countR = true, countL = true;
            int x = 1;
            for (; x < span; ++x)
            {
                Tile tR = Framing.GetTileSafely(i + x, j);
                Tile tL = Framing.GetTileSafely(i - x, j);
                if (tR.WallType != t.WallType)
                    countR = false;
                if (tL.WallType != t.WallType)
                    countL = false;
                if (countR)
                {
                    tilesRight++;
                    if(recursive)
                        FrameWall(i  + x, j, false);
                }
                if (countL)
                {
                    tilesLeft++;
                    if(recursive)
                        FrameWall(i - x, j, false);
                }
                if (!countL && !countR)
                    break;
            }
            if (tilesLeft > tilesRight) //More tiles to the left
            {
                int fromRight = span - 1;
                fromRight -= tilesRight;
                frameNumber = fromRight;
            }
            else
            {
                int fromLeft = 0;
                fromLeft += tilesLeft;
                frameNumber = fromLeft;
            }
            //only 3 bits for both frameX and frameY, meaning the highest value is 7 (* 36 = 252)
            x = frameNumber % 8;
            int y = frameNumber / 8;
            t.WallFrameX = x * 36;
            t.WallFrameY = y * 36;
        }
    }
    public class EvostoneGrandPillar : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(400);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneWall);
            Item.width = 38;
            Item.height = 34;
            Item.rare = ItemRarityID.Blue;
            Item.createWall = ModContent.WallType<EvostoneGrandPillarWall>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient(ModContent.ItemType<EvostoneBrick>(), 1).AddTile(TileID.HeavyWorkBench).Register();
            Recipe.Create(ModContent.ItemType<EvostoneBrick>()).AddIngredient(this, 4).AddTile(TileID.HeavyWorkBench).Register();
        }
    }
    public class EvostoneRuneBrickWallTile : EvostoneBrickWallTile
    {
        public override string Texture => "SOTS/Items/Invidia/EvostoneBrickWallTile";
        public static Texture2D rune = null;
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<EvostoneDust>();
            HitSound = SoundID.Tink;
            AddMapEntry(Color.Lerp(new Color(14, 53, 4), new Color(25, 38, 49), 0.6f));
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            RunicEvostoneBrickTile.ModLight(i, j, ref r, ref g, ref b);
            r *= 0.7f;
            g *= 0.7f;
            b *= 0.7f;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            rune ??= ModContent.Request<Texture2D>("SOTS/Items/Invidia/RunesWall", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            RunicEvostoneBrickTile.DrawRune(i, j, rune, 0.9f);
        }
    }
    public class EvostoneRuneBrickWall : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(400);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneWall);
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Green;
            Item.createWall = ModContent.WallType<EvostoneRuneBrickWallTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient(ModContent.ItemType<RunicEvostoneBrick>(), 1).AddTile(TileID.WorkBenches).Register();
            Recipe.Create(ModContent.ItemType<RunicEvostoneBrick>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
        }
    }
}