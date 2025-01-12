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

namespace SOTS.Items.Furniture.Evil
{
	public class EvilPlatingBathtub : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(36, 22);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<EvilPlatingBathtubTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<EvilPlating>(14).AddTile(TileID.Anvils).Register();
		}
	}
	public class EvilPlatingBathtubTile : Bathtub<EvilPlatingBathtub>
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
    public class EvilPlatingBed : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(40, 26);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingBedTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 15).AddIngredient(ItemID.Silk, 5).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingBedTile : Bed<EvilPlatingBed>
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
    public class EvilPlatingBlastDoor : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.rare = ItemRarityID.Blue;
            Item.width = 16;
            Item.height = 36;
            Item.createTile = ModContent.TileType<EvilPlatingBlastDoorTileClosed>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 6).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingBlastDoorTileClosed : BlastDoorClosed
    {
        public override int DoorItemID => ModContent.ItemType<Evil.EvilPlatingBlastDoor>();
        public override int OpenDoorTile => ModContent.TileType<Evil.EvilPlatingBlastDoorTileOpen>();
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
    public class EvilPlatingBlastDoorTileOpen : BlastDoorOpen
    {
        public override int DoorItemID => ModContent.ItemType<Evil.EvilPlatingBlastDoor>();
        public override int ClosedDoorTile => ModContent.TileType<Evil.EvilPlatingBlastDoorTileClosed>();
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
    public class EvilPlatingBookcase : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(32, 40);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingBookcaseTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 20).AddIngredient(ItemID.Book, 10).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingBookcaseTile : Bookcase<EvilPlatingBookcase>
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
    public class EvilPlatingCandle : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(16, 20);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingCandleTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 4).AddIngredient(ItemID.Torch, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingCandleTile : Candle<EvilPlatingCandle>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override Vector3 LightClr => SOTSTile.EvilPlatingLight * 3f;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            for (int k = 0; k < 4; k++)
            {
                SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, new Color(60, 50, 50, 0), Main.rand.NextVector2Circular(1, 1) * (k * 0.25f));
            }
        }
    }
    public class EvilPlatingChair : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(16, 34);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingChairTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 4).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingChairTile : Chair<EvilPlatingChair>
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
    public class EvilPlatingChandelier : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(38, 42);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingChandelierTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 4).AddIngredient(ItemID.Torch, 4).AddIngredient(ItemID.Chain, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingChandelierTile : Chandelier<EvilPlatingChandelier>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override Vector3 LightClr => SOTSTile.EvilPlatingLight * 3f;

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            for (int k = 0; k < 4; k++)
            {
                SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, new Color(60, 50, 50, 0), Main.rand.NextVector2Circular(1, 1) * (k * 0.25f));
            }
        }
    }
    public class EvilPlatingClock : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(24, 50);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingClockTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 20).AddIngredient(ItemID.Glass, 6).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingClockTile : Clock<EvilPlatingClock>
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
    public class EvilPlatingDresser : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(38, 26);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingDresserTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 16).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingDresserTile : Dresser
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override int DresserDrop => ModContent.ItemType<EvilPlatingDresser>();
        protected override string DresserName => Language.GetTextValue("Mods.SOTS.ContainerName.EvilPlatingDresserTile");
        public override LocalizedText DefaultContainerName(int frameX, int frameY)
        {
            return Language.GetText("Mods.SOTS.ContainerName.EvilPlatingDresserTile");
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
    public class EvilPlatingLamp : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(14, 34);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingLampTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 3).AddIngredient(ItemID.Torch, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingLampTile : Lamp<EvilPlatingLamp>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override Vector3 LightClr => SOTSTile.EvilPlatingLight * 3f;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int xFrameOffset = Main.tile[i, j].TileFrameX;
            int yFrameOffset = Main.tile[i, j].TileFrameY;
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            Vector2 drawOffset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawPosition = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + drawOffset;
            Color drawColour= new Color(60, 50, 50, 0);
            for (int k = 0; k < 4; k++)
            {
                spriteBatch.Draw(glowmask, drawPosition + Main.rand.NextVector2Circular(1, 1) * (k * 0.25f), new Rectangle(xFrameOffset, yFrameOffset, 16, 16), drawColour, 0.0f, Vector2.Zero, 1f, i % 2 == 1 ?  SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
            }
        }
    }
    public class EvilPlatingLantern : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(16, 32);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingLanternTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 6).AddIngredient(ItemID.Torch, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingLanternTile : Lantern<EvilPlatingLantern>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override Vector3 LightClr => SOTSTile.EvilPlatingLight * 3f;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int xFrameOffset = Main.tile[i, j].TileFrameX;
            int yFrameOffset = Main.tile[i, j].TileFrameY;
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            Vector2 drawOffset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawPosition = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + drawOffset;
            Color drawColour= new Color(60, 50, 50, 0);
            var effects = SpriteEffects.None;
            SetSpriteEffects(i, j, ref effects);
            if (effects.HasFlag(SpriteEffects.FlipHorizontally))
            {
                drawPosition.X -= 0f;
            }
            for (int k = 0; k < 4; k++)
            {
                spriteBatch.Draw(glowmask, drawPosition + Main.rand.NextVector2Circular(1, 1) * (k * 0.25f), new Rectangle(xFrameOffset, yFrameOffset, 16, 16), drawColour, 0.0f, Vector2.Zero, 1f, effects, 0.0f);
            }
        }
    }
    public class EvilPlatingPiano : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(38, 30);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingPianoTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 15).AddIngredient(ItemID.Bone, 4).AddIngredient(ItemID.Book, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingPianoTile : Piano<EvilPlatingPiano>
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
    public class EvilPlatingPlatform : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(200);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.rare = ItemRarityID.Blue;
            Item.width = 26;
            Item.height = 16;
            Item.createTile = ModContent.TileType<EvilPlatingPlatformTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(2).AddIngredient(ModContent.ItemType<EvilPlating>()).Register();
            Recipe.Create(ModContent.ItemType<EvilPlating>()).AddIngredient(this, 2).Register();
        }
    }
    public class EvilPlatingPlatformTile : ModTile
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
            AddMapEntry(SOTSTile.EvilPlatingColor);
            DustType = DustID.Lead;
            //ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<EvilPlatingPlatform>();
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
    public class EvilPlatingSink : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(32, 32);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingSinkTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 6).AddIngredient(ItemID.WaterBucket, 1).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingSinkTile : Sink<EvilPlatingSink>
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
    public class EvilPlatingSofa : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(40, 26);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingSofaTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 5).AddIngredient(ItemID.Silk, 2).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingSofaTile : Sofa<EvilPlatingSofa>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
    public class EvilPlatingChest : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(32, 34);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingChestTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 20).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingChestTile : ContainerType
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override int ChestKey => ModContent.ItemType<OldKey>();
        protected override int ChestDrop => ModContent.ItemType<EvilPlatingChest>();
        protected override int DustType => DustID.Lead;
        protected override void AddMapEntires()
        {
            Color color = Color.Lerp(SOTSTile.EvilPlatingColor, Color.Black, 0.17f);
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
    public class EvilPlatingTable : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(40, 26);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingTableTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 8).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingTableTile : Table<EvilPlatingTable>
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
    public class EvilPlatingToilet : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(16, 32);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingToiletTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 8).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingToiletTile : Chair<EvilPlatingToilet>
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
    public class EvilPlatingTorch : ModItem
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
            Item.Size = new Vector2(14, 16);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EvilPlatingTorchTile>();
        }
        public override void HoldItem(Player player)
        {
            Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
            Lighting.AddLight(position, SOTSTile.EvilPlatingLight * 2.7f);
        }
        public override void PostUpdate()
        {
            if (!Item.wet)
            {
                Lighting.AddLight(new Vector2((Item.position.X + Item.width / 2) / 16f, (Item.position.Y + Item.height / 2) / 16f), SOTSTile.EvilPlatingLight * 2.7f);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe(3).AddIngredient(ItemID.Torch, 3).AddIngredient(ModContent.ItemType<EvilPlating>()).Register();
        }
    }
    public class EvilPlatingTorchTile : ModTile
    {
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 5;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, DustID.RainbowMk2);
            dust.color = ColorHelper.RedEvilColor;
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
            AddMapEntry(new Color(SOTSTile.EvilPlatingLight * 3), name);
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
            Vector3 color = SOTSTile.EvilPlatingLight * 2.5f;
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
            Color color= new Color(60, 50, 50, 0);
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
            for (int k = 0; k < 4; k++)
            {
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow")), drawPosition + Main.rand.NextVector2Circular(1, 1) * (k * 0.25f), new Rectangle(frameX, frameY, width, height), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            }
        }
    }
    public class EvilPlatingWorkBench : ModItem
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
            Item.createTile = ModContent.TileType<EvilPlatingWorkBenchTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EvilPlating>(), 10).AddTile(TileID.Anvils).Register();
        }
    }
    public class EvilPlatingWorkBenchTile : Workbench<EvilPlatingWorkBench>
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