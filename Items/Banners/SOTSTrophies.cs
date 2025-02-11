using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Helpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Banners
{
	public class SOTSTrophies : ModTile
	{
        public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true; // Necessary since Style3x3Wall uses AnchorWall
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(120, 85, 60), name);
		}
        public override bool CreateDust(int i, int j, ref int type)
        {
			type = 7;
			return base.CreateDust(i, j, ref type);
        }
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            SpecialPostDraw(i, j, spriteBatch); //This is just a method that runs the drawcode
        }
        public void SpecialPostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            for (int k = 0; k < 3; k++)
            SOTSTile.DrawSlopedGlowMask(i, j, -1, texture, new Color(100, 100, 100, 0), Main.rand.NextVector2Circular(1, 1) * (k* 0.15f));
            for (int k = 0; k < 7; k++)
            {
                Color color = new Color(100, 100, 100, 0);
                Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
                if (k != 0)
                {
                    color = ColorHelper.Pastel(MathHelper.ToRadians(k * 60));
                    color.A = 0;
                }
                else
                circular *= 0f;
                SOTSTile.DrawSlopedGlowMask(i, j, -1, texture, color, circular * 0.57f);
            }
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
        }
    }
	public abstract class ModTrophy : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSTrophies>();
			Item.placeStyle = 0;
		}
	}
	public class PutridPinkyTrophy : ModTrophy
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSTrophies>();
			Item.placeStyle = 0;
		}
    }
    public class CurseTrophy : ModTrophy
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSTrophies>();
            Item.placeStyle = 1;
        }
    }
    public class AdvisorTrophy : ModTrophy
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSTrophies>();
            Item.placeStyle = 2;
        }
    }
    public class GlowmothTrophy : ModTrophy
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSTrophies>();
            Item.placeStyle = 3;
        }
    }
    public class PolarisTrophy : ModTrophy
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSTrophies>();
            Item.placeStyle = 4;
        }
    }
    public class LuxTrophy : ModTrophy
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = TileType<SOTSTrophies>();
            Item.placeStyle = 5;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            //int bonus = (int)(counter / 360f);
            for (int k = 0; k < 5; k++)
            {
                Color color = new Color(100, 100, 100, 0);
                Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(k * 90 + Main.GameUpdateCount));
                if (k != 0)
                {
                    color = ColorHelper.Pastel(MathHelper.ToRadians(k * 90));
                    color.A = 0;
                }
                else
                    circular *= 0f;
                Rectangle clone = new Rectangle(0, 0, 74, 36);
                Main.spriteBatch.Draw(texture, position + (circular * 0.4f), clone, color, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
            }
        }
    }
}
