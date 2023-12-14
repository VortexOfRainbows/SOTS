using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.NPCs.Boss.Curse;
using Terraria.DataStructures;

namespace SOTS.Items.Pyramid
{
	public class CursedHiveBlock : ModItem
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
			Item.rare = ItemRarityID.Blue;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<CursedHiveSafe>();
		}
	}
	public class CursedHiveSafe : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTile>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTileSafe>()] = true;
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<CursedHiveBlock>();
			AddMapEntry(new Color(135, 120, 158));
			MineResist = 0.5f;
			HitSound = SoundID.Tink;
			//DustType = ModContent.DustType<CurseDust>(); //too much light
			DustType = 32;
		}
	}
	public class CursedHive : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<CursedHiveBlock>();
			AddMapEntry(new Color(135, 120, 158));
			MineResist = 0.5f;
			HitSound = SoundID.Tink;
			//DustType = ModContent.DustType<CurseDust>(); //too much light
			DustType = 32;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && !noItem && Main.rand.NextBool(24) && !fail && !effectOnly)
			{
				Projectile.NewProjectile(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8, j * 16 + 8), Vector2.Zero, ModContent.ProjectileType<ReleaseWallMimic>(), 0, 0, Main.myPlayer);
				noItem = true;
			}
		}
		public override bool CanExplode(int i, int j)
		{
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return SOTSWorld.downedCurse;
		}
	}
	public class ReleaseWallMimic : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Release Wall Mimic"); //Do you enjoy how all my net sycning is done via projectiles?
		}
		public override void SetDefaults()
		{
			Projectile.alpha = 255;
			Projectile.timeLeft = 24;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.width = 40;
			Projectile.height = 40;
		}
		public override void AI()
		{
			Projectile.alpha = 255;
			Projectile.Kill();
		}
		public override void OnKill(int timeLeft)
		{
			if(Projectile.ai[0] == -1)
			{
				if (!NPC.AnyNPCs(ModContent.NPCType<PharaohsCurse>()))
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int npc1 = NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.position.X + Projectile.width / 2, (int)Projectile.position.Y + Projectile.height, ModContent.NPCType<PharaohsCurse>());
						Main.npc[npc1].netUpdate = true;
					}
				}
			}
			else
            {
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int npc1 = NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.position.X + Projectile.width / 2, (int)Projectile.position.Y + Projectile.height, ModContent.NPCType<NPCs.WallMimic>());
					Main.npc[npc1].netUpdate = true;
					Main.npc[npc1].ai[2] = 900;
				}
				SOTSUtils.PlaySound(SoundID.Item14, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
				int x2 = (int)(Projectile.Center.X / 16f);
				int y2 = (int)(Projectile.Center.Y / 16f);
				for (int i = x2 - 1; i <= x2 + 1; i++)
				{
					for (int j = y2 - 1; j <= y2 + 1; j++)
					{
						bool canKillTile = true;
						if (Main.tile[i, j] != null && Main.tile[i, j].HasTile)
						{
							canKillTile = true;
							if (Main.tileDungeon[(int)Main.tile[i, j ].TileType] || Main.tile[i, j ].TileType == 88 || Main.tile[i, j ].TileType == 21 || Main.tile[i, j ].TileType == 26 || Main.tile[i, j ].TileType == 107 || Main.tile[i, j ].TileType == 108 || Main.tile[i, j ].TileType == 111 || Main.tile[i, j ].TileType == 226 || Main.tile[i, j ].TileType == 237 || Main.tile[i, j ].TileType == 221 || Main.tile[i, j ].TileType == 222 || Main.tile[i, j ].TileType == 223 || Main.tile[i, j ].TileType == 211 || Main.tile[i, j ].TileType == 404)
							{
								canKillTile = false;
							}
							if (!Main.hardMode && Main.tile[i, j ].TileType == 58)
							{
								canKillTile = false;
							}
							if (!TileLoader.CanExplode(i, j) && (Main.tile[i, j ].TileType != (ushort)Mod.Find<ModTile>("PyramidSlabTile").Type))
							{
								canKillTile = false;
							}
							if (canKillTile)
							{
								WorldGen.KillTile(i, j, false, false, Main.tile[i, j ].TileType == (ushort)Mod.Find<ModTile>("CursedHive").Type);
								if (!Main.tile[i, j].HasTile && Main.netMode != 0)
								{
									NetMessage.SendData(17, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
								}
							}
						}
						if (canKillTile)
						{
							for (int x = i - 1; x <= i + 1; x++)
							{
								for (int y = j - 1; y <= j + 1; y++)
								{
									if (Main.tile[x, y] != null && Main.tile[x, y].WallType > 0)
									{
										if (Main.tile[x, y].WallType == 0 && Main.netMode != 0)
										{
											NetMessage.SendData(17, -1, -1, null, 2, (float)x, (float)y, 0f, 0, 0, 0);
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}