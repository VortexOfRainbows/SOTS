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
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			TileID.Sets.DisableSmartCursor[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(191, 142, 111), name);
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
}