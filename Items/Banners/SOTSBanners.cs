using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
}