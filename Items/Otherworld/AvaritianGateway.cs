using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss;
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
			TileObjectData.newTile.Height = 8;
			TileObjectData.newTile.Width = 9;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 5, 2); 
			TileObjectData.newTile.Origin = new Point16(4, 7);
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
			Item.NewItem(i * 16, j * 16, 128, 144, drop);
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
					if ((player.Center - pos).Length() < 2800f)
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
							int npc = NPC.NewNPC(i * 16 + 8, j * 16 + 8 - 128, mod.NPCType("TheAdvisorHead"));
							Main.npc[npc].netUpdate = true;
						}
						for (int k = 0; k < 4; k++)
						{
							float degrees = 0;
							if (k == 0)
								degrees = 80f;
							if (k == 1)
								degrees = 40f;
							if (k == 2)
								degrees = -40f;
							if (k == 3)
								degrees = -80f;
							Vector2 direction = new Vector2(0, 1500).RotatedBy(MathHelper.ToRadians(degrees));
							bool activated = false;
							for (int l = 0; l < 1500; l += 16)
							{
								direction -= direction.SafeNormalize(Vector2.Zero) * 16f;
								int addX = (int)direction.X / 16;
								int addY = (int)direction.Y / 16;
								int locX = addX + i;
								int locY = addY + j;
								Tile tile = Framing.GetTileSafely(locX, locY);
								if (tile.active() && !Main.tileSolidTop[tile.type] && Main.tileSolid[tile.type] && (tile.type == (ushort)mod.TileType("AvaritianPlatingTile") || tile.type == (ushort)mod.TileType("PortalPlatingTile") || tile.type == (ushort)mod.TileType("DullPlatingTile")))
								{
									direction += direction.SafeNormalize(Vector2.Zero) * 80f;

									if (Main.netMode != 1)
									{
										int npc = NPC.NewNPC(i * 16 + 8 + (int)direction.X, j * 16 + 8 + (int)direction.Y, mod.NPCType("OtherworldlyConstructHead2"));
										Main.npc[npc].netUpdate = true;
										TheAdvisorHead.ConstructIds[k] = npc;
									}
									activated = true;
									break;
								}
							}
							if (!activated)
							{
								direction = direction.SafeNormalize(Vector2.Zero) * -240f;
								direction.Y += 80;
								if (Main.netMode != 1)
								{
									int npc = NPC.NewNPC(i * 16 + 8 + (int)direction.X, j * 16 + 8 + (int)direction.Y, mod.NPCType("OtherworldlyConstructHead2"));
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
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			int type = Main.tile[i, j].frameX / 18 + (Main.tile[i, j].frameY / 18 * 9);
			if (type != 58)
				return;

			r = 0.9f;
			g = 0.9f;
			b = 1.1f;
		}
	}
}