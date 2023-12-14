using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Curse;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid
{
	public class PyramidGate : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.consumable = true;
			Item.createTile = ModContent.TileType<PyramidGateTile>();
			Item.placeStyle = 0;
		}
	}	
	public class PyramidGateTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.DrawsWalls[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
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
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(220, 180, 25), name);
			DustType = DustID.GoldCoin;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Tile tile = Main.tile[i, j];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Pyramid/PyramidGateTile").Value;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			Color color = WorldGen.paintColor((int)Main.tile[i, j].TileColor);
			color = Lighting.GetColor(i, j, color);
			Vector2 pos = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
			Main.spriteBatch.Draw(texture, pos, frame, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.cursorItemIconID = ModContent.ItemType<PyramidKey>();
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}
		public override void MouseOverFar(int i, int j)
		{
			Player player = Main.LocalPlayer;
			MouseOver(i, j);
			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}
        public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i - tile.TileFrameX / 18;
			int top = j - tile.TileFrameY / 18;
			Main.mouseRightRelease = false;
			int key = ModContent.ItemType<PyramidKey>();
			if (NPC.downedBoss2 && player.ConsumeItem(key))
			{
				Projectile.NewProjectile(new EntitySource_TileInteraction(player, i, j), new Vector2(left, top) * 16 + new Vector2(40, 8), Vector2.Zero, ModContent.ProjectileType<PyramidGateProj>(), 0, 0, Main.myPlayer);
			}
			return true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
        public override bool CanDrop(int i, int j)
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
	}
	public class PyramidGateProj : ModProjectile
	{
		public override string Texture => "SOTS/Items/Pyramid/PyramidGate";
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindNPCsAndTiles.Add(index);
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Multiplayer Code 5000");
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public override void SetDefaults()
		{
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 5;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.hide = true;
		}
        public override void AI()
        {
			Projectile.Kill();
        }
        public override void OnKill(int timeLeft)
		{
			int i = (int)Projectile.Center.X / 16;
			int j = (int)Projectile.Center.Y / 16;
			WorldGen.KillTile(i, j, false, false, false);
			if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
			Vector2 center = Projectile.Center + new Vector2(0, -16);
			SOTSUtils.PlaySound(SoundID.Item62, (int)center.X, (int)center.Y, 1.25f, -0.5f);
			if(Main.netMode != NetmodeID.Server)
			{
				for (int k = 0; k < 16; k++)
				{
					int goreIndex2 = Gore.NewGore(Projectile.GetSource_Death(), center - new Vector2(16, 40) + new Vector2(Main.rand.NextFloat(-16, 16f), Main.rand.NextFloat(-16, 64f)), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex2].scale = 0.9f;
				}
				int goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(i * 16, j * 16), Vector2.Zero, ModGores.GoreType("Gores/Tiles/PyramidGateGore1"), 1f);
				Main.gore[goreIndex].velocity *= 0.2f;
				goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(i * 16 - 16, j * 16), Vector2.Zero, ModGores.GoreType("Gores/Tiles/PyramidGateGore2"), 1f);
				Main.gore[goreIndex].velocity *= 0.2f;
				goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(i * 16 + 16, j * 16), Vector2.Zero, ModGores.GoreType("Gores/Tiles/PyramidGateGore3"), 1f);
				Main.gore[goreIndex].velocity *= 0.2f;
				goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(i * 16 + 32, j * 16), Vector2.Zero, ModGores.GoreType("Gores/Tiles/PyramidGateGore4"), 1f);
				Main.gore[goreIndex].velocity *= 0.2f;
				goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(i * 16 - 32, j * 16), Vector2.Zero, ModGores.GoreType("Gores/Tiles/PyramidGateGore5"), 1f);
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

				num1 = Dust.NewDust(center - new Vector2(12, 12), 16, 16, DustID.FireflyHit);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.15f;
				Main.dust[num1].velocity += direction;
				Main.dust[num1].velocity.Y *= 4;
				Main.dust[num1].velocity.X *= 2.0f;
				Main.dust[num1].scale = 1.2f;

				num1 = Dust.NewDust(center - new Vector2(12, 12), 16, 16, DustID.GemDiamond);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.9f;
				Main.dust[num1].velocity += direction;
				Main.dust[num1].velocity.Y *= 3f;
				Main.dust[num1].velocity.X *= 1.5f;
				Main.dust[num1].scale = 1.65f;

				num1 = Dust.NewDust(center - new Vector2(12, 12), 16, 16, DustID.FireflyHit);
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