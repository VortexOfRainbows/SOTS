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
			item.width = 74;
			item.height = 66;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.Purple;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.consumable = true;
			item.createTile = mod.TileType("AcediaGatewayTile");
		}
	}	
	public class AcediaGatewayTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.Height = 8;
			TileObjectData.newTile.Width = 9;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 5, 2); 
			TileObjectData.newTile.Origin = new Point16(4, 7);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Strange Gateway");
			AddMapEntry(new Color(44, 12, 62), name);
			disableSmartCursor = true;
			dustType = mod.DustType("AcedianDust");
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
			int drop = mod.ItemType("AcediaGateway");
			Item.NewItem(i * 16, j * 16, 128, 144, drop);
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			int type = Main.tile[i, j].frameX / 18 + (Main.tile[i, j].frameY / 18 * 9);
			if (type != 58)
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