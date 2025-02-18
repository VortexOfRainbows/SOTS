using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs;
using SOTS.NPCs.AbandonedVillage;
using SOTS.NPCs.Anomaly;
using SOTS.NPCs.Chaos;
using SOTS.NPCs.Gizmos;
using SOTS.NPCs.Inferno;
using SOTS.NPCs.Phase;
using SOTS.NPCs.Tide;
using SOTS.NPCs.TreasureSlimes;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Banners
{
    public class SOTSBanners : ModBannerTile
	{
		public override void NearbyEffects(int i, int j, bool closer)
		{
                if (closer)
                {
                    return;
                }
                // Calculate the tile place style, then map that place style to an ItemID and BannerID.
                int tileStyle = TileObjectData.GetTileStyle(Main.tile[i, j]);
                int itemType = TileLoader.GetItemDropFromTypeAndStyle(Type, tileStyle);
                int bannerID = NPCLoader.BannerItemToNPC(itemType);
                if (bannerID == -1)
                {
                    return;
                }
                // Once the BannerID and Item type have been calculated, we apply the banner buff
                if (ItemID.Sets.BannerStrength.IndexInRange(itemType) && ItemID.Sets.BannerStrength[itemType].Enabled)
                {
                    Main.SceneMetrics.NPCBannerBuff[bannerID] = true;
                    Main.SceneMetrics.hasBanner = true;
                }
            if (bannerID == NPCType<PhaseAssaulterHead>()) {
                Main.SceneMetrics.NPCBannerBuff[NPCType<PhaseAssaulterBody>()] = true;
                Main.SceneMetrics.NPCBannerBuff[NPCType<PhaseAssaulterTail>()] = true;
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
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 0, 10, 0);
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 0;
		}
	}
	public class BlueSlimerBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>(); 
			Item.placeStyle = 0;
		}
	}
	public class TreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 1;
		}
	}
	public class GoldenTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 2;
		}
	}
	public class FrozenTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 3;
		}
	}
	public class ShadowTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 4;
		}
	}
	public class PyramidTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 5;
		}
	}
	public class NatureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 6;
		}
	}
	public class FlamingGhastBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 7;
		}
	}
	public class BleedingGhastBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 8;
		}
	}
	public class ArcticGoblinBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 9;
		}
	}
	public class LostSoulBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 10;
		}
	}
	public class SnakeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 11;
		}
	}
	public class SnakePotBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 12;
		}
	}
	public class SittingMushroomBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 13;
		}
	}
	public class WallMimicBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 14;
		}
	}
	public class TwilightDevilBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 15;
		}
	}
	public class FluxSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 16;
		}
	}
	public class LesserWispBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 17;
		}
	}
	public class GhastBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 18;
		}
	}
	public class MaligmorBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 19;
		}
	}
	public class TeratomaBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 20;
		}
	}
	public class CorruptionTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 21;
		}
	}
	public class CrimsonTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 22;
		}
	}
	public class JungleTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 23;
		}
	}
	public class TwilightScouterBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 24;
		}
	}
	public class HallowTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 25;
		}
	}
	public class DungeonTreasureSlimeBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 26;
		}
	}
	public class PhaseSpeederBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 27;
		}
	}
	public class PhaseAssaulterBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 28;
		}
	}
	public class UltracapBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 29;
		}
	}
	public class PlanetoidBanner : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSBanners>();
			Item.placeStyle = 30;
		}
    }
    public class SmallPhantarayBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 31;
        }
    }
    public class BigPhantarayBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 32;
        }
    }
    public class ChimeraBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 33;
        }
    }
    public class CorpsebloomBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 34;
        }
    }
    public class EarthenGizmoBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 35;
        }
    }
    public class ThroeBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 36;
        }
    }
    public class FamishedBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 37;
        }
    }
    public class BallOWormsBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 38;
        }
    }
    public class BallOGutsBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 39;
        }
    }
    public class CowBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 40;
        }
    }
    public class FistfullBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 41;
        }
    }
    public class MutagenTreasureSlimeBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSBanners>();
            Item.placeStyle = 42;
        }
    }
    public class HoloSlimeBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<HoloSlimeBannerTile>();
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSlimeBannerOutline").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSlimeBannerFill").Value;
            Color color = new Color(110, 110, 110, 0);
            for (int k = 0; k < 5; k++)
            {
                float x = Main.rand.Next(-10, 11) * 0.03f;
                float y = Main.rand.Next(-10, 11) * 0.03f;
                if (k == 0)
                    Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSlimeBannerOutline").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSlimeBannerFill").Value;
            Color color = new Color(110, 110, 110, 0);
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            for (int k = 0; k < 5; k++)
            {
                float x = Main.rand.Next(-10, 11) * 0.03f;
                float y = Main.rand.Next(-10, 11) * 0.03f;
                if (k == 0)
                    Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
    public class HoloSwordBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<HoloSwordBannerTile>();
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSwordBannerOutline").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSwordBannerFill").Value;
            Color color = new Color(110, 110, 110, 0);
            for (int k = 0; k < 5; k++)
            {
                float x = Main.rand.Next(-10, 11) * 0.03f;
                float y = Main.rand.Next(-10, 11) * 0.03f;
                if (k == 0)
                    Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSwordBannerOutline").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSwordBannerFill").Value;
            Color color = new Color(110, 110, 110, 0);
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            for (int k = 0; k < 5; k++)
            {
                float x = Main.rand.Next(-10, 11) * 0.03f;
                float y = Main.rand.Next(-10, 11) * 0.03f;
                if (k == 0)
                    Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
    public class HoloEyeBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<HoloEyeBannerTile>();
            Item.width = 20;
            Item.height = 30;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloEyeBannerBase").Value;
            Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloEyeBannerBase").Value;
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, lightColor * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Banners/HoloEyeBannerOutline").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloEyeBannerFill").Value;
            Color color = new Color(110, 110, 110, 0);
            for (int k = 0; k < 5; k++)
            {
                float x = Main.rand.Next(-10, 11) * 0.03f;
                float y = Main.rand.Next(-10, 11) * 0.03f;
                if (k == 0)
                    Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

                Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Banners/HoloEyeBannerOutline").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloEyeBannerFill").Value;
            Color color = new Color(110, 110, 110, 0);
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            for (int k = 0; k < 5; k++)
            {
                float x = Main.rand.Next(-10, 11) * 0.03f;
                float y = Main.rand.Next(-10, 11) * 0.03f;
                if (k == 0)
                    Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

                Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }
        }
    }
    public class HoloSlimeBannerTile : ModBannerTile
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                return;
            }
            // Calculate the tile place style, then map that place style to an ItemID and BannerID.
            int tileStyle = TileObjectData.GetTileStyle(Main.tile[i, j]);
            int itemType = TileLoader.GetItemDropFromTypeAndStyle(Type, tileStyle);
            int bannerID = NPCLoader.BannerItemToNPC(itemType);
            if (bannerID == -1)
            {
                return;
            }
            // Once the BannerID and Item type have been calculated, we apply the banner buff
            if (ItemID.Sets.BannerStrength.IndexInRange(itemType) && ItemID.Sets.BannerStrength[itemType].Enabled)
            {
                Main.SceneMetrics.NPCBannerBuff[bannerID] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSlimeBannerTileFill").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSlimeBannerTileOutline").Value;
            Color color = new Color(90, 90, 90, 0);
            int frameX = Main.tile[i, j].TileFrameX;
            int frameY = Main.tile[i, j].TileFrameY;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            zero.Y -= 2;
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
    public class HoloSwordBannerTile : ModBannerTile
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                return;
            }
            // Calculate the tile place style, then map that place style to an ItemID and BannerID.
            int tileStyle = TileObjectData.GetTileStyle(Main.tile[i, j]);
            int itemType = TileLoader.GetItemDropFromTypeAndStyle(Type, tileStyle);
            int bannerID = NPCLoader.BannerItemToNPC(itemType);
            if (bannerID == -1)
            {
                return;
            }
            // Once the BannerID and Item type have been calculated, we apply the banner buff
            if (ItemID.Sets.BannerStrength.IndexInRange(itemType) && ItemID.Sets.BannerStrength[itemType].Enabled)
            {
                Main.SceneMetrics.NPCBannerBuff[bannerID] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSwordBannerTileFill").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloSwordBannerTileOutline").Value;
            Color color = new Color(90, 90, 90, 0);
            int frameX = Main.tile[i, j].TileFrameX;
            int frameY = Main.tile[i, j].TileFrameY;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            zero.Y -= 2;
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
    public class HoloEyeBannerTile : ModBannerTile
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                return;
            }
            // Calculate the tile place style, then map that place style to an ItemID and BannerID.
            int tileStyle = TileObjectData.GetTileStyle(Main.tile[i, j]);
            int itemType = TileLoader.GetItemDropFromTypeAndStyle(Type, tileStyle);
            int bannerID = NPCLoader.BannerItemToNPC(itemType);
            if (bannerID == -1)
            {
                return;
            }
            // Once the BannerID and Item type have been calculated, we apply the banner buff
            if (ItemID.Sets.BannerStrength.IndexInRange(itemType) && ItemID.Sets.BannerStrength[itemType].Enabled)
            {
                Main.SceneMetrics.NPCBannerBuff[bannerID] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.tile[i, j].TileFrameX != 0 || Main.tile[i, j].TileFrameY != 0)
            {
                return true;
            }
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Banners/HoloEyeBannerTileFill").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloEyeBannerTileOutline").Value;
            Texture2D texture3 = Mod.Assets.Request<Texture2D>("Items/Banners/HoloEyeBannerTilePupil").Value;
            Color color = new Color(90, 90, 90, 0);
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            zero.Y -= 2;
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
}