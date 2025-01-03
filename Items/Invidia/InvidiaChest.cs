using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Items.Furniture;
using SOTS.Projectiles.Chaos;
using System;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Invidia
{
	public class InvidiaChest : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 32;
			Item.height = 30;
			Item.maxStack = 9999;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<InvidiaChestTile>();
			Item.placeStyle = 3;
		}
        public override bool? UseItem(Player player)
        {
            Item.useAnimation = Item.useTime;
            Item.placeStyle = ++Item.placeStyle % 10;
            return base.UseItem(player);
        }
    }
	public class InvidiaChestTile : ContainerType
	{
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            SpecialPostDraw(i, j, spriteBatch);
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.08f;
			g = 0.2f;
			b = 0.1f;
		}
		private void SpecialPostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Invidia/InvidiaChestTileGlow").Value;
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX % 36 == 0 && tile.TileFrameY % 38 == 0)
                DrawChains(i, j, spriteBatch, true);
            int left = i;
            int top = j;
            int chestType = tile.TileFrameX / 36;
            if (tile.TileFrameX % 36 != 0)
                left--;
            if (tile.TileFrameY % 38 != 0)
                top--;
            int chestI = Chest.FindChest(left, top);
            if (chestI < 0)
                return;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
                zero = Vector2.Zero;
            Color lightColor = Lighting.GetColor(i, j);
            Chest chest = Main.chest[chestI];
            int cFrame = chest.frame;
            Rectangle frame = new Rectangle(tile.TileFrameX, 38 * cFrame + tile.TileFrameY, 16, 18);
            Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
            float percent = chestType == 0 || chestType == 6 ? SOTSWorld.MoonPhasePercent : SOTSWorld.SantuaryMoonPhase(chestType + 2);
            float fillPercent = percent * percent * 0.9f + 0.1f * SOTSTile.PlanetariumLightingColorMultiplier(i, j) * percent;
            Color runeColor = Color.Lerp(lightColor, Color.White, fillPercent) * fillPercent;
            runeColor.A = 0;
            int c = SOTS.Config.lowFidelityMode ? 3 : 6;
            int d = SOTS.Config.lowFidelityMode ? 120 : 60;
            spriteBatch.Draw(texture, pos, frame, Color.Lerp(lightColor, Color.Black, 0.25f - 0.25f * percent), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            for (int a = 0; a < c; a++)
            {
                spriteBatch.Draw(texture, pos + new Vector2(.25f + 0.25f * percent, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter + a * d)), 
                    frame, runeColor * 0.23f * fillPercent, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(texture, pos,
                frame, runeColor * 0.5f * fillPercent, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {

        }
        private void DrawChains(int x, int y, SpriteBatch spriteBatch, bool front = true)
        {
            Tile tile = Main.tile[x, y];
            int chestType = tile.TileFrameX / 36;
            if (chestType <= 1)
                return;
            float percent = chestType == 0 || chestType == 6 ? SOTSWorld.MoonPhasePercent : SOTSWorld.SantuaryMoonPhase(chestType + 2) / 0.96f;
            int left = x - tile.TileFrameX / 18 % 2;
            int top = y - tile.TileFrameY / 18 % 2;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            float completionPercent = MathF.Max(0, 1.0f - percent * percent * percent * percent * percent);
            Color color = new Color(11, 142, 50, 0) * completionPercent;
            Vector2 pos = new Vector2(left * 16 - (int)Main.screenPosition.X, top * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(16, 14);
            Texture2D texture = SOTSUtils.WhitePixel;
            Vector2 drawOrigin = new Vector2(0, texture.Height * 0.5f);
            int startEnd = 0;
            if (front)
                startEnd = 180;
            int increment = 5;
            for(int a = 0; a < 2; a++)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    for (int i = startEnd; i < startEnd + 180; i += increment)
                    {
                        float size = 0.6f - 0.2f * MathF.Sin(MathF.PI / 180f * i) + 0.2f * completionPercent;
                        Vector2 CP = circlePos(i + 90, j, a, size);
                        Vector2 nextCP = circlePos(i + 90 + increment, j, a, size);
                        Vector2 toNext = nextCP - CP;
                        spriteBatch.Draw(texture, pos + CP, null, color * (1 - 0.5f * MathF.Sin(MathF.PI / 180f * i)), toNext.ToRotation(), drawOrigin, new Vector2(toNext.Length() / 2, size), SpriteEffects.None, 0f);
                    }
                }
            }
        }
        private Vector2 circlePos(int i, int j, int a, float size = 1)
        {
            float speedM = 1f;
            float sin = MathF.Sin(i * MathF.PI / 30f + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 1.5f));
            float jOffset = j * sin * MathF.Abs(sin);
            float radians = MathHelper.ToRadians(i);
            float sinusoid = 0.3f + 0.1f * MathF.Sin(MathF.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter * speedM)));
            Vector2 circular = new Vector2(28 + jOffset * 4, 0).RotatedBy(radians) * size;
            circular.X *= sinusoid;
            circular += new Vector2(jOffset * 2, 0).RotatedBy(radians) * size;
            circular = circular.RotatedBy(MathHelper.ToRadians(50 + 4 * MathF.Sin(SOTSWorld.GlobalCounter * MathF.PI / 180f + a)));
            if (a == 1)
                circular.X *= -1;
            return circular;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            if (tile.TileFrameX % 36 == 0 && tile.TileFrameY % 38 == 0)
            {
                DrawChains(i, j, spriteBatch, false);
            }
            return true;
        }
        protected override int ChestDrop => ModContent.ItemType<InvidiaChest>();
		protected override int ChestKey => ItemID.MoonGlobe; //Temporary
		protected override int DustType => ModContent.DustType<EvostoneDust>();
		protected override void AddMapEntires()
		{
			AddMapEntry(new Color(86, 226, 100), this.GetLocalization("MapEntry0"), MapChestName);
            AddMapEntry(new Color(86, 226, 100), this.GetLocalization("MapEntry1"), MapChestName);
            AddMapEntry(new Color(86, 226, 100), this.GetLocalization("MapEntry2"), MapChestName);
            AddMapEntry(new Color(86, 226, 100), this.GetLocalization("MapEntry3"), MapChestName);
            AddMapEntry(new Color(86, 226, 100), this.GetLocalization("MapEntry4"), MapChestName);
            AddMapEntry(new Color(86, 226, 100), this.GetLocalization("MapEntry5"), MapChestName);
            AddMapEntry(new Color(86, 226, 100), this.GetLocalization("MapEntry6"), MapChestName);
            AddMapEntry(new Color(86, 226, 100), this.GetLocalization("MapEntry7"), MapChestName);
            AddMapEntry(new Color(86, 226, 100), this.GetLocalization("MapEntry8"), MapChestName);
            AddMapEntry(new Color(86, 226, 100), this.GetLocalization("MapEntry9"), MapChestName);
        }
        public override bool IsLockedChest(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.TileFrameX >= 36;
        }
        public bool CanUnlockChest(int i, int j, ref int dustType)
        {
            Tile tile = Main.tile[i, j];
            int chestType = tile.TileFrameX / 36;
            if (chestType >= 2)
            {
                return SOTSWorld.SantuaryMoonPhase(chestType + 2) > 0.96f; //This gives like 5 minutes around midnight to open each chest
            }
            return true;
        }
        public override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
        {
            Tile tile = Main.tile[i, j];
            frameXAdjustment = (short)(tile.TileFrameX / 36 * -36);
            return CanUnlockChest(i, j, ref dustType);
        }
    }
}