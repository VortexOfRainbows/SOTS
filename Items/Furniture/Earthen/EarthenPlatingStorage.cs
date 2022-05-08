using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.GhostTown;
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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<EarthenPlating>(), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
    public class EarthenPlatingStorageTile : ContainerType
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override string ChestName => "Earthen Plating Storage";
        protected override int ChestDrop => ModContent.ItemType<EarthenPlatingStorage>();
        protected override int DustType => DustID.Iron;
        protected override void AddMapEntires()
        {
            Color color = Color.Lerp(SOTSTile.EarthenPlatingColor, Color.Black, 0.17f);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault(ChestName);
            AddMapEntry(color, name, MapChestName);
            name = CreateMapEntryName(Name + "_Locked"); // With multiple map entries, you need unique translation keys.
            name.SetDefault("Locked " + ChestName);
            AddMapEntry(color, name, MapChestName);
        }
        public override ushort GetMapOption(int i, int j)
        {
            if (Main.tile[i, j].frameX < 36)
                return 0;
            return 1;
        }
        public override bool IsLockedChest(int i, int j) => Main.tile[i, j].frameX / 36 == 1;
        protected override bool ManageLockedChest(Player player, int i, int j, int x, int y)
        {
            int key = ModContent.ItemType<OldKey>();
            return player.ConsumeItem(key);
        }
        protected override int ShowHoverItem(Player player, int i, int j, int x, int y)
        {
            if(IsLockedChest(x, y))
                return ModContent.ItemType<OldKey>();
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
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow")), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, 38 * (chest == -1 ? 0 : Main.chest[chest].frame) + tile.frameY, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            catch
            {

            }
        }
        public override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
        {
            dustType = DustType;
            return true;
        }
    }
}