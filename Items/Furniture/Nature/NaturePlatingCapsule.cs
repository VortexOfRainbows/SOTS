using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.Nature
{
    public class NaturePlatingCapsule : ModItem
    {
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.StoneBlock);
            item.Size = new Vector2(32, 24);
            item.rare = ItemRarityID.Blue;
            item.createTile = ModContent.TileType<NaturePlatingCapsuleTile>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<NaturePlating>(), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
    public class NaturePlatingCapsuleTile : ContainerType
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override string ChestName => "Nature Plating Capsule";
        protected override int ChestDrop => ModContent.ItemType<NaturePlatingCapsule>();
        protected override void AddMapEntires()
        {
            AddMapEntry(Color.Lerp(SOTSTile.NaturePlatingColor, Color.Black, 0.17f), CreateMapEntryName(GetType().Name), MapChestName);
            dustType = DustID.Tungsten;
        }
        protected override int ShowHoverItem(Player player, int i, int j, int x, int y)
        {
            return ChestDrop;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 32, chestDrop);
            base.KillMultiTile(i, j, frameX, frameY);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            try
            {
                Tile tile = Main.tile[i, j];
                int left = i;
                int top = j;
                if (tile.frameX % 36 != 0)
                {
                    left--;
                }
                if (tile.frameY != 0)
                {
                    top--;
                }
                int chest = Chest.FindChest(left, top);
                var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                Main.spriteBatch.Draw(ModContent.GetTexture(this.GetPath("Glow")), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, 38 * (chest == -1 ? 0 : Main.chest[chest].frame) + tile.frameY, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            catch
            {

            }
        }
    }
}