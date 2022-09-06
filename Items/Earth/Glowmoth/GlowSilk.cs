using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth.Glowmoth
{
	public class GlowSilk : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glow Silk Block");
			Tooltip.SetDefault("");
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Blue;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<GlowSilkTile>();
		}
	}
	public class GlowSilkTile : ModTile
	{
		private static bool FramingPreventRepitions = false;
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			if (!FramingPreventRepitions)
			{
				FramingPreventRepitions = true;
				try
				{
					WorldGen.TileFrame(i, j, resetFrame, noBreak);
					if (Main.tile[i, j].TileFrameY < 90 && WorldGen.genRand.NextBool(5))
					{
						Main.tile[i, j].TileFrameY += 90;
					}
					FramingPreventRepitions = false;
					return false;
				}
				catch
				{

				}
				FramingPreventRepitions = false;
			}
			return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(Main.tile[i, j].TileFrameY >= 90)
			{
				r = 0.2f;
				g = 0.4f;
				b = 0.6f;
			}
			else
            {
				r = 0;
				g = 0;
				b = 0;
            }
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Earth/Glowmoth/GlowSilkTileGlow").Value;
			if (Main.tile[i, j].TileFrameY >= 90)
				SOTSTile.DrawSlopedGlowMask(i, j, Type, texture, Color.White, Vector2.Zero, false);
		}
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			ItemDrop = ModContent.ItemType<GlowSilk>();
			AddMapEntry(new Color(93, 99, 144));
			MineResist = 1.0f;
			HitSound = SoundID.Dig;
			DustType = DustID.Silk;
		}
	}
}