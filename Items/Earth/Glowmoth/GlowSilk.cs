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
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			int rand = i + j;
			if(rand % 2 == 0)
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
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			int rand = i + j;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Earth/Glowmoth/GlowSilkTileAlt").Value;
			if (rand % 2 == 0)
			{
				SOTSTile.DrawSlopedGlowMask(i, j, Type, texture, Lighting.GetColor(i, j), Vector2.Zero, false);
				return false;
            }
			return base.PreDraw(i, j, spriteBatch);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			int rand = i + j;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Earth/Glowmoth/GlowSilkTileGlow").Value;
			if (rand % 2 == 0)
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