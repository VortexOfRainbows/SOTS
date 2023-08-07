using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Terraria.DataStructures;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class CursedApple : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 32;
			Item.maxStack = 1;
			Item.rare = ItemRarityID.LightPurple;
			//Item.consumable = true;
			//Item.createTile = mod.TileType("CursedAppleTile");
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.accessory = true;
			Item.canBePlacedInVanityRegardlessOfConditions = true;
			Item.shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
		}
		public override void EquipFrameEffects(Player player, EquipType type)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.petPepper = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.soulsOnKill += 2;
			//modPlayer.typhonRange = 120;
			if (!hideVisual)
				modPlayer.petPepper = true;
		}
	}
	public class CursedAppleTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(185, 20, 40), name);
			TileObjectData.addTile(Type);
			HitSound = SoundID.Grass;
			MineResist = 0.5f;
			DustType = DustID.Grass;

		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<CursedApple>());//this defines what to drop when this tile is destroyed
		}
	}
}