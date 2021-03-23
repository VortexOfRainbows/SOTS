using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Banners
{
	public class SOTSBanners : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);
			dustType = -1;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int style = frameX / 18;
			string item;
			switch (style)
			{
				case 0:
					item = "BlueSlimerBanner";
					break;
				case 1:
					item = "TreasureSlimeBanner";
					break;
				case 2:
					item = "GoldenTreasureSlimeBanner";
					break;
				case 3:
					item = "FrozenTreasureSlimeBanner";
					break;
				case 4:
					item = "ShadowTreasureSlimeBanner";
					break;
				case 5:
					item = "PyramidTreasureSlimeBanner";
					break;
				case 6:
					item = "NatureSlimeBanner";
					break;
				case 7:
					item = "FlamingGhastBanner";
					break;
				case 8:
					item = "BleedingGhastBanner";
					break;
				case 9:
					item = "ArcticGoblinBanner";
					break;
				case 10:
					item = "LostSoulBanner";
					break;
				case 11:
					item = "SnakeBanner";
					break;
				case 12:
					item = "SnakePotBanner";
					break;
				case 13:
					item = "SittingMushroomBanner";
					break;
				case 14:
					item = "WallMimicBanner";
					break;
				default:
					return;
			}
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType(item));
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].frameX / 18;
				string type;
				switch (style)
				{
					case 0:
						type = "BlueSlimer";
						break;
					case 1:
						type = "TreasureSlime";
						break;
					case 2:
						type = "GoldenTreasureSlime";
						break;
					case 3:
						type = "FrozenTreasureSlime";
						break;
					case 4:
						type = "ShadowTreasureSlime";
						break;
					case 5:
						type = "PyramidTreasureSlime";
						break;
					case 6:
						type = "NatureSlime";
						player.NPCBannerBuff[ModContent.NPCType<BloomingHook>()] = true;
						break;
					case 7:
						type = "FlamingGhast";
						break;
					case 8:
						type = "BleedingGhast";
						break;
					case 9:
						type = "ArcticGoblin";
						break;
					case 10:
						type = "LostSoul";
						break;
					case 11:
						type = "Snake";
						break;
					case 12:
						type = "SnakePot";
						break;
					case 13:
						type = "SittingMushroom";
						break;
					case 14:
						type = "WallMimic";
						break;
					default:
						return;
				}
				player.NPCBannerBuff[mod.NPCType(type)] = true;
				player.hasBanner = true;
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
	public abstract class ModBanner : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 24;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = ItemRarityID.Blue;
			item.value = Item.buyPrice(0, 0, 10, 0);
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 0;
		}
	}
	public class BlueSlimerBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>(); 
			item.placeStyle = 0;
		}
	}
	public class TreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 1;
		}
	}
	public class GoldenTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 2;
		}
	}
	public class FrozenTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 3;
		}
	}
	public class ShadowTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 4;
		}
	}
	public class PyramidTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 5;
		}
	}
	public class NatureSlimeBanner : ModBanner
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flowering Slime Banner");
		}
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 6;
		}
	}
	public class FlamingGhastBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 7;
		}
	}
	public class BleedingGhastBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 8;
		}
	}
	public class ArcticGoblinBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 9;
		}
	}
	public class LostSoulBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 10;
		}
	}
	public class SnakeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 11;
		}
	}
	public class SnakePotBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 12;
		}
	}
	public class SittingMushroomBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 13;
		}
	}
	public class WallMimicBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners>();
			item.placeStyle = 14;
		}
	}
	public class HoloSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<HoloSlimeBannerTile>();
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Banners/HoloSlimeBannerOutline");
			Texture2D texture2 = mod.GetTexture("Items/Banners/HoloSlimeBannerFill");
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Banners/HoloSlimeBannerOutline");
			Texture2D texture2 = mod.GetTexture("Items/Banners/HoloSlimeBannerFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y + 2), null, color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
	public class HoloSwordBanner : ModBanner
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holo Blade Banner");
		}
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<HoloSwordBannerTile>();
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Banners/HoloSwordBannerOutline");
			Texture2D texture2 = mod.GetTexture("Items/Banners/HoloSwordBannerFill");
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Banners/HoloSwordBannerOutline");
			Texture2D texture2 = mod.GetTexture("Items/Banners/HoloSwordBannerFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y + 2), null, color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
	public class HoloEyeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<HoloEyeBannerTile>();
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture2 = mod.GetTexture("Items/Banners/HoloEyeBannerBase");
			Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = mod.GetTexture("Items/Banners/HoloEyeBannerBase");
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture2, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, lightColor * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Banners/HoloEyeBannerOutline");
			Texture2D texture2 = mod.GetTexture("Items/Banners/HoloEyeBannerFill");
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Texture2D texture = mod.GetTexture("Items/Banners/HoloEyeBannerOutline");
			Texture2D texture2 = mod.GetTexture("Items/Banners/HoloEyeBannerFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y + 2), null, color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
	}
	public class HoloSlimeBannerTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 1;
			TileObjectData.addTile(Type);
			dustType = -1;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			string item;
			item = "HoloSlimeBanner";
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType(item));
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				string type;
				type = "HoloSlime";
				player.NPCBannerBuff[mod.NPCType(type)] = true;
				player.hasBanner = true;
			}
		}
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D texture = mod.GetTexture("Items/Banners/HoloSlimeBannerTileFill");
			Texture2D texture2 = mod.GetTexture("Items/Banners/HoloSlimeBannerTileOutline");
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color = new Color(90, 90, 90, 0);
			int frameX = Main.tile[i, j].frameX;
			int frameY = Main.tile[i, j].frameY;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.05f;
				float y = Main.rand.Next(-10, 11) * 0.05f;
				if(k <= 2)
                {
					x = 0;
					y = 0;
				}
				if (k == 0)
					Main.spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y) + zero, new Rectangle(frameX, frameY, 16, 16), color * 0.5f, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture2, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y) + zero, new Rectangle(frameX, frameY, 16, 16), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
		}
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
	public class HoloSwordBannerTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 1;
			TileObjectData.addTile(Type);
			dustType = -1;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			string item;
			item = "HoloSwordBanner";
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType(item));
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				string type;
				type = "HoloBlade";
				player.NPCBannerBuff[mod.NPCType(type)] = true;
				player.hasBanner = true;
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			return false;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D texture = mod.GetTexture("Items/Banners/HoloSwordBannerTileFill");
			Texture2D texture2 = mod.GetTexture("Items/Banners/HoloSwordBannerTileOutline");
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color = new Color(90, 90, 90, 0);
			int frameX = Main.tile[i, j].frameX;
			int frameY = Main.tile[i, j].frameY;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.05f;
				float y = Main.rand.Next(-10, 11) * 0.05f;
				if (k <= 2)
				{
					x = 0;
					y = 0;
				}
				if (k == 0)
					Main.spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y) + zero, new Rectangle(frameX, frameY, 16, 16), color * 0.5f, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture2, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y) + zero, new Rectangle(frameX, frameY, 16, 16), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
		}
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
	public class HoloEyeBannerTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 1;
			TileObjectData.addTile(Type);
			dustType = -1;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			string item;
			item = "HoloEyeBanner";
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType(item));
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				string type;
				type = "HoloEye";
				player.NPCBannerBuff[mod.NPCType(type)] = true;
				player.hasBanner = true;
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (Main.tile[i, j].frameX != 0 || Main.tile[i, j].frameY != 0)
			{
				return true;
			}
			Texture2D texture = mod.GetTexture("Items/Banners/HoloEyeBannerTileFill");
			Texture2D texture2 = mod.GetTexture("Items/Banners/HoloEyeBannerTileOutline");
			Texture2D texture3 = mod.GetTexture("Items/Banners/HoloEyeBannerTilePupil");
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color = new Color(90, 90, 90, 0);
			int frameX = Main.tile[i, j].frameX;
			int frameY = Main.tile[i, j].frameY;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.05f;
				float y = Main.rand.Next(-10, 11) * 0.05f;

				Vector2 between = Main.LocalPlayer.Center - new Vector2(i * 16 + 8, j * 16 + 40);
				between = between.SafeNormalize(Vector2.Zero) * 1.75f;
				if (k <= 2)
				{
					x = 0;
					y = 0;
				}
				x -= 2;
				y -= 2;
				if (k == 0)
					Main.spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y) + zero, null, color * 0.5f, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture2, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y) + zero, null, color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);

				x += 10;
				y += 40;
				Main.spriteBatch.Draw(texture3, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y) + zero + between, null, color, 0f, new Vector2(4, 4), 1f, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
	public class SOTSBanners2 : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);
			dustType = -1;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int style = frameX / 18;
			string item;
			switch (style)
			{
				case 0:
					item = "TwilightDevilBanner";
					break;
				default:
					return;
			}
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType(item));
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].frameX / 18;
				string type;
				switch (style)
				{
					case 0:
						type = "TwilightDevil";
						break;
					default:
						return;
				}
				player.NPCBannerBuff[mod.NPCType(type)] = true;
				player.hasBanner = true;
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
	public class TwilightDevilBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 0;
		}
	}
}