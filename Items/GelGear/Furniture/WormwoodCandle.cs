using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace SOTS.Items.GelGear.Furniture
{
	public class WormwoodCandle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Candle");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 20;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 1;
			item.value = 0;
			item.consumable = true;
			item.createTile = mod.TileType("WormwoodCandleTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Wormwood", 4);
			recipe.AddIngredient(ItemID.PinkTorch, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Wormwood", 1);
			recipe.AddIngredient(ItemID.PinkGel, 1);
			recipe.SetResult(ItemID.PinkTorch, 5);
			recipe.AddRecipe();
		}
	}	
	public class WormwoodCandleTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.CoordinateHeights = new int[]{18};
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Candle");
			AddMapEntry(new Color(200, 100, 130), name);
			disableSmartCursor = true;
			adjTiles = new int[]{TileID.Torches};
			torch = true;
			drop = mod.ItemType("WormwoodCandle");
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(Main.tile[i,j].frameX < 18)
			{
				r = 1.2f;
				g = 0.1f;
				b = 1.2f;
			}
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType("WormwoodCandle");
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = -2;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color = new Color(100, 100, 100, 0);
			int frameX = Main.tile[i, j].frameX;
			int frameY = Main.tile[i, j].frameY;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 7; k++)
			{
				float x = (float)Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
				float y = (float)Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;
				Main.spriteBatch.Draw(mod.GetTexture("Items/GelGear/Furniture/WormwoodCandleTileFlame"), 
				new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y - 2) + y) + zero, 
				new Rectangle(frameX, frameY, 16, 18), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
		}
	}
}