using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Curse;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid
{
	public class PyramidGate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyramid Gate");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.consumable = true;
			item.createTile = ModContent.TileType<PyramidGateTile>();
			item.placeStyle = 0;
		}
	}	
	public class PyramidGateTile : ModTile
	{
		public override void SetDefaults()
		{
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileBlockLight[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
			TileObjectData.newTile.CoordinateHeights = new[] { 16 };
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Width = 5;
			TileObjectData.newTile.DrawYOffset = 0;
			TileObjectData.newTile.Origin = new Point16(2, 0); 
			TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);
			TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Pyramid Gate");
			AddMapEntry(new Color(220, 180, 25), name);
			disableSmartCursor = true;
			dustType = DustID.GoldCoin;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Tile tile = Main.tile[i, j];
			Texture2D texture = mod.GetTexture("Items/Pyramid/PyramidGateTile");
			Rectangle frame = new Rectangle(tile.frameX, tile.frameY, 16, 16);
			Color color = WorldGen.paintColor((int)Main.tile[i, j].color());
			color = Lighting.GetColor(i, j, color);
			Vector2 pos = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
			Main.spriteBatch.Draw(texture, pos, frame, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.showItemIcon2 = ModContent.ItemType<PyramidKey>();
			player.noThrow = 2;
			player.showItemIcon = true;
		}
		public override void MouseOverFar(int i, int j)
		{
			Player player = Main.LocalPlayer;
			MouseOver(i, j);
			if (player.showItemIconText == "")
			{
				player.showItemIcon = false;
				player.showItemIcon2 = 0;
			}
		}
        public override bool NewRightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i - tile.frameX / 18;
			int top = j - tile.frameY / 18;
			Main.mouseRightRelease = false;
			int key = ModContent.ItemType<PyramidKey>();
			if (player.ConsumeItem(key))
			{
				Projectile.NewProjectile(new Vector2(left, top) * 16 + new Vector2(40, 8), Vector2.Zero, ModContent.ProjectileType<PyramidGateProj>(), 0, 0, Main.myPlayer);
			}
			return true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
        public override bool Slope(int i, int j)
        {
            return false;
        }
        public override bool CanExplode(int i, int j)
		{
			return false;
        }
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 9;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			//Item.NewItem(i * 16, j * 16, 80, 16, ModContent.ItemType<TaintedKeystoneShard>(), 1);
			//Item.NewItem(i * 16, j * 16, 80, 16, ModContent.ItemType<PyramidGate>());
		}
	}
	public class PyramidGateProj : ModProjectile
	{
		public override string Texture => "SOTS/Items/Pyramid/PyramidGate";
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCsAndTiles.Add(index);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Multiplayer Code 5000");
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
		public override void SetDefaults()
		{
			projectile.height = 16;
			projectile.width = 16;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 5;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.hide = true;
		}
        public override void AI()
        {
			projectile.Kill();
        }
        public override void Kill(int timeLeft)
		{
			int i = (int)projectile.Center.X / 16;
			int j = (int)projectile.Center.Y / 16;
			WorldGen.KillTile(i, j, false, false, false);
			if (!Main.tile[i, j].active() && Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
			Vector2 center = projectile.Center + new Vector2(0, -16);
			Main.PlaySound(2, (int)center.X, (int)center.Y, 62, 1.25f, -0.5f);
			if(Main.netMode != NetmodeID.Server)
			{
				for (int k = 0; k < 16; k++)
				{
					int goreIndex2 = Gore.NewGore(center - new Vector2(16, 40) + new Vector2(Main.rand.NextFloat(-16, 16f), Main.rand.NextFloat(-16, 64f)), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex2].scale = 0.9f;
				}
				int goreIndex = Gore.NewGore(new Vector2(i * 16, j * 16), Vector2.Zero, mod.GetGoreSlot("Gores/Tiles/PyramidGateGore1"), 1f);
				Main.gore[goreIndex].velocity *= 0.2f;
				goreIndex = Gore.NewGore(new Vector2(i * 16 - 16, j * 16), Vector2.Zero, mod.GetGoreSlot("Gores/Tiles/PyramidGateGore2"), 1f);
				Main.gore[goreIndex].velocity *= 0.2f;
				goreIndex = Gore.NewGore(new Vector2(i * 16 + 16, j * 16), Vector2.Zero, mod.GetGoreSlot("Gores/Tiles/PyramidGateGore3"), 1f);
				Main.gore[goreIndex].velocity *= 0.2f;
				goreIndex = Gore.NewGore(new Vector2(i * 16 + 32, j * 16), Vector2.Zero, mod.GetGoreSlot("Gores/Tiles/PyramidGateGore4"), 1f);
				Main.gore[goreIndex].velocity *= 0.2f;
				goreIndex = Gore.NewGore(new Vector2(i * 16 - 32, j * 16), Vector2.Zero, mod.GetGoreSlot("Gores/Tiles/PyramidGateGore5"), 1f);
				Main.gore[goreIndex].velocity *= 0.2f;
			}
			for (j = 0; j < 30; j++)
			{
				Vector2 direction = new Vector2(0, -2).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45)));
				int num1 = Dust.NewDust(center - new Vector2(12, 12), 16, 16, DustID.GoldCoin);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.15f;
				Main.dust[num1].velocity += direction;
				Main.dust[num1].velocity.Y *= 3;
				Main.dust[num1].velocity.X *= 1.5f;
				Main.dust[num1].scale = 1.15f; 

				num1 = Dust.NewDust(center - new Vector2(12, 12), 16, 16, 198);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.15f;
				Main.dust[num1].velocity += direction;
				Main.dust[num1].velocity.Y *= 4;
				Main.dust[num1].velocity.X *= 2.0f;
				Main.dust[num1].scale = 1.2f;

				num1 = Dust.NewDust(center - new Vector2(12, 12), 16, 16, 91);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.9f;
				Main.dust[num1].velocity += direction;
				Main.dust[num1].velocity.Y *= 3f;
				Main.dust[num1].velocity.X *= 1.5f;
				Main.dust[num1].scale = 1.65f;

				num1 = Dust.NewDust(center - new Vector2(12, 12), 16, 16, 198);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.75f;
				Main.dust[num1].velocity += direction;
				Main.dust[num1].velocity.Y *= 2f;
				Main.dust[num1].velocity.X *= 1f;
				Main.dust[num1].scale = 2f;
			}
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
	}
}