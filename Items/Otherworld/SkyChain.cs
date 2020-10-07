using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace SOTS.Items.Otherworld
{
	public class SkyChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sky Chain");
			Tooltip.SetDefault("'It anchors into... somewhere'");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 10;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 9;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.consumable = true;
			item.createTile = mod.TileType("SkyChainTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AvaritianPlating", 4);
			recipe.AddIngredient(null, "TwilightGel", 4);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}	
	public class SkyChainTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.CoordinateHeights = new int[]{18};
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Sky Chain");
			AddMapEntry(new Color(255, 255, 255), name);
			disableSmartCursor = true;
			drop = mod.ItemType("SkyChain");
			dustType = mod.DustType("AvaritianDust");
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(Main.tile[i,j].frameX < 18)
			{
				r = 1.2f;
				g = 1.2f;
				b = 1.2f;
			}
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/SkyChainEffect");
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].color()) * (100f / 255f);
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for(int z = 0; z < 20; z ++)
			{
				Vector2 dynamicAddition = new Vector2(z * 0.3f + 0.6f, 0).RotatedBy(MathHelper.ToRadians(z * 24 + Main.GlobalTime * 40));
				for (int k = 0; k < 3; k++)
				{
					Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
					pos.Y -= z * 17 + 8;
					pos += dynamicAddition;

					int j2 = j * 16 - z * 18 - 8;
					Tile tile2 = Framing.GetTileSafely(i, j2/16);
					if((!tile2.active() || (!Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type])) && WorldGen.InWorld(i, j2 / 16, 27))
					{
						Main.spriteBatch.Draw(texture, pos, null, color * ((20f - z) / 20f), 0f, new Vector2(8, 8), 1f, SpriteEffects.None, 0f);
					}
					else
					{
						return;
					}
				}
			}
		}
	}
}