using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Pyramid;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid 
{
	public class SpawnGoldGate : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spawn Gold Gate"); //Do you (source code reader) enjoy how all my net sycning is done via projectiles?
		}
		public override void SetDefaults()
		{
			projectile.alpha = 255;
			projectile.timeLeft = 24;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.netImportant = true;
			projectile.width = 16;
			projectile.height = 16;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override void AI()
		{
			projectile.alpha = 255;
			projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			int i = (int)projectile.Center.X / 16 + 1;
			int j = (int)projectile.Center.Y / 16;
			int top = j - 2;
			int left = i - 1;
			bool valid = true;
			Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 4, 1.0f, 0.3f);
			for (int x = left; x < left + 2; x++)
			{
				for (int y = top; y < top + 5; y++)
				{
					if (Main.tile[x, y].type != ModContent.TileType<AncientGoldGateTile>() && Main.tile[x, y].type != ModContent.TileType<TrueSandstoneTile>())
						WorldGen.KillTile(x, y, false, false, false);
					else
						valid = false;
					if (!Main.tile[x, y].active() && Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, x, y, 0f, 0, 0, 0);
				}
			}
			if(valid)
			{
				bool placed = WorldGen.PlaceTile(i, j + 2, ModContent.TileType<AncientGoldGateTile>(), false, true, -1, 4); //place pillar tile
				if (projectile.ai[0] != -1 && placed)
					for (int x = left; x < left + 2; x++)
					{
						for (int y = top; y < top + 5; y++)
						{
							Main.tile[x, y].frameX += 36;
							//NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, x, y, 0f, 0, 0, 0);
							if (Main.netMode != 1)
								NetMessage.SendTileSquare(-1, x, y, 2);
						}
					}
			}
			Main.PlaySound(SoundID.Item4, projectile.Center);
			Vector2 position = projectile.Center;
			for (int k = 0; k < 360; k += 10)
			{
				Vector2 circularLocation = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(k));
				circularLocation += new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
				int type = ModContent.DustType<CurseDust>();
				int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, type, 0, 0, 0, default);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = circularLocation;
			}
		}
	}
}
		
			