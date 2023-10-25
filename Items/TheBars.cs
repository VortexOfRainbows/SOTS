using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Permafrost;
using SOTS.Items.Planetarium.FromChests;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SOTS.Items.Earth;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Chaos;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace SOTS.Items
{
	public class TheBars : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileShine[Type] = 1100;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.MetalBar")); // localized text for "Metal Bar"
		}
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num -= 2;
			Tile tile = Main.tile[i, j];
			int style = tile.TileFrameX / 18;
			if (style >= 3 && style <= 7)
			{
				num += 4;
			}
        }
        public override bool CreateDust(int i, int j, ref int type)
		{
			Tile tile = Main.tile[i, j];
			int style = tile.TileFrameX / 18;
			if (style == 0 || style == 11)
			{
				type = DustID.Silver;
			}
			if (style == 1 || style == 2)
			{
				type = ModContent.DustType<AvaritianDust>();
			}
			if (style == 3 || style == 4)
			{
				type = 116; //skyware dust
				if (Main.rand.NextBool(2))
					type = DustID.Gold;
			}
			if (style == 5 || style == 6)
			{
				type = DustID.t_Meteor;
				if (Main.rand.NextBool(3))
					type = DustID.Torch;
			}
			if (style == 7)
			{
				type = ModContent.DustType<CopyIceDust>();
				if (Main.rand.NextBool(3))
					type = 60; // DustID.RedTorch
			}
			if (style == 8)
			{
				type = ModContent.DustType<CopyIceDust>();
			}
			if (style == 9)
			{
				type = ModContent.DustType<VibrantDust>();
				if (Main.rand.NextBool(3))
					type = 59; //DustID.BlueTorch
			}
			if(style == 10)
            {
				Dust dust2 = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<CopyDust4>());
				dust2.noGravity = true;
				dust2.velocity *= 0.8f;
				dust2.scale = 1.4f;
				dust2.color = new Color(238, 145, 219, 0);
				dust2.fadeIn = 0.1f;
				return false;
            }
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 16, 16, type);
			if (type == 59 || type == 60 || type == DustID.Torch)
			{
				dust.scale *= 1.4f;
			}			
			if(type == ModContent.DustType<CopyIceDust>())
				dust.scale *= 1.2f;
			return false;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			int style = tile.TileFrameX / 18;
			if(style == 10)
			{
				PhaseOreTile.Draw((Texture2D)ModContent.Request<Texture2D>("SOTS/Items/PhaseBarTileOutline"), (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/PhaseBarTileFill"), i, j, 0.5f, true);
				return false;
            }				
			return true;
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile tile = Main.tile[i, j];
			Tile tileAbove = Main.tile[i, j - 1];
			int style = tile.TileFrameX / 18;
			if (style == 1 || style == 2)
			{
				if (tileAbove.HasTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]))
                {
					style = 2;
				}
				else
                {
					style = 1;
                }
			}
			if (style == 3 || style == 4)
			{
				if (tileAbove.HasTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]))
				{
					style = 4;
				}
				else
				{
					style = 3;
				}
			}
			if (style == 5 || style == 6)
			{
				if (tileAbove.HasTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]))
				{
					style = 6;
				}
				else
				{
					style = 5;
				}
			}
			tile.TileFrameX = (short)(style * 18);
			return true;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			int style = tile.TileFrameX / 18;
			float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/TheBarsGlow").Value;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 tilePosition = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
			if (style == 1 || style == 2)
			{
				Color color;
				color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
				color.A = 0;
				float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
				for (int k = 0; k < 4; k++)
				{
					Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.1f * k;
					Main.spriteBatch.Draw(texture, tilePosition + offset, frame, color * alphaMult * 0.8f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
			}
			if (style == 3 || style == 4)
			{
				Main.spriteBatch.Draw(texture, tilePosition, frame, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
			Tile tile = Main.tile[i, j];
			int style = tile.TileFrameX / 18;
			EntitySource_TileBreak source = new EntitySource_TileBreak(i, j);
			if (style == 0) 
			{
				yield return new Item(ModContent.ItemType<AncientSteelBar>());
			}
			if (style == 1 || style == 2)
            {
                yield return new Item(ModContent.ItemType<HardlightAlloy>());
			}
			if (style == 3 || style == 4)
            {
                yield return new Item(ModContent.ItemType<StarlightAlloy>());
			}
			if (style == 5 || style == 6)
            {
                yield return new Item(ModContent.ItemType<OtherworldlyAlloy>());
			}
			if (style == 7)
            {
                yield return new Item(ModContent.ItemType<AbsoluteBar>());
			}
			if (style == 8)
            {
                yield return new Item(ModContent.ItemType<FrigidBar>());
			}
			if (style == 9)
            {
                yield return new Item(ModContent.ItemType<VibrantBar>());
			}
			if (style == 10)
            {
                yield return new Item(ModContent.ItemType<PhaseBar>());
			}
			if (style == 11)
            {
                yield return new Item(ModContent.ItemType<AncientSteelBar>());
			}
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			int style = tile.TileFrameX / 18;
			if (style == 10)
			{
				float currentDistanceAway = 196;
				int playerN = PhaseOreTile.closestPlayer(i, j, ref currentDistanceAway);
				if (playerN == -1 && !Main.LocalPlayer.CanSeeInvisibleBlocks)
					return;
				float alphaScale = (float)Math.Pow(1.0f - currentDistanceAway / 196f, 0.5f) * 0.3f;
                if (Main.LocalPlayer.CanSeeInvisibleBlocks && alphaScale < 0.1f)
                    alphaScale = 0.1f;
                if (currentDistanceAway >= 195)
					return;
				r = 2.5f * alphaScale;
				g = 1.45f * alphaScale;
				b = 2.2f * alphaScale;
			}
			else
            {
				r = 0;
				g = 0;
				b = 0;
            }
		}
	}
}