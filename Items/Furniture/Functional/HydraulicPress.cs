using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs.Boss;
using SOTS.Projectiles.Blades;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Functional
{
	public class HydraulicPress : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 48;
			Item.height = 58;
			Item.rare = ItemRarityID.Orange;
			Item.createTile = ModContent.TileType<HydraulicPressTile>();
		}
	}	
	public class HydraulicPressTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileObsidianKill[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.Width = 6;
			TileObjectData.newTile.Height = 8;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 6, 0);
			TileObjectData.newTile.Origin = new Point16(3, 7);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(SOTSTile.EarthenPlatingColor, name);
			DustType = DustID.Iron;
			MineResist = 0.1f;
		}
        public override void NearbyEffects(int i, int j, bool closer)
        {
			if(closer)
            {
				Tile tile = Main.tile[i, j];
				int left = i - tile.TileFrameX / 18;
				int top = j - tile.TileFrameY / 18;
				if((tile.TileFrameX == 0 || tile.TileFrameX == 6 * 18) && tile.TileFrameY == 0)
                {
					for(int k = 0; k < 6; k++)
                    {
						for(int h = 0; h < 8; h++)
                        {
							int trueI = left + k;
							int trueJ = top + h;
							tile = Main.tile[trueI, trueJ];
							if(h >= 4 && h <= 6)
                            {
								if(!tile.IsActuated)
                                {
									tile.IsActuated = true;
									NetMessage.SendTileSquare(Main.myPlayer, trueI, trueJ, 1);
								}
							}
							else if(tile.IsActuated)
                            {
								tile.IsActuated = false;
								NetMessage.SendTileSquare(Main.myPlayer, trueI, trueJ, 1);
							}
                        }
                    }
                }
			}
        }
        public override bool CanExplode(int i, int j)
        {
            return true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return true;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16 + 16, j * 16 + 16, 64, 96, ModContent.ItemType<HydraulicPress>());
		}
    }
}