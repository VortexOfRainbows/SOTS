using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Advisor;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace SOTS.Items.Otherworld
{
	public class AvaritianGateway : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avaritia Gateway");
			Tooltip.SetDefault("'A strange portal that leads nowhere'\nServes as the spawning location for the Advisor");
		}
		public override void SetDefaults()
		{
			item.width = 46;
			item.height = 42;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 9;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.consumable = true;
			item.createTile = mod.TileType("AvaritianGatewayTile");
		}
	}	
	public class AvaritianGatewayTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.Height = 9;
			TileObjectData.newTile.Width = 9;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 5, 2); 
			TileObjectData.newTile.Origin = new Point16(4, 8);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Strange Gateway");
			AddMapEntry(new Color(55, 45, 65), name);
			disableSmartCursor = true;
			dustType = mod.DustType("AvaritianDust");
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = mod.ItemType("AvaritianGateway");
			Item.NewItem(i * 16, j * 16, 144, 144, drop);
		}
        public override void RandomUpdate(int i, int j)
		{
			if (Main.netMode != 1)
				SpawnAdvisor(i, j, mod);
			base.RandomUpdate(i, j);
		}
		public static bool SpawnAdvisor(int i, int j, Mod mod)
		{
			int type = Main.tile[i, j].frameX / 18 + (Main.tile[i, j].frameY / 18 * 9);
			if (type == 58)
			{
				bool playerNear = false;
				for (int k = 0; k < Main.player.Length; k++)
				{
					Vector2 pos = new Vector2(i * 16 + 8, j * 16 + 8);
					Player player = Main.player[k];
					if ((player.Center - pos).Length() < 2800f && player.active)
					{
						playerNear = true;
						break;
					}
				}
				if (!playerNear)
				{
					if (!NPC.AnyNPCs(mod.NPCType("TheAdvisorHead")))
					{
						if (Main.netMode != 1)
						{
							int npc = NPC.NewNPC(i * 16 + 8, j * 16 + 8 - 240, mod.NPCType("TheAdvisorHead"));
							Main.npc[npc].netUpdate = true;
						}
						for (int k = 0; k < 4; k++)
						{
							float degrees = 0;
							if (k == 0 || k == 3)
								degrees = 80f;
							if (k == 1 || k == 2)
								degrees = 40f;
							bool activated = false;
							for (int l = 1200; l > 0; l -= 10)
							{
								Vector2 direction = new Vector2(0, l).RotatedBy(MathHelper.ToRadians(degrees));
								if (k == 2 || k == 3)
									direction.X *= -1;
								int locX = (int)(direction.X / 16f + 0.5f) + i;
								int locY = (int)(direction.Y / 16f + (k == 2 || k == 3 ? -0.5f : 0)) + j;
								Tile tile = Framing.GetTileSafely(locX, locY);
								if (tile.active() && !Main.tileSolidTop[tile.type] && Main.tileSolid[tile.type] && (tile.type == (ushort)mod.TileType("AvaritianPlatingTile") || tile.type == (ushort)mod.TileType("PortalPlatingTile") || tile.type == (ushort)mod.TileType("DullPlatingTile")))
								{
									direction = direction.SafeNormalize(Vector2.Zero) * 80f;
									if (Main.netMode != 1)
									{
										Vector2 location = new Vector2(locX * 16 + 8, locY * 16 + 8);
										int npc = NPC.NewNPC((int)location.X + (int)direction.X, (int)location.Y + (int)direction.Y, mod.NPCType("OtherworldlyConstructHead2"), 0, 0, 0, location.X, location.Y);
										Main.npc[npc].netUpdate = true;
										TheAdvisorHead.ConstructIds[k] = npc;
									}
									activated = true;
									break;
								}
							}
							if (!activated)
							{
								Vector2 direction = new Vector2(0, 240).RotatedBy(MathHelper.ToRadians(degrees));
								if (k == 2 || k == 3)
									direction.X *= -1;
								direction.Y += 80;
								if (Main.netMode != 1)
								{
									int npc = NPC.NewNPC(i * 16 + 8 + (int)(direction.X + 0.5f), j * 16 + 8 + (int)(direction.Y + 0.5f), mod.NPCType("OtherworldlyConstructHead2"));
									Main.npc[npc].netUpdate = true;
									TheAdvisorHead.ConstructIds[k] = npc;
								}
							}
						}
						return true;
					}
				}
			}
			return false;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			float uniquenessCounter = Main.GlobalTime * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = mod.GetTexture("Items/Otherworld/AvaritianGatewayTileGlow");
			Rectangle frame = new Rectangle(tile.frameX, tile.frameY, 16, 16);
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].color()) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 5; k++)
			{
				Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero;
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.10f * k;
				Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * 0.75f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			int type = Main.tile[i, j].frameX / 18 + (Main.tile[i, j].frameY / 18 * 9);
			if (type != 67)
				return;

			r = 1.1f;
			g = 1.2f;
			b = 1.3f;
		}
	}
}