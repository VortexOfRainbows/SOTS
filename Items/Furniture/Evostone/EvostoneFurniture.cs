using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Items.Invidia;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Evostone
{
    public class EvostonePlatform : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(200);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.rare = ItemRarityID.Blue;
            Item.width = 26;
            Item.height = 14;
            Item.createTile = ModContent.TileType<EvostonePlatformTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(2).AddIngredient(ModContent.ItemType<EvostoneBrick>()).Register();
            Recipe.Create(ModContent.ItemType<EvostoneBrick>()).AddIngredient(this, 2).Register();
        }
    }
    public class EvostonePlatformTile : ModTile
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {

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
            AddMapEntry(ColorHelper.Evostone);
            DustType = ModContent.DustType<EvostoneDust>();
            //ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<TidalPlatingPlatform>();
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
   
}