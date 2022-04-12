using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs;
using SOTS.NPCs.Inferno;
using SOTS.NPCs.TreasureSlimes;
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
			int item;
			switch (style)
			{
				case 0:
					item = ItemType<BlueSlimerBanner>();
					break;
				case 1:
					item = ItemType<TreasureSlimeBanner>();
					break;
				case 2:
					item = ItemType<GoldenTreasureSlimeBanner>();
					break;
				case 3:
					item = ItemType<FrozenTreasureSlimeBanner>();
					break;
				case 4:
					item = ItemType<ShadowTreasureSlimeBanner>();
					break;
				case 5:
					item = ItemType<PyramidTreasureSlimeBanner>();
					break;
				case 6:
					item = ItemType<NatureSlimeBanner>();
					break;
				case 7:
					item = ItemType<FlamingGhastBanner>();
					break;
				case 8:
					item = ItemType<BleedingGhastBanner>();
					break;
				case 9:
					item = ItemType<ArcticGoblinBanner>();
					break;
				case 10:
					item = ItemType<LostSoulBanner>();
					break;
				case 11:
					item = ItemType<SnakeBanner>();
					break;
				case 12:
					item = ItemType<SnakePotBanner>();
					break;
				case 13:
					item = ItemType<SittingMushroomBanner>();
					break;
				case 14:
					item = ItemType<WallMimicBanner>();
					break;
				default:
					return;
			}
			Item.NewItem(i * 16, j * 16, 16, 48, item);
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].frameX / 18;
				int type;
				switch (style)
				{
					case 0:
						type = NPCType<BlueSlimer>();
						break;
					case 1:
						type = NPCType<BasicTreasureSlime>();
						break;
					case 2:
						type = NPCType<GoldenTreasureSlime>();
						break;
					case 3:
						type = NPCType<IceTreasureSlime>();
						break;
					case 4:
						type = NPCType<ShadowTreasureSlime>();
						break;
					case 5:
						type = NPCType<PyramidTreasureSlime>();
						break;
					case 6:
						type = NPCType<NatureSlime>();
						break;
					case 7:
						type = NPCType<FlamingGhast>();
						break;
					case 8:
						type = NPCType<BleedingGhast>();
						break;
					case 9:
						type = NPCType<ArcticGoblin>();
						break;
					case 10:
						type = NPCType<LostSoul>();
						break;
					case 11:
						type = NPCType<Snake>();
						break;
					case 12:
						type = NPCType<SnakePot>();
						break;
					case 13:
						type = NPCType<SittingMushroom>();
						break;
					case 14:
						type = NPCType<WallMimic>();
						break;
					default:
						return;
				}
				player.NPCBannerBuff[type] = true;
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
			item.width = 20;
			item.height = 30;
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
			int item;
			switch (style)
			{
				case 0:
					item = ItemType<TwilightDevilBanner>();
					break;
				case 1:
					item = ItemType<FluxSlimeBanner>();
					break;
				case 2:
					item = ItemType<LesserWispBanner>();
					break;
				case 3:
					item = ItemType<GhastBanner>();
					break;
				case 4:
					item = ItemType<MaligmorBanner>();
					break;
				case 5:
					item = ItemType<TeratomaBanner>();
					break;
				case 6:
					item = ItemType<CorruptionTreasureSlimeBanner>();
					break;
				case 7:
					item = ItemType<CrimsonTreasureSlimeBanner>();
					break;
				case 8:
					item = ItemType<JungleTreasureSlimeBanner>();
					break;
				case 9:
					item = ItemType<TwilightScouterBanner>();
					break;
				case 10:
					item = ItemType<HallowTreasureSlimeBanner>();
					break;
				case 11:
					item = ItemType<DungeonTreasureSlimeBanner>();
					break;
				default:
					return;
			}
			Item.NewItem(i * 16, j * 16, 16, 48, item);
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].frameX / 18;
				int type;
				switch (style)
				{
					case 0:
						type = NPCType<TwilightDevil>();
						break;
					case 1:
						type = NPCType<FluxSlime>();
						break;
					case 2:
						type = NPCType<LesserWisp>();
						break;
					case 3:
						type = NPCType<Ghast>();
						break;
					case 4:
						type = NPCType<Maligmor>();
						break;
					case 5:
						type = NPCType<Teratoma>();
						break;
					case 6:
						type = NPCType<CorruptionTreasureSlime>();
						break;
					case 7:
						type = NPCType<CrimsonTreasureSlime>();
						break;
					case 8:
						type = NPCType<JungleTreasureSlime>();
						break;
					case 9:
						type = NPCType<TwilightScouter>();
						break;
					case 10:
						type = NPCType<HallowTreasureSlime>();
						break;
					case 11:
						type = NPCType<DungeonTreasureSlime>();
						break;
					default:
						return;
				}
				player.NPCBannerBuff[type] = true;
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
	public class FluxSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 1;
		}
	}
	public class LesserWispBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 2;
		}
	}
	public class GhastBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 3;
		}
	}
	public class MaligmorBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 4;
		}
	}
	public class TeratomaBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 5;
		}
	}
	public class CorruptionTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 6;
		}
	}
	public class CrimsonTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 7;
		}
	}
	public class JungleTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 8;
		}
	}
	public class TwilightScouterBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 9;
		}
	}
	public class HallowTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 10;
		}
	}
	public class DungeonTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			item.createTile = TileType<SOTSBanners2>();
			item.placeStyle = 11;
		}
	}
}