using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth.Glowmoth
{
	public class GlowSilk : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(200);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Cobweb);
			Item.width = 24;
			Item.height = 26;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 0, 20);
			Item.createTile = ModContent.TileType<GlowWebTile>();
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.White, 0.2f);
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Vector3 vColor = VoidPlayer.VibrantColor.ToVector3() * 0.34f;
			Lighting.AddLight(Item.position, vColor);
		}
    }
	public class GlowSilkTile : ModTile
	{
		public override bool CreateDust(int i, int j, ref int type)
		{
			if (Main.rand.NextBool(5))
			{
				int num2 = Dust.NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[num2];
				dust.color = VoidPlayer.VibrantColorAttempt(Main.rand.NextFloat(360));
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 0.9f;
				return false;
			}
			return base.CreateDust(i, j, ref type);
		}
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
				r = 0.25f;
				g = 0.5f;
				b = 0.8f;
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
			if (Main.tile[i, j].TileFrameY >= 90)
			{
				Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Earth/Glowmoth/GlowSilkTileGlow").Value;
				SOTSTile.DrawSlopedGlowMask(i, j, Type, texture, Color.White, Vector2.Zero, false);
			}
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
        public override bool Drop(int i, int j)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemDrop, 1 + Main.rand.Next(3));
			return false;
        }
    }
}