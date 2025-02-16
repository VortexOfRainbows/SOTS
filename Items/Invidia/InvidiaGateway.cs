using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Invidia
{
	public class InvidiaGateway : ModItem
	{
		public override string Texture => "SOTS/Items/Planetarium/AvaritianGateway";
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 42;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.consumable = true;
			Item.createTile = ModContent.TileType<InvidiaGatewayTile>();
		}
	}	
	public class InvidiaGatewayTile : ModTile
	{
		public override void SetStaticDefaults()
		{
            Main.tileObsidianKill[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.Height = 27;
			TileObjectData.newTile.Width = 29;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = 
				[ 16, 16, 16, 16, 16, 16, 16, 16, 16, 16,
				  16, 16, 16, 16, 16, 16, 16, 16, 16, 16,
				  16, 16, 16, 16, 16, 16, 18 ];
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0); 
			TileObjectData.newTile.Origin = new Point16(14, 26);
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(31, 39, 57), name);
			DustType = ModContent.DustType<EvostoneDust>();
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
		/*public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/AvaritianGatewayTileGlow").Value;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (50f / 255f);
			color.A = 0;
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 5; k++)
			{
				Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero;
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.10f * k;
				Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}*/
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			//int type = Main.tile[i, j].TileFrameX / 18 + (Main.tile[i, j].TileFrameX / 18 * 9);
			//if (type != 67)
			//	return;
			//r = 1.1f;
			//g = 1.2f;
			//b = 1.3f;
		}
	}
}