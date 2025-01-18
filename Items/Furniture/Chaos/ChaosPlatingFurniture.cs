using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Helpers;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Chaos
{
	public class ChaosPlatingBathtub : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(40, 24);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<ChaosPlatingBathtubTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<ChaosPlating>(14).AddTile(TileID.Anvils).Register();
		}
	}
	public class ChaosPlatingBathtubTile : Bathtub<ChaosPlatingBathtub>
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class ChaosPlatingBed : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(40, 24);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingBedTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 15).AddIngredient(ItemID.Silk, 5).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingBedTile : Bed<ChaosPlatingBed>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class ChaosPlatingBlastDoor : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.rare = ItemRarityID.Blue;
            Item.width = 14;
            Item.height = 34;
            Item.createTile = ModContent.TileType<ChaosPlatingBlastDoorTileClosed>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 6).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingBlastDoorTileClosed : BlastDoorClosed
    {
        public override int DoorItemID => ModContent.ItemType<Chaos.ChaosPlatingBlastDoor>();
        public override int OpenDoorTile => ModContent.TileType<Chaos.ChaosPlatingBlastDoorTileOpen>();
        public override string GetName()
        {
            return this.GetLocalizedValue("MapEntry");
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class ChaosPlatingBlastDoorTileOpen : BlastDoorOpen
    {
        public override int DoorItemID => ModContent.ItemType<Chaos.ChaosPlatingBlastDoor>();
        public override int ClosedDoorTile => ModContent.TileType<Chaos.ChaosPlatingBlastDoorTileClosed>();
        public override string GetName()
        {
            return this.GetLocalizedValue("MapEntry");
        }
    }
    public class ChaosPlatingBookcase : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(36, 44);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingBookcaseTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 20).AddIngredient(ItemID.Book, 10).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingBookcaseTile : Bookcase<ChaosPlatingBookcase>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class ChaosPlatingCandle : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(16, 20);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingCandleTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 4).AddIngredient(ItemID.Torch, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingCandleTile : Candle<ChaosPlatingCandle>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override Vector3 LightClr => SOTSTile.ChaosPlatingLight * 3f;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            for (int k = 0; k < 3; k++)
            {
                SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, new Color(100, 100, 100, 0), Main.rand.NextVector2Circular(1, 1) * (k * 0.15f));
            }
        }
    }
    public class ChaosPlatingChair : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(16, 30);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingChairTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 4).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingChairTile : Chair<ChaosPlatingChair>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override void SetStaticDefaults(TileObjectData t)
        {
            Main.tileLighted[Type] = true;
            base.SetStaticDefaults(t);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class ChaosPlatingChandelier : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(38, 36);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingChandelierTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 4).AddIngredient(ItemID.Torch, 4).AddIngredient(ItemID.Chain, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingChandelierTile : Chandelier<ChaosPlatingChandelier>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override Vector3 LightClr => SOTSTile.ChaosPlatingLight * 3f;

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            for (int k = 0; k < 3; k++)
            {
                SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, new Color(100, 100, 100, 0), Main.rand.NextVector2Circular(1, 1) * (k * 0.15f));
            }
        }
    }
    public class ChaosPlatingClock : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(24, 50);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingClockTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 20).AddIngredient(ItemID.Glass, 6).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingClockTile : Clock<ChaosPlatingClock>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class ChaosPlatingDresser : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(36, 28);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingDresserTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 16).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingDresserTile : Dresser
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override int DresserDrop => ModContent.ItemType<ChaosPlatingDresser>();
        protected override string DresserName => Language.GetTextValue("Mods.SOTS.ContainerName.ChaosPlatingDresserTile");
        public override LocalizedText DefaultContainerName(int frameX, int frameY)
        {
            return Language.GetText("Mods.SOTS.ContainerName.ChaosPlatingDresserTile");
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class ChaosPlatingLamp : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(16, 36);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingLampTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 3).AddIngredient(ItemID.Torch, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingLampTile : Lamp<ChaosPlatingLamp>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override Vector3 LightClr => SOTSTile.ChaosPlatingLight * 3f;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int xFrameOffset = Main.tile[i, j].TileFrameX;
            int yFrameOffset = Main.tile[i, j].TileFrameY;
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            Vector2 drawOffset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawPosition = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + drawOffset;
            Color drawColour = new Color(100, 100, 100, 0);
            for (int k = 0; k < 3; k++)
            {
                spriteBatch.Draw(glowmask, drawPosition + Main.rand.NextVector2Circular(1, 1) * (k * 0.15f), new Rectangle(xFrameOffset, yFrameOffset, 16, 16), drawColour, 0.0f, Vector2.Zero, 1f, i % 2 == 1 ?  SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
            }
        }
    }
    public class ChaosPlatingLantern : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(16, 32);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingLanternTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 6).AddIngredient(ItemID.Torch, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingLanternTile : Lantern<ChaosPlatingLantern>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override Vector3 LightClr => SOTSTile.ChaosPlatingLight * 3f;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int xFrameOffset = Main.tile[i, j].TileFrameX;
            int yFrameOffset = Main.tile[i, j].TileFrameY;
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            Vector2 drawOffset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawPosition = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + drawOffset;
            Color drawColour = new Color(100, 100, 100, 0);
            var effects = SpriteEffects.None;
            SetSpriteEffects(i, j, ref effects);
            if (effects.HasFlag(SpriteEffects.FlipHorizontally))
            {
                drawPosition.X -= 0f;
            }
            for (int k = 0; k < 3; k++)
            {
                spriteBatch.Draw(glowmask, drawPosition + Main.rand.NextVector2Circular(1, 1) * (k * 0.15f), new Rectangle(xFrameOffset, yFrameOffset, 16, 16), drawColour, 0.0f, Vector2.Zero, 1f, effects, 0.0f);
            }
        }
    }
    public class ChaosPlatingPiano : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(38, 26);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingPianoTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 15).AddIngredient(ItemID.Bone, 4).AddIngredient(ItemID.Book, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingPianoTile : Piano<ChaosPlatingPiano>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class ChaosPlatingPlatform : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(200);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.rare = ItemRarityID.Blue;
            Item.width = 26;
            Item.height = 16;
            Item.createTile = ModContent.TileType<ChaosPlatingPlatformTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(2).AddIngredient(ModContent.ItemType<ChaosPlating>()).Register();
            Recipe.Create(ModContent.ItemType<ChaosPlating>()).AddIngredient(this, 2).Register();
        }
    }
    public class ChaosPlatingPlatformTile : ModTile
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleMultiplier = 27;
            TileObjectData.newTile.StyleWrapLimit = 27;
            TileObjectData.newTile.UsesCustomCanPlace = false;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
            AddMapEntry(SOTSTile.ChaosPlatingColor);
            DustType = DustID.Platinum;
            //ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<ChaosPlatingPlatform>();
            AdjTiles = new int[] { TileID.Platforms };
            TileID.Sets.Platforms[Type] = true;
        }
        public override void PostSetDefaults()
        {
            Main.tileNoSunLight[Type] = false;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
    public class ChaosPlatingSink : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(28, 26);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingSinkTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 6).AddIngredient(ItemID.WaterBucket, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingSinkTile : Sink<ChaosPlatingSink>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class ChaosPlatingSofa : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(42, 26);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingSofaTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 5).AddIngredient(ItemID.Silk, 2).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingSofaTile : Sofa<ChaosPlatingSofa>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class ChaosPlatingChest : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(32, 32);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingChestTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 20).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingChestTile : ContainerType
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override int ChestKey => ModContent.ItemType<OldKey>();
        protected override int ChestDrop => ModContent.ItemType<ChaosPlatingChest>();
        protected override int DustType => DustID.Platinum;
        protected override void AddMapEntires()
        {
            Color color = Color.Lerp(SOTSTile.ChaosPlatingColor, Color.Black, 0.17f);
            AddMapEntry(color, this.GetLocalization("MapEntry0"), MapChestName);
            AddMapEntry(color, this.GetLocalization("MapEntry1"), MapChestName);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            try
            {
                Tile tile = Main.tile[i, j];
                int left = i;
                int top = j;
                if (tile.TileFrameX % 36 != 0)
                {
                    left--;
                }
                if (tile.TileFrameY != 0)
                {
                    top--;
                }
                int chest = Chest.FindChest(left, top);
                var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow")), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, 38 * (chest == -1 ? 0 : Main.chest[chest].frame) + tile.TileFrameY, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            catch
            {

            }
        }
    }
    public class ChaosPlatingTable : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(38, 26);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingTableTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 8).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingTableTile : Table<ChaosPlatingTable>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class ChaosPlatingToilet : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(16, 28);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingToiletTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 8).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingToiletTile : Chair<ChaosPlatingToilet>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
    public class ChaosPlatingTorch : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(100);
            ItemID.Sets.Torches[Type] = true;
            ItemID.Sets.WaterTorches[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Torch);
            Item.Size = new Vector2(14, 18);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingTorchTile>();
        }
        public override void HoldItem(Player player)
        {
            Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
            Lighting.AddLight(position, SOTSTile.ChaosPlatingLight * 2.7f);
        }
        public override void PostUpdate()
        {
            if (!Item.wet)
            {
                Lighting.AddLight(new Vector2((Item.position.X + Item.width / 2) / 16f, (Item.position.Y + Item.height / 2) / 16f), SOTSTile.ChaosPlatingLight * 2.7f);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe(3).AddIngredient(ItemID.Torch, 3).AddIngredient(ModContent.ItemType<ChaosPlating>()).Register();
        }
    }
    public class ChaosPlatingTorchTile : ModTile
    {
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 5;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, DustID.RainbowMk2);
            dust.color = ColorHelper.ChaosPink;
            dust.color.A = 0;
            dust.noGravity = true;
            dust.fadeIn = 0.1f;
            dust.scale *= 1.8f;
            dust.velocity *= 2.4f;
            return false;
        }
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileWaterDeath[Type] = false;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
            TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
            TileObjectData.newAlternate.AnchorAlternateTiles = new[] { 124 };
            TileObjectData.newAlternate.WaterDeath = false;
            TileObjectData.addAlternate(1);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
            TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
            TileObjectData.newAlternate.AnchorAlternateTiles = new[] { 124 };
            TileObjectData.newAlternate.WaterDeath = false;
            TileObjectData.addAlternate(2);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
            TileObjectData.newAlternate.AnchorWall = true;
            TileObjectData.newAlternate.WaterDeath = false;
            TileObjectData.addAlternate(0);
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(SOTSTile.ChaosPlatingLight * 3), name);
            DustType = DustID.GoldCoin;
            AdjTiles = new int[] { TileID.Torches };
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.Torch[Type] = true;
        }
        public override bool CanPlace(int i, int j)
        {
            return Main.tile[i, j].LiquidAmount == 0;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Vector3 color = SOTSTile.ChaosPlatingLight * 2.5f;
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX < 66)
            {
                r = color.X;
                g = color.Y;
                b = color.Z;
            }
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = new Color(100, 100, 100, 0);
            int frameX = Main.tile[i, j].TileFrameX;
            int frameY = Main.tile[i, j].TileFrameY;
            int width = 20;
            int height = 20;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Vector2 drawPosition = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) - (width - 16f) / 2f, (float)(j * 16 - (int)Main.screenPosition.Y)) + zero;
            for (int k = 0; k < 3; k++)
            {
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow")), drawPosition + Main.rand.NextVector2Circular(1, 1) * (k * 0.15f), new Rectangle(frameX, frameY, width, height), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            }
        }
    }
    public class ChaosPlatingWorkBench : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(32, 18);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ChaosPlatingWorkBenchTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ChaosPlating>(), 10).AddTile(TileID.Anvils).Register();
        }
    }
    public class ChaosPlatingWorkBenchTile : Workbench<ChaosPlatingWorkBench>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override void SetStaticDefaults(TileObjectData t)
        {
            Main.tileLighted[Type] = true;
            base.SetStaticDefaults(t);
            AdjTiles = new int[] { TileID.WorkBenches, TileID.Anvils };
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
}