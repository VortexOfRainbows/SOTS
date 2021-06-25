using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid.AncientGold
{
	public class AncientGoldCampfire : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eternal Fireplace");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 18;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.rare = ItemRarityID.LightRed;
			item.value = 0;
			item.consumable = true;
			item.createTile = mod.TileType("AncientGoldCampfireTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<RoyalGoldBrick>(), 10);
			recipe.AddIngredient(ModContent.ItemType<AncientGoldTorch>(), 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}	
	public class AncientGoldCampfireTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Eternal Fireplace");
			AddMapEntry(new Color(255, 220, 100), name);
			disableSmartCursor = true;
			dustType = DustID.GoldCoin;
			adjTiles = new int[] { TileID.WorkBenches };
			animationFrameHeight = 36;
		}
        public override void NearbyEffects(int i, int j, bool closer)
        {
			if (closer)
			{
				Main.LocalPlayer.AddBuff(BuffID.Campfire, 6, true);
			}
            base.NearbyEffects(i, j, closer);
		}
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 2)
			{
				frameCounter = 0;
				frame++;
				if (frame >= 8)
				{
					frame = 0;
				}
			}
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = mod.ItemType("AncientGoldCampfire");
			Item.NewItem(i * 16, j * 16, 48, 32, drop);
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].frameX < 18 || Main.tile[i, j].frameX > 35 || Main.tile[i, j].frameY % 36 < 18)
				return;

			r = 1.3f;
			g = 1.1f;
			b = 1.1f;
		}
	}
}