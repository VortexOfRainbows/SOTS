using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid.AncientGold
{
	public class AncientGoldSpike : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Gold Spike");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.LightRed;
			item.consumable = true;
			item.createTile = mod.TileType("AncientGoldSpikeTile");
		}
	}
	public class AncientGoldSpikeTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<RoyalGoldBrickTile>()] = true;
            Main.tileMerge[Type][ModContent.TileType<PyramidSlabTile>()] = true;
            Main.tileBrick[Type] = false;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<AncientGoldSpike>();
			AddMapEntry(new Color(150, 130, 20));
			mineResist = 2.0f;
			minPick = 0;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = DustID.GoldCoin;
        }
        public override bool Slope(int i, int j)
        {
            return false;
        }
        public static Vector2 HurtTiles(Vector2 Position, int Width, int Height)
        {
            var vector2_1 = Position;
            var num1 = (int)(Position.X / 16.0) - 1;
            var num2 = (int)((Position.X + Width) / 16.0) + 2;
            var num3 = (int)(Position.Y / 16.0) - 1;
            var num4 = (int)((Position.Y + Height) / 16.0) + 2;
            if (num1 < 0)
                num1 = 0;
            if (num2 > Main.maxTilesX)
                num2 = Main.maxTilesX;
            if (num3 < 0)
                num3 = 0;
            if (num4 > Main.maxTilesY)
                num4 = Main.maxTilesY;
            for (var i = num1; i < num2; ++i)
            {
                for (var j = num3; j < num4; ++j)
                {
                    if (Main.tile[i, j] != null && Main.tile[i, j].slope() == (byte)0 && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && Main.tile[i, j].type == ModContent.TileType<AncientGoldSpikeTile>())
                    {
                        Vector2 vector2_2;
                        vector2_2.X = (float)(i * 16);
                        vector2_2.Y = (float)(j * 16);
                        var num6 = 16;
                        if (Main.tile[i, j].halfBrick())
                        {
                            vector2_2.Y += 8f;
                            num6 -= 8;
                        }
                        if (vector2_1.X + Width >= vector2_2.X && vector2_1.X <= vector2_2.X + 16.0 && (vector2_1.Y + Height >= vector2_2.Y && vector2_1.Y <= vector2_2.Y + num6 + 11.0 / 1000.0))
                        {
                            var num7 = 1;
                            if (vector2_1.X + (Width / 2) < vector2_2.X + 8.0)
                                num7 = -1;
                            return new Vector2((float)num7, 50);
                        }
                    }
                }
            }

            return new Vector2();
        }
    }
}