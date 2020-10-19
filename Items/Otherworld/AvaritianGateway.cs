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
			DisplayName.SetDefault("Avaritian Gateway");
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
			int type = Main.tile[i, j].frameX / 18 + (Main.tile[i, j].frameY / 18 * 9);
			bool playerNear = false;
			for(int k = 0; k < Main.player.Length; k++)
            {
				Vector2 pos = new Vector2(i * 16 + 8, j * 16 + 8);
				Player player = Main.player[k];
				if((player.Center - pos).Length() < 3000f)
                {
					playerNear = true;
					break;
				}
            }
			if(type == 58 && !playerNear)
            {
				if(!NPC.AnyNPCs(mod.NPCType("TheAdvisorHead")))
                {
					if(Main.netMode != 1)
					{
						int npc = NPC.NewNPC(i * 16 + 8, j * 16 + 8 - 128, mod.NPCType("TheAdvisorHead"));
						Main.npc[npc].netUpdate = true;
					}
					for(int k = 0; k < 4; k++)
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
						for(int l = 0; l < 1500; l += 16)
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
						if(!activated)
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
                }
            }
			base.RandomUpdate(i, j);
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
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			int type = Main.tile[i, j].frameX/18 + (Main.tile[i, j].frameY/18 * 9);


			Texture2D texture = mod.GetTexture("Items/Otherworld/HardlightGearBorder");
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/HardlightGearFill");
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].color()) * (100f / 255f);
			color.A = 0;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			/*
			Vector2 dynamicAddition = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTime * 40));
			if(type == -1)
				for (int k = 0; k < 5; k++)
				{
					Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
					pos.Y -= 20 + dynamicAddition.Y;
					if(k == 0)
						Main.spriteBatch.Draw(texture2, pos, null, color * 0.5f, Main.GlobalTime * (i % 2 == 0 ? 1 : -1), new Vector2(13, 13), 0.8f, SpriteEffects.None, 0f);

					Main.spriteBatch.Draw(texture, pos, null, color, Main.GlobalTime * (i % 2 == 0 ? 1 : -1), new Vector2(13, 13), 0.8f, SpriteEffects.None, 0f);
				}
			if(type == 65)
				for (int k = 0; k < 5; k++)
				{
					Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 9, (float)(j * 16 - (int)Main.screenPosition.Y) + 9) + zero;
					if (k == 0)
						Main.spriteBatch.Draw(texture2, pos, null, color * 0.5f, Main.GlobalTime, new Vector2(13, 13), 0.5f, SpriteEffects.None, 0f);

					Main.spriteBatch.Draw(texture, pos, null, color, Main.GlobalTime, new Vector2(13, 13), 0.75f, SpriteEffects.None, 0f);
				}
			if(type == 69)
				for (int k = 0; k < 5; k++)
				{
					Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 9, (float)(j * 16 - (int)Main.screenPosition.Y) + 9) + zero;
					if (k == 0)
						Main.spriteBatch.Draw(texture2, pos, null, color * 0.5f, -Main.GlobalTime, new Vector2(13, 13), 0.5f, SpriteEffects.None, 0f);

					Main.spriteBatch.Draw(texture, pos, null, color, -Main.GlobalTime, new Vector2(13, 13), 0.75f, SpriteEffects.None, 0f);
				}
				*/
			return true;
		}
	}
}