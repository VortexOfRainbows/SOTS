using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Otherworld.Blocks;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Otherworld.Furniture
{
	public class SkyChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sky Chain");
			Tooltip.SetDefault("'It anchors onto... somewhere'");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.LightRed;
			Item.width = 16;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<SkyChainTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AvaritianPlating>(), 4);
			recipe.AddIngredient(ModContent.ItemType<TwilightGel>(), 4);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
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
			drop = ModContent.ItemType<SkyChain>();
			DustType = ModContent.DustType<AvaritianDust>();
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(Main.tile[i,j].frameX < 18)
			{
				r = 0.425f;
				g = 0.425f;
				b = 0.425f;
			}
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/SkyChainTileGlow").Value;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 5; k++)
			{
				Vector2 pos = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.10f * k;
				Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * 0.75f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/SkyChainHelixOutline").Value;
			Texture2D textureF = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/SkyChainHelixFill").Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float height = 16;
			float timer = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int maxLength = 20;
			for(int j2 = 1; j2 < maxLength; j2++)
			{
				Tile tile2 = Framing.GetTileSafely(i, j - j2);
				if ((tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type]) || !WorldGen.InWorld(i, j - j2, 27))
				{
					maxLength = j2;
					break;
				}
			}
			maxLength++;
			Vector2 previous = new Vector2(i * 16 + 8, j * 16 + 16);
			for (int z = 0; z < maxLength; z ++)
			{
				float dynamicMult = 0.52f + 0.48f * (float)Math.Cos(MathHelper.ToRadians(180f * z / maxLength));
				Vector2 dynamicAddition = new Vector2(20f / maxLength * z * 0.4f + 0.5f, 0).RotatedBy(MathHelper.ToRadians(z * 24 + timer)) * dynamicMult;
				Vector2 pos = new Vector2(i * 16 + 8, j * 16 + 8);
				pos.Y -= z * 16;
				pos += dynamicAddition;
				Vector2 rotateTo = pos - previous;
				float lengthTo = rotateTo.Length();
				float stretch = (lengthTo / height) * 1.00275f;
				if (z == 0)
					stretch = 1f;
				Vector2 scaleVector2 = new Vector2(0.8f, stretch);
				if (z != 0)
				{
					float alphaScale = (32f - z * 1.575f) / 20f;
					for (int k = 0; k < 5; k++)
					{
						if (k == 0)
							Main.spriteBatch.Draw(textureF, previous + zero - Main.screenPosition, null, color * alphaScale * 0.575f, rotateTo.ToRotation() + MathHelper.ToRadians(90), origin, scaleVector2, SpriteEffects.None, 0f);
						Main.spriteBatch.Draw(texture, previous + zero - Main.screenPosition, null, color * alphaScale, rotateTo.ToRotation() + MathHelper.ToRadians(90), origin, scaleVector2, SpriteEffects.None, 0f);
					}
				}
				previous = pos;
			}
			return true;
		}
	}
}