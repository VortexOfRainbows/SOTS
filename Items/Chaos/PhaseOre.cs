using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Otherworld.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class PhaseOre : ModItem
	{
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(silver: 20);
			item.createTile = ModContent.TileType<PhaseOreTile>();
		}
	}
	public class PhaseOreTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileShine[Type] = 200;
			Main.tileShine2[Type] = true;
			Main.tileValue[Type] = 1200;
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = false;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<PhaseOre>();
			//AddMapEntry(new Color(0, 0, 0, 0));
			mineResist = 5.4f;
			minPick = 180; //adamantite/chlorophyte level
			soundType = 3;
			soundStyle = 53;
			dustType = ModContent.DustType<CopyDust4>(); //DustID.PinkFlame
		}
		public override bool KillSound(int i, int j)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			Main.PlaySound(3, (int)pos.X, (int)pos.Y, 53, 0.25f, 0.6f);
			int type = Main.rand.Next(3) + 1;
			Main.PlaySound(SoundLoader.customSoundType, (int)pos.X, (int)pos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/VibrantOre" + type), 1.85f, Main.rand.NextFloat(0.1f, 0.2f));
			return false;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 7;
		}
        public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<CopyDust4>());
			dust.noGravity = true;
			dust.velocity *= 0.8f;
			dust.scale = 1.4f;
			dust.color = new Color(238, 145, 219, 0);
			dust.fadeIn = 0.1f;
			return false;
		}
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Draw(i, j);
			return false;
		}
		public static int closestPlayer(int i, int j, ref float minDist)
		{
			int p = -1;
			for (int k = 0; k < Main.player.Length; k++)
			{
				Player player = Main.player[k];
				if(player.active && !player.dead)
				{
					Vector2 pos = new Vector2(i * 16 + 8, j * 16 + 8);
					float length = (player.Center - pos).Length();
					if (length < minDist)
					{
						minDist = length;	
						p = player.whoAmI;
						if (Main.netMode == NetmodeID.SinglePlayer)
							break;
					}
				}
			}
			return p;
		}
        public override void NearbyEffects(int i, int j, bool closer)
		{
			float currentDistanceAway = 196;
			if (Main.rand.NextBool(300) && closestPlayer(i, j, ref currentDistanceAway) != -1)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 0.5f;
				dust.scale = 1.2f;
				dust.color = new Color(238, 145, 219, 0);
				dust.alpha = 100;
				dust.fadeIn = 0.1f;
			}
		}
        public static void Draw(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			float currentDistanceAway = 196;
			int playerN = closestPlayer(i, j, ref currentDistanceAway);
			if (playerN == -1)
			{
				if (tile.frameY <= 72)
				{
					tile.frameY += 90;
				}
				return;
			}
			float alphaScale = (float)Math.Pow(1.0f - currentDistanceAway / 196f, 0.5f);
			if (alphaScale > 0.0)
            {
				if(tile.frameY > 72)
                {
					for(int k = 0; k < 2; k++)
                    {
						Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.5f;
						dust.scale = 1.2f;
						dust.color = new Color(238, 145, 219, 0);
						dust.alpha = 200;
						dust.fadeIn = 0.1f;
					}
					tile.frameY -= 90;
				}
				Texture2D texture = ModContent.GetTexture("SOTS/Items/Chaos/PhaseOreTileFill");
				Texture2D texture2 = ModContent.GetTexture("SOTS/Items/Chaos/PhaseOreTileOutline");
				float degOff = (i + j) * 7f;
				for (int k = 0; k < 5 * alphaScale; k++)
				{
					Vector2 offset = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 90 + degOff));
					SOTSTile.DrawSlopedGlowMask(i, j, tile.type, texture2, new Color(100, 100, 100, 0) * alphaScale, k == 0 ? Vector2.Zero : offset);
					offset = new Vector2(1.5f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 90 + degOff));
					SOTSTile.DrawSlopedGlowMask(i, j, tile.type, texture, new Color(90, 90, 90, 0) * alphaScale, k == 0 ? Vector2.Zero : offset);
				}
			}	
		}
        public override bool CanExplode(int i, int j)
		{
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			float currentDistanceAway = 196;
			int playerN = closestPlayer(i, j, ref currentDistanceAway);
			if (playerN == -1)
				return;
			float alphaScale = (float)Math.Pow(1.0f - currentDistanceAway / 196f, 0.5f) * 0.3f;
			if (currentDistanceAway >= 195)
				return;
			r = 2.5f * alphaScale;
			g = 1.45f * alphaScale;
			b = 2.2f * alphaScale;
		}
	}
}