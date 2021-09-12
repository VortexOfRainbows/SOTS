using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid
{
	/*
	public class CursedGrowth : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Growth");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.rare = ItemRarityID.LightRed;
			item.value = 0;
			item.consumable = true;
			item.createTile = mod.TileType("CursedGrowthTile");
		}
	}	
	*/
	public class CursedGrowthTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileNoFail[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cursed Growth");
			AddMapEntry(new Color(135, 100, 185), name);
			disableSmartCursor = true;
			dustType = DustID.GoldCoin;
			animationFrameHeight = 36;
			dustType = ModContent.DustType<CurseDust3>();
			soundType = SoundID.NPCHit;
			soundStyle = 1;
			mineResist = 0.1f;
		}
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
			num = 2;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			return true; 
		}
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 6)
			{
				frameCounter = 0;
				frame++;
				if (frame >= 6)
				{
					frame = 0;
				}
			}
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			//Item.NewItem(i * 16, j * 16, 48, 32, mod.ItemType("CursedTumor"));
		}
	}
}