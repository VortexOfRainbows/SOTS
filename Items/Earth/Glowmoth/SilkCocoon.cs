using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Pyramid;
using SOTS.Void;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Earth.Glowmoth
{
	public class GlowmothBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("A slab from an ancient burial site, it may be hard to break");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.LightRed;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<SilkCocoonTile>();
		}
	}
	public class SilkCocoonTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 3, 0); 
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.WaterDeath = false;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(30, 90, 180), name);
			DustType = DustID.Silk;
			HitSound = SoundID.NPCHit18;
		}
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return true;
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			SOTSTile.DrawSlopedGlowMask(i, j, Type, ModContent.Request<Texture2D>("SOTS/Items/Earth/Glowmoth/SilkCocoonTileGlow").Value, Color.White, new Vector2(0, 2));
		}
        public override void RandomUpdate(int i, int j)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && Main.rand.NextBool(21))
			{
				Tile tile = Main.tile[i, j];
				int left = i - (tile.TileFrameX / 18) % 3;
				int top = j - (tile.TileFrameY / 18) % 3;
				for (int x = left; x < left + 3; x++)
				{
					for (int y = top; y < top + 3; y++)
					{
						if (Main.tile[x, y].TileFrameY < 108)
							Main.tile[x, y].TileFrameY += 54;
					}
				}
				NetMessage.SendTileSquare(-1, left + 1, top + 1, 3);
			}
			base.RandomUpdate(i, j);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Tile tile = Main.tile[i, j];
			int left = i - (tile.TileFrameX / 18) % 3;
			int top = j - (tile.TileFrameY / 18) % 3;
			left += 1;
			top += 1;
			if(Main.tile[left, top].TileFrameY >= 108)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient && fail)
				{
					Projectile.NewProjectile(new EntitySource_Misc("SOTS:BreakSilkCocoon"), new Vector2(left, top) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<SilkCocoonProjectile>(), 0, 0, Main.myPlayer);
				}
			}
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
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Vector3 vColor = ColorHelpers.VibrantColor.ToVector3() * 0.65f;
			r = vColor.X;
			g = vColor.Y;
			b = vColor.Z;
		}
	}
	public class SilkCocoonProjectile : ModProjectile
	{
        public override string Texture => "SOTS/Items/Earth/Glowmoth/SilkCocoonTile";
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public override void SetDefaults()
		{
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 480;
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.hide = true;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.width, Projectile.position.Y - Projectile.height) - new Vector2(5), Projectile.width * 3, Projectile.height * 3, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[num2];
				dust.color = ColorHelpers.VibrantColorAttempt(Main.rand.NextFloat(360));
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.75f;
				dust.velocity += Projectile.velocity;
				
				dust = Dust.NewDustDirect(new Vector2(Projectile.position.X - Projectile.width, Projectile.position.Y - Projectile.height) - new Vector2(5), Projectile.width * 3, Projectile.height * 3, DustID.Silk);
				dust.scale *= 1.25f;
			}
		}
		public override bool ShouldUpdatePosition()
		{
			return true;
		}
		public override void AI()
		{
			int i = (int)Projectile.Center.X / 16;
			int j = (int)Projectile.Center.Y / 16;
			Tile current = Framing.GetTileSafely(i, j);
			if (!current.HasTile || current.TileType != ModContent.TileType<SilkCocoonTile>() || current.TileFrameY < 108)
			{
				Projectile.Kill();
				Projectile.active = false;
			}
			Projectile.timeLeft = 7200;
			int range = 1;
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int x = -range; x <= range; x++)
				{
					for (int y = -range; y <= range; y++)
					{
						if (Main.tile[i + x, j + y].TileFrameY >= 108)
							Main.tile[i + x, j + y].TileFrameY = (short)(Main.tile[i + x, j + y].TileFrameY % 54);
						if (Main.netMode == NetmodeID.Server)
							NetMessage.SendTileSquare(-1, i, j, 3);

					}
				}
				int item = Item.NewItem(Projectile.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<GlowSilk>(), Main.rand.Next(25, 41), false, 0, true);
				NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0.0f, 0.0f, 0, 0, 0);
			}
			SOTSUtils.PlaySound(SoundID.NPCDeath1, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.10f, -0.2f);
			Projectile.Kill();
			Projectile.active = false;
		}
	}
}