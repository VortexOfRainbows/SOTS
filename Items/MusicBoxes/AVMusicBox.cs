using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Fragments;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.MusicBoxes
{
	public class AVMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<AVMusicBoxTile>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}
		public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<SootBlock>(100).AddIngredient<FragmentOfEvil>(10).AddIngredient(ItemID.MusicBox).AddTile(TileID.HeavyWorkBench).Register();
        }
	}
	public class AVMusicBoxTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 0;
			TileObjectData.addTile(Type);
			TileID.Sets.DisableSmartCursor[Type] = true;
			AddMapEntry(new Color(191, 142, 111), Language.GetText("ItemName.MusicBox"));
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
			yield return new Item(ModContent.ItemType<AVMusicBox>());
        }
        public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<AVMusicBox>();
		}
    }
    public class AVMinesMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.createTile = ModContent.TileType<AVMinesMusicBoxTile>();
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 100000;
            Item.accessory = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<SootBlock>(100).AddIngredient<FragmentOfEvil>(5).AddIngredient<FragmentOfEarth>(5).AddIngredient(ItemID.MusicBox).AddTile(TileID.HeavyWorkBench).Register();
        }
    }
    public class AVMinesMusicBoxTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 0;
            TileObjectData.addTile(Type);
            TileID.Sets.DisableSmartCursor[Type] = true;
            AddMapEntry(new Color(191, 142, 111), Language.GetText("ItemName.MusicBox"));
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<AVMinesMusicBox>());
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<AVMinesMusicBox>();
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int frameX = Main.tile[i, j].TileFrameX / 18;
            int frameY = Main.tile[i, j].TileFrameY / 18;
            if (frameX >= 2)
            {
                frameX += (int)Main.GameUpdateCount / 9 % 4 * 2;
                Texture2D glowmask = ModContent.Request<Texture2D>(this.GetPath("Glow")).Value;
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                Main.spriteBatch.Draw(glowmask, new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + zero, new Rectangle(frameX * 18, frameY * 18, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}