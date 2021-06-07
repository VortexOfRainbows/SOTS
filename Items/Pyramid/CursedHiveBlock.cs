using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	public class CursedHiveBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infected Pyramid Brick");
			Tooltip.SetDefault("");// A living clump of matter residing in a broken down brick\n'It has the consistency of a tumor'");
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
			item.rare = 5;
			item.consumable = true;
			item.createTile = mod.TileType("CursedHiveSafe");
		}
	}
	public class CursedHiveSafe : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("CursedHiveBlock");
			AddMapEntry(new Color(51, 46, 91));
			mineResist = 0.5f;
			soundType = SoundID.Tink;
			soundStyle = 2;
			//dustType = mod.DustType("CurseDust"); //too much light
			dustType = 32;
		}
		public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("CursedHive"))
			{
				return true;
			}
			return false;
		}
		public override bool Slope(int i, int j)
		{
			if (SOTSWorld.downedCurse)
				return true;

			return false;
		}
	}
	public class CursedHive : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("CursedHiveBlock");
			AddMapEntry(new Color(51, 46, 91));
			mineResist = 0.5f;
			soundType = SoundID.Tink;
			soundStyle = 2;
			//dustType = mod.DustType("CurseDust"); //too much light
			dustType = 32;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (Main.netMode != 1 && !noItem && Main.rand.Next(24) == 0 && !fail && !effectOnly)
			{
				Projectile.NewProjectile(new Vector2(i * 16 + 8, j * 16 + 8), Vector2.Zero, mod.ProjectileType("ReleaseWallMimic"), 0, 0, Main.myPlayer);
				noItem = true;
			}
			base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
		}
		public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("CursedHive"))
			{
				return true;
			}
			return false;
		}
		public override bool Slope(int i, int j)
		{
			if (SOTSWorld.downedCurse)
				return true;

			return false;
		}
	}
	public class ReleaseWallMimic : ModProjectile
	{
		int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Release Wall Mimic"); //Do you enjoy how all my net sycning is done via projectiles?
		}
		public override void SetDefaults()
		{
			projectile.alpha = 255;
			projectile.timeLeft = 24;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.width = 40;
			projectile.height = 40;
		}
		public override void AI()
		{
			projectile.alpha = 255;
			projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != 1)
			{
				int npc1 = NPC.NewNPC((int)projectile.position.X + projectile.width / 2, (int)projectile.position.Y + projectile.height, mod.NPCType("WallMimic"));
				Main.npc[npc1].netUpdate = true;
				Main.npc[npc1].ai[2] = 900;
			}

			Main.PlaySound(SoundID.Item14, (int)(projectile.Center.X), (int)(projectile.Center.Y));
			int x2 = (int)(projectile.Center.X / 16f);
			int y2 = (int)(projectile.Center.Y / 16f);
			for (int i = x2 - 1; i <= x2 + 1; i++)
			{
				for (int j = y2 - 1; j <= y2 + 1; j++)
				{
					bool canKillTile = true;
					if (Main.tile[i, j] != null && Main.tile[i, j].active())
					{
						canKillTile = true;
						if (Main.tileDungeon[(int)Main.tile[i, j].type] || Main.tile[i, j].type == 88 || Main.tile[i, j].type == 21 || Main.tile[i, j].type == 26 || Main.tile[i, j].type == 107 || Main.tile[i, j].type == 108 || Main.tile[i, j].type == 111 || Main.tile[i, j].type == 226 || Main.tile[i, j].type == 237 || Main.tile[i, j].type == 221 || Main.tile[i, j].type == 222 || Main.tile[i, j].type == 223 || Main.tile[i, j].type == 211 || Main.tile[i, j].type == 404)
						{
							canKillTile = false;
						}
						if (!Main.hardMode && Main.tile[i, j].type == 58)
						{
							canKillTile = false;
						}
						if (!TileLoader.CanExplode(i, j) && (Main.tile[i, j].type != (ushort)mod.TileType("PyramidSlabTile")))
						{
							canKillTile = false;
						}
						if (canKillTile)
						{
							WorldGen.KillTile(i, j, false, false, Main.tile[i, j].type == (ushort)mod.TileType("CursedHive"));
							if (!Main.tile[i, j].active() && Main.netMode != 0)
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
								if (Main.tile[x, y] != null && Main.tile[x, y].wall > 0)
								{
									if (Main.tile[x, y].wall == 0 && Main.netMode != 0)
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