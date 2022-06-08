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
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 0;
			Item.consumable = true;
			Item.createTile = mod.TileType("CursedGrowthTile");
		}
	}	
	*/
	public class CursedGrowthTile : ModTile
	{
		public override void SetStaticDefaults()
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
			AddMapEntry(new Color(78, 55, 108), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustID.GoldCoin;
			AnimationFrameHeight = 36;
			DustType = ModContent.DustType<CurseDust3>();
			HitSound = SoundID.NPCHit1;
			MineResist = 0.1f;
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