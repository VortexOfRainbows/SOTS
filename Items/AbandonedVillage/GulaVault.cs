using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class GulaVault : ModItem
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
			Item.createTile = ModContent.TileType<GulaVaultTile>();
			Item.placeStyle = 3;
		}
	}
	public class GulaVaultTile : ContainerType
	{
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.2f;
			g = 0.04f;
			b = 0.02f;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }
            if (tile.TileFrameY % 38 != 0)
            {
                top--;
            }
            int chestI = Chest.FindChest(left, top);
            if (chestI < 0)
                return;
            Chest chest = Main.chest[chestI];
            int cFrame = chest.frame;
            float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/AbandonedVillage/GulaVaultTileGlow").Value;
            Rectangle frame = new Rectangle(tile.TileFrameX, 38 * cFrame + tile.TileFrameY, 16, 18);
            Color color;
            color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
            color.A = 0;
            float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
            //Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Items/AbandonedVillage/GulaVaultTile").Value, pos, frame, Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            for (int k = 0; k < 3; k++)
            {
                Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.10f * k;
                Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * 0.6f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return true;
        }
        protected override int ChestDrop => ModContent.ItemType<GulaVault>();
		protected override int ChestKey => ModContent.ItemType<OldKey>();
		protected override int DustType => ModContent.DustType<GulaDust>();
		protected override void AddMapEntires()
		{
			AddMapEntry(new Color(240, 80, 40), this.GetLocalization("MapEntry0"), MapChestName);
			AddMapEntry(new Color(240, 80, 40), this.GetLocalization("MapEntry1"), MapChestName);
            AddMapEntry(new Color(240, 80, 40), this.GetLocalization("MapEntry2"), MapChestName);
            AddMapEntry(new Color(240, 80, 40), this.GetLocalization("MapEntry3"), MapChestName);
        }
        public override bool IsLockedChest(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.TileFrameX >= 36;
        }
    }
}