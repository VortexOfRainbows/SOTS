using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.Nature
{
    public class NaturePlatingCapsule : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(32, 24);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<NaturePlatingCapsuleTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<NaturePlating>(), 20).AddTile(TileID.Anvils).Register();
        }
    }
    public class NaturePlatingCapsuleTile : ContainerType
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override string ChestName => "Nature Plating Capsule";
        protected override int ChestKey => ModContent.ItemType<OldKey>();
        protected override int ChestDrop => ModContent.ItemType<NaturePlatingCapsule>();
        protected override int DustType => DustID.Tungsten;
        protected override void AddMapEntires()
        {
            Color color = Color.Lerp(SOTSTile.NaturePlatingColor, Color.Black, 0.17f);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault(ChestName);
            AddMapEntry(color, name, MapChestName);

            name = CreateMapEntryName(Name + "_Locked"); // With multiple map entries, you need unique translation keys.
            name.SetDefault("Locked " + ChestName);
            AddMapEntry(color, name, MapChestName);
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
}