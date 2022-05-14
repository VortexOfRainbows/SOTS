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
			Projectile.alpha = 255;
			Projectile.timeLeft = 24;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
			Projectile.width = 16;
			Projectile.height = 16;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override void AI()
		{
			Projectile.alpha = 255;
			Projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			Vector2 position = Projectile.Center + new Vector2(16, 0);
			for (int k = 0; k < 360; k += 5)
			{
				Vector2 circularLocation = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(k));
				circularLocation += new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
				int type = ModContent.DustType<CurseDust>();
				int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, type, 0, 0, 0, default);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = circularLocation;
				Main.dust[num1].scale *= 1.5f;
				num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, type, 0, 0, 0, default);
				Main.dust[num1].velocity = circularLocation + new Vector2(0, -1);
				Main.dust[num1].scale *= 1.5f;
			}
			if (Main.netMode == 1)
				return;
			int i = (int)Projectile.Center.X / 16 + 1;
			int j = (int)Projectile.Center.Y / 16;
			int top = j - 2;
			int left = i - 1;
			bool valid = true;
			SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 4, 1.25f, 0.3f);
			for (int x = left; x < left + 2; x++)
			{
				for (int y = top; y < top + 5; y++)
				{
					if (Main.tile[x, y].type != ModContent.TileType<AncientGoldGateTile>() && Main.tile[x, y].type != ModContent.TileType<TrueSandstoneTile>())
						WorldGen.KillTile(x, y, false, false, false);
					else
						valid = false;
					if (!Main.tile[x, y].HasTile && Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, x, y, 0f, 0, 0, 0);
				}
			}
			if(valid)
			{
				bool placed = WorldGen.PlaceTile(i, j + 2, ModContent.TileType<AncientGoldGateTile>(), false, true, -1, 4); //place pillar tile
				NetMessage.SendData(MessageID.TileChange, -1, -1, null, 1, i, j + 2, ModContent.TileType<AncientGoldGateTile>(), 4, 0, 0);
				for (int x = left; x < left + 2; x++)
				{
					for (int y = top; y < top + 5; y++)
					{
						if (Projectile.ai[0] != -1 && placed)
							Main.tile[x, y].frameX += 36;
						NetMessage.SendTileSquare(-1, x, y, 2);
					}
				}
			}
		}
	}
}
		
			