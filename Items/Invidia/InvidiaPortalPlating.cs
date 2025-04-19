using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Invidia
{
	public class InvidiaPortalPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Lime;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<InvidiaPortalPlatingTile>();
		}
	}
	public class InvidiaPortalPlatingTile : ModTile
	{
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
            float fillPercent = SOTSWorld.MoonPhasePercent * SOTSWorld.MoonPhasePercent * 0.9f + 0.1f * SOTSTile.PlanetariumLightingColorMultiplier(i, j) * SOTSWorld.MoonPhasePercent;
			r = 0.3f * fillPercent;
            g = 1f * fillPercent;
			b = 0.4f * fillPercent;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color lC = Lighting.GetColor(i, j);
            float fillPercent = SOTSWorld.MoonPhasePercent * SOTSWorld.MoonPhasePercent * 0.9f + 0.1f * SOTSTile.PlanetariumLightingColorMultiplier(i, j) * SOTSWorld.MoonPhasePercent;
            Color runeColor = Color.Lerp(lC, Color.White, fillPercent) * fillPercent;
            //float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
            Tile tile = Main.tile[i, j];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Invidia/InvidiaPortalPlatingTileGlow").Value;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.175f; // + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 3; k++)
			{
				Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
				Vector2 offset = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(k * 120 + SOTSWorld.GlobalCounter));
				Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * fillPercent, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(86, 226, 100));
			MineResist = 2f;
			MinPick = 250;
			HitSound = SoundID.Tink;
			DustType = ModContent.DustType<EvostoneDust>();
			TileID.Sets.GemsparkFramingTypes[Type] = Type;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
			return false;
		}
	}
}