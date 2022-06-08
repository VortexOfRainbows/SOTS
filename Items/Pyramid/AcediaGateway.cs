using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid
{
	public class AcediaGateway : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acedia Gateway");
			Tooltip.SetDefault("'A strange portal that leads nowhere'");
		}
		public override void SetDefaults()
		{
			Item.width = 74;
			Item.height = 66;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Purple;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("AcediaGatewayTile").Type;
		}
	}	
	public class AcediaGatewayTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.Height = 9;
			TileObjectData.newTile.Width = 9;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 5, 2);
			TileObjectData.newTile.Origin = new Point16(4, 8);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Strange Gateway");
			AddMapEntry(new Color(44, 12, 62), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = Mod.Find<ModDust>("AcedianDust").Type;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			//float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Pyramid/AcediaGatewayTileGlow").Value;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.1f; // + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 2; k++)
			{
				Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.1f * k;
				Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * 1f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return false;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = ModContent.ItemType<AcediaGateway>();
			Item.NewItem(i * 16, j * 16, 144, 144, drop);
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			int type = Main.tile[i, j].TileFrameX / 18 + (Main.tile[i, j].TileFrameY / 18 * 9);
			if (type != 67)
				return;

			r = 1.1f;
			g = 0.5f;
			b = 1.1f;

			r *= 0.25f;
			b *= 0.25f;
			g *= 0.25f;
		}
	}
}