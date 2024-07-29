using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Planetarium.Blocks;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Planetarium.Furniture
{
	public class SkyChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
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
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AvaritianPlating>(), 4).AddIngredient(ModContent.ItemType<TwilightGel>(), 4).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}	
	public class SkyChainTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.CoordinateHeights = new int[]{18};
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(255, 255, 255), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<SkyChain>();
			DustType = ModContent.DustType<AvaritianDust>();
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(Main.tile[i,j].TileFrameX < 18)
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/Furniture/SkyChainTileGlow").Value;
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/Furniture/SkyChainHelixOutline").Value;
			Texture2D textureF = Mod.Assets.Request<Texture2D>("Items/Planetarium/Furniture/SkyChainHelixFill").Value;
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
				if ((tile2.HasTile && Main.tileSolid[tile2.TileType] && !Main.tileSolidTop[tile2.TileType] && !tile2.IsActuated) || !WorldGen.InWorld(i, j - j2, 27))
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