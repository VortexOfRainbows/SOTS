using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Pyramid;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items
{
	public class ElectromagneticDeterrentTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("D.E.F.E");
			AddMapEntry(new Color(66, 77, 93), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustID.Electric;
			AnimationFrameHeight = 54;
		}
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
			num = 3;
        }
        public override void NearbyEffects(int i, int j, bool closer)
		{
			Main.LocalPlayer.AddBuff(ModContent.BuffType<Buffs.DEFEBuff>(), 6);
			base.NearbyEffects(i, j, closer);
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			// Flips the sprite
			SpriteEffects effects = SpriteEffects.None;
			Tile tile = Main.tile[i, j];
			Texture2D texture = SOTSTile.GetTileDrawTexture(i, j);
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int k = 6;
			if (Main.tileFrame[Type] < 6)
				k = Main.tileFrame[Type] % 7;
			int animate = k * 54;
			if(k == 5)
            {
                if (Main.tile[i, j].TileFrameY % 54 == 18 && Main.tile[i, j].TileFrameX % 54 != 18)
				{
					SOTSUtils.PlaySound(SoundID.Item53, i * 16 + 24, j * 16 + 8, 0.6f, -0.1f);
					int direction = (int)(Main.tile[i, j].TileFrameX / 18 - 1);
					for(int l = 0; l < 6; l++)
					{
						Dust dust = Dust.NewDustDirect(new Vector2(i * 16 + 4, j * 16 + 13 + direction * 3), 8, 4, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity.Y *= 0.14f;
						dust.velocity.X = Math.Abs(dust.velocity.X) * 2.5f * direction + direction;
						dust.scale = 1.2f;
						dust.color = Color.Lerp(new Color(0, 158, 208, 0), new Color(122, 233, 255, 0), Main.rand.NextFloat(1f));
						dust.fadeIn = 0.1f;
					}
				}
            }
			Main.spriteBatch.Draw(texture,
				new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero,
				new Rectangle(tile.TileFrameX, tile.TileFrameY + animate, 16, 16),
				Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f);
			return false; 
		}
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 6)
			{
				frameCounter = 0;
				frame++;
				if (frame >= 10)
				{
					frame = 0;
				}
			}
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<ElectromagneticDeterrent>());
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Color color = new Color(80, 80, 80, 0);
			int frameX = Main.tile[i, j].TileFrameX / 18;
			int frameY = Main.tile[i, j].TileFrameY / 18;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/ElectromagneticDeterrentTileGlow").Value;
			int k = 6;
			if (Main.tileFrame[Type] < 6)
				k = Main.tileFrame[Type] % 7;
			int animate = k * 54;
			for (k = 0; k < 7; k++)
			{
				float x = Main.rand.NextFloat(-1, 1);
				float y = Main.rand.NextFloat(-1, 1);
				if (k <= 1)
				{
					x = 0;
					y = 0;
				}
				Main.spriteBatch.Draw(texture, new Vector2(i * 16 - Main.screenPosition.X + x, j * 16 - Main.screenPosition.Y + y + 2) + zero, new Rectangle(frameX * 18, frameY * 18 + animate, 16, 16), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 1.0f;
			g = 1.24f;
			b = 1.25f;
		}
	}
}