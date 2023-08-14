using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using SOTS.Void;

namespace SOTS.Items.Earth.Glowmoth
{
	public class GlowWebTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.IsBeam[Type] = true;
			TileID.Sets.BreakableWhenPlacing[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileFrameImportant[Type] = false;
			Main.tileLavaDeath[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileBrick[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(30, 120, 170));
			HitSound = SoundID.Grass;
			DustType = DustID.Silk;
		}
        public override bool CreateDust(int i, int j, ref int type)
        {
			if(Main.rand.NextBool(3))
			{
				int num2 = Dust.NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[num2];
				dust.color = ColorHelpers.VibrantColorAttempt(Main.rand.NextFloat(360));
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 0.9f;
				return false;
			}
            return base.CreateDust(i, j, ref type);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Vector3 vColor = ColorHelpers.VibrantColor.ToVector3() * 0.325f;
			r = vColor.X;
			g = vColor.Y;
			b = vColor.Z;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D texture = TextureAssets.Tile[Type].Value;
			SOTSTile.DrawSlopedGlowMask(i, j, Type, texture, Color.Lerp(Lighting.GetColor(i, j), Color.White, 0.2f) * 0.6f, Vector2.Zero, false);
			return false;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num -= 3;
		}
	}
}