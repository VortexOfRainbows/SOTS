using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Otherworld;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Earthen
{
	public class EarthenPlatingStorage : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(32, 24);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EarthenPlatingStorageTile>();
            //Item.placeStyle = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EarthenPlating>(), 20).AddTile(TileID.Anvils).Register();
        }
    }
    public class EarthenPlatingStorageTile : ContainerType
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override int ChestKey => ModContent.ItemType<OldKey>();
        protected override int ChestDrop => ModContent.ItemType<EarthenPlatingStorage>();
        protected override int DustType => DustID.Iron;
        protected override void AddMapEntires()
        {
            Color color = Color.Lerp(SOTSTile.EarthenPlatingColor, Color.Black, 0.17f);
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
}