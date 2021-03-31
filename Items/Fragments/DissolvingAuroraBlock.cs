using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Fragments
{
	public class DissolvingAuroraBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aurora Block");
			Tooltip.SetDefault("");
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
			item.rare = 3;
			item.consumable = true;
			item.createTile = mod.TileType("DissolvingAuroraTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingAurora", 1);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();
		}
	}
	public class DissolvingAuroraTile : ModTile
	{
		public static Color color = new Color(45, 95, 115);
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("DissolvingAuroraBlock");
			AddMapEntry(color);
			mineResist = 0.2f;
			TileID.Sets.GemsparkFramingTypes[Type] = Type;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 5;
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, 267);
			dust.color = color;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.8f;
			dust.velocity *= 2.4f;
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 1.0f;
			g = 1.2f;
			b = 2.3f;
			r *= 0.3f;
			g *= 0.3f;
			b *= 0.3f;
		}
		public static void DrawEffects(int i, int j, SpriteBatch spriteBatch, Mod mod, bool wall = false)
        {
			Texture2D texture = mod.GetTexture("Gores/AuroraParticle");
			Texture2D textureBlock = mod.GetTexture("Gores/AuroraBlockOutline");
			Color color;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 16; k += 2)
			{
				Vector2 location = new Vector2(i * 16, j * 16);
				//color = DissolvingAuroraTile.color;
				//if (Main.tile[i, j].color() != 0)
				color = WorldGen.paintColor((int)Main.tile[i, j].color());
				if (wall && Main.tile[i, j].wallColor() != 0)
					color = WorldGen.paintColor((int)Main.tile[i, j].wallColor());
				color = new Color(color.R, color.G, color.B, 0);
				int seed = k + i + j + (i * j);
				int uniqueParticleFrame = i - seed + (int)(Main.GlobalTime * 10); //serves as a type of randomizer for the particles
				if (i % 2 == 0)
					uniqueParticleFrame += 3;
				if (i % 3 == 0)
					uniqueParticleFrame += 3;
				if (i % 4 == 0)
					uniqueParticleFrame += 3;
				if (seed % 2 == 0)
					uniqueParticleFrame += 2;
				if (seed % 3 == 0)
					uniqueParticleFrame += 4;
				if (seed % 6 == 0)
					uniqueParticleFrame += 3;
				if (seed % 7 == 0)
					uniqueParticleFrame += 3;

				uniqueParticleFrame = Math.Abs(uniqueParticleFrame) % (18 + (((k / 2) % 2 == 0) ? 1 : 0) + ((((k / 2) + i) % 3 == 0) ? 1 : 0) + ((((k / 3) + j) % 5 == 0) ? 1 : 0));
				if (uniqueParticleFrame < 15)
				{
					location.Y += (uniqueParticleFrame * -2) + 46;
				}
				else
				{
					color *= 0f;
				}

				if (Main.tile[i, j].slope() == 3)
					location.Y -= k;

				if (Main.tile[i, j].slope() == 4)
					location.Y -= 15 - k;

				location.X += k;
				Vector2 drawPos = location - Main.screenPosition;

				if (!Main.tile[i, j + 1].active() || !Main.tileSolid[Main.tile[i, j + 1].type] && uniqueParticleFrame < 7 && uniqueParticleFrame != 0)
				{
					color *= (float)(uniqueParticleFrame / 14f);
					ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
					for (int l = 0; l < 7; l++)
					{
						float x = (float)Utils.RandomInt(ref randSeed, -16, 17) * 0.05f;
						float y = (float)Utils.RandomInt(ref randSeed, -16, 17) * 0.05f;
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y - 2) + zero, new Rectangle(0, 6 * (uniqueParticleFrame % 4), 6, 6), color, 0f, new Vector2(3, 3), 0.75f, SpriteEffects.None, 0f);
					}
					//spriteBatch.Draw(texture, drawPos, null, color, 0, new Vector2(0,0), 1, SpriteEffects.None, 0f);
				}
			}
			if (Main.tileSolid[Main.tile[i, j].type] && !Main.tileSolidTop[Main.tile[i, j].type])
			{
				//color = DissolvingAuroraTile.color;
				//if (Main.tile[i, j].color() != 0)
				color = WorldGen.paintColor((int)Main.tile[i, j].color());
				if (wall && Main.tile[i, j].wallColor() != 0)
					color = WorldGen.paintColor((int)Main.tile[i, j].wallColor());
				color = new Color(color.R, color.G, color.B, 0);
				ulong randSeed2 = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
				for (int l = 0; l < 7 - (Main.tile[i, j].inActive() ? 1 : 0); l++)
				{
					float x = (float)Utils.RandomInt(ref randSeed2, -16, 17) * 0.1f;
					float y = (float)Utils.RandomInt(ref randSeed2, -16, 17) * 0.1f;
					if (Main.tile[i, j].inActive() && l < 4)
					{
						x = 0;
						y = 0;
					}
					bool canUp = true;
					bool canDown = true;
					bool canLeft = true;
					bool canRight = true;
					if (Main.tile[i, j - 1].active() && Main.tileSolid[Main.tile[i, j - 1].type])
						canUp = false;

					if (Main.tile[i, j + 1].active() && Main.tileSolid[Main.tile[i, j + 1].type])
						canDown = false;

					if (Main.tile[i + 1, j].active() && Main.tileSolid[Main.tile[i + 1, j].type])
						canRight = false;

					if (Main.tile[i - 1, j].active() && Main.tileSolid[Main.tile[i - 1, j].type])
						canLeft = false;

					if (!canUp && !canDown)
					{
						y = 0;
					}
					else if (!canUp || !canDown)
					{
						if (!canUp)
							y = Math.Abs(y);

						if (!canDown)
							y = -Math.Abs(y);
					}
					if (!canRight && !canLeft)
					{
						x = 0;
					}
					else if (!canRight || !canLeft)
					{
						if (!canRight)
							x = -Math.Abs(x);

						if (!canLeft)
							x = Math.Abs(x);
					}
					Main.spriteBatch.Draw(textureBlock, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y - 2) + zero, new Rectangle(0, 20 * (Main.tile[i, j].halfBrick() ? 1 : Main.tile[i, j].slope() > 0 ? Main.tile[i, j].slope() + 1 : 0), 16, 20), color, 0f, default, 1f, SpriteEffects.None, 0f);
				}
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawEffects(i, j, spriteBatch, mod);
			return true;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
			Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
            return false;
        }
	}
}