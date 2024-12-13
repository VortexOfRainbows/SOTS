using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs.Boss;
using SOTS.Projectiles.Blades;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.AbandonedVillage
{
	public class PinkyTestTubeItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 26;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<PinkyTestTube>();
		}
	}	
	public class PinkyTestTube : ModTile
	{
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int frameX = Main.tile[i, j].TileFrameX / 18;
            int frameY = Main.tile[i, j].TileFrameY / 18;
			if((frameX == 7 || frameX == 8) && frameY >= 3 && frameY <= 6)
            {
                Color lighting = Lighting.GetColor(i, j);
                SOTSTile.DrawSlopedGlowMask(i, j, Type, ModContent.Request<Texture2D>("SOTS/Items/AbandonedVillage/PinkyTestTubeGlass1").Value, lighting * 0.27058f, new Vector2(0, 2));
                SOTSTile.DrawSlopedGlowMask(i, j, Type, ModContent.Request<Texture2D>("SOTS/Items/AbandonedVillage/PinkyTestTubeGlass2").Value, lighting * 0.62745f, new Vector2(0, 2));
            }
        }
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
			TileObjectData.newTile.Width = 10;
			TileObjectData.newTile.Height = 8;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 8, 0);
			TileObjectData.newTile.Origin = new Point16(5, 7);
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(75, 57, 86), name);
			MinPick = 250;
			DustType = DustID.Lead;
			HitSound = SoundID.Tink;
			MineResist = 0.1f;
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
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if((frameX == 1 || frameX == 2) && frameY == 6)
            {
                r = 0.5f;
                g = 0.75f;
                b = 0.75f;
            }
		}
    }
}