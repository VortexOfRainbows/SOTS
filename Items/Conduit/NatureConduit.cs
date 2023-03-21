using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Conduit
{
	public class NatureConduit : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.Size = new Vector2(36, 36);
			Item.value = Item.buyPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<NatureConduitTile>();
		}
	}
	public class NatureConduitTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = false; //sunlight passes through these pipes
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			ItemDrop = ModContent.ItemType<NatureConduit>();
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<ConduitCounterTE>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorRight = AnchorData.Empty;
			TileObjectData.newTile.AnchorLeft = AnchorData.Empty;
			TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(66, 93, 77), name);
			MineResist = 2f;
			HitSound = SoundID.Tink;
			DustType = DustID.Tungsten;
			TileID.Sets.DrawsWalls[Type] = true;
		}
		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
		}
        public override bool CreateDust(int i, int j, ref int type)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameY == 82)
			{
				for (int k = -2; k <= 2; k++)
				{
					for (int h = -2; h <= 2; h++)
					{
						if (Math.Abs(h) != 2 || Math.Abs(k) != 2) //will not check outer 4 corners
						{
							if (k != 0 || h != 0) //will not check the very center
							{
								if(Main.rand.NextBool(3))
								Dust.NewDust(new Vector2(i + k, j + h) * 16, 16, 16, DustID.Lead);
							}
						}
					}
				}
            }
			return base.CreateDust(i, j, ref type);
        }
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D texture = Terraria.GameContent.TextureAssets.Tile[Type].Value;
			Vector2 location = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			Color color = Lighting.GetColor(i, j, WorldGen.paintColor(Main.tile[i, j].TileColor));
			Vector2 origin = new Vector2(40, 40);
			spriteBatch.Draw(texture, location + zero - Main.screenPosition, new Rectangle(16 + tile.TileFrameX, tile.TileFrameY, 80, 80), color, 0f, origin, 1f, SpriteEffects.None, 0f);
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			return false;
		}
		public override bool CanPlace(int i, int j)
		{
			for (int i2 = i - 1; i2 <= i + 1; i2++)
			{
				for (int j2 = j - 1; j2 <= j + 1; j2++)
				{
					Tile tile = Framing.GetTileSafely(i2, j2);
					if (tile.TileType == Type)
					{
						return false;
					}
				}
			}
			return true;
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
			int left = i;
			int top = j;
			int index = ModContent.GetInstance<ConduitCounterTE>().Find(left, top);
			if (index == -1)
				return;
			ConduitCounterTE entity = (ConduitCounterTE)TileEntity.ByID[index];
			int chasis = entity.tileCountChasis;
			int dissolving = entity.tileCountDissolving;
			entity.NearbyClientUpdate(i, j);
			if(chasis != entity.tileCountChasis || dissolving != entity.tileCountDissolving)
				Main.NewText("D: " + entity.tileCountDissolving + "\nC: " + entity.tileCountChasis);
		}
		public override bool Drop(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			if (tile.TileFrameY == 82)
			{
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<ConduitChasis>(), 20);
			}
            return true;
		}
	}
	public class ConduitCounterTE : ModTileEntity
	{
		public int tileCountDissolving = 0;
		public int tileCountChasis = 0;
        public void NearbyClientUpdate(int i, int j) //Normally, TileEntity.Update() is run for singleplayer/server... This will be run for singleplayer/client, and syncing is made that way thusly.
        {
			tileCountDissolving = 0;
			tileCountChasis = 0;
			Tile tile = Main.tile[i, j];
			for(int k = -2; k <= 2; k++)
			{
				for (int h = -2; h <= 2; h++)
				{
					if (Math.Abs(h) != 2 || Math.Abs(k) != 2) //will not check outer 4 corners
                    {
						if(k != 0 || h != 0) //will not check the very center
                        {
							int x = i + k;
							int y = j + h;
							Tile residualTile = Main.tile[x, y];
							if(tile.TileFrameY != 0)
                            {
								if (residualTile.HasTile)
                                {
									if (residualTile.TileType == ModContent.TileType<DissolvingNatureTile>())
									{
										tileCountDissolving++;
									}
									else
                                    {
										BreakTile(x, y);
                                    }
                                }
							}
							else if (residualTile.HasTile && residualTile.TileType == ModContent.TileType<DissolvingNatureTile>())
							{
								BreakTile(x, y);
							}
							else if(residualTile.HasTile && residualTile.TileType == ModContent.TileType<ConduitChasisTile>())
                            {
								tileCountChasis++;
                            }
                        }
                    }
				}
			}
			ConvertToActiveState(i, j);
        }
		public void ConvertToActiveState(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if(tileCountChasis >= 20)
            {
				tile.TileFrameY = 82;
				for (int k = -2; k <= 2; k++)
				{
					for (int h = -2; h <= 2; h++)
					{
						if (Math.Abs(h) != 2 || Math.Abs(k) != 2) //will not check outer 4 corners
						{
							if (k != 0 || h != 0) //will not check the very center
							{
								int x = i + k;
								int y = j + h;
								WorldGen.KillTile(x, y, noItem: true);
							}
						}
					}
				}
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 5);
			}
		}
		public void BreakTile(int i, int j, bool noDrop = false)
        {
			WorldGen.KillTile(i, j, noItem: noDrop);
			NetMessage.SendData(MessageID.TileManipulation, Main.myPlayer, Main.myPlayer, null, 0, i, j);
        }
        public override bool IsTileValidForEntity(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.HasTile && tile.TileType == (ushort)ModContent.TileType<NatureConduitTile>();
		}
		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
		{
			//Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 5);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i, j);
		}
	}
}