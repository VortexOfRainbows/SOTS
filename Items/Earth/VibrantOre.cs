using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Otherworld.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth
{
	public class VibrantOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Shard");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.width = 20;
			item.height = 18;
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<VibrantOreTile>();
		}
	}
	public class VibrantOreTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileShine[Type] = 200;
			Main.tileShine2[Type] = true;
			Main.tileValue[Type] = 420; //above gold
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<VibrantOre>();
			AddMapEntry(new Color(113, 151, 34));
			mineResist = 1.6f;
			minPick = 40; //no copper/tin pickaxe!
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = 44;
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			r = 0.09f;
			g = 0.11f;
			b = 0.05f;
            base.ModifyLight(i, j, ref r, ref g, ref b);
        }
        /*public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			SOTSTile.DrawSlopedGlowMask(i, j, Main.tile[i, j].type, ModContent.GetTexture("SOTS/Items/Earth/VibrantOreTileGlow"), new Color(70, 80, 70, 0));
		}*/
	}
}