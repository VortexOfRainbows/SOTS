using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.Permafrost;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class FrigidIceTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileSpelunker[Type] = true;
			Main.tileShine[Type] = 1000;
			Main.tileShine2[Type] = true;
			Main.tileOreFinderPriority[Type] = 420; //above gold
			MinPick = 45; //requires silver to mine
			MineResist = 0.5f;
			DustType = ModContent.DustType<ModIceDust>();
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<FrigidIce>();
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(100, 173, 232), name);
			HitSound = new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/FrigidOre1");
		}
		public override bool KillSound(int i, int j, bool fail)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			int type = Main.rand.Next(2) + 1;
			SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/FrigidOre" + type), (int)pos.X, (int)pos.Y, 2f, Main.rand.NextFloat(0.9f, 1.1f));
			return false;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
			if(!fail && !effectOnly && !noItem && Main.rand.NextBool(3))
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int amt = 2 + Main.rand.Next(3);
					for (int a = 0; a < amt; a++)
					{
						Projectile.NewProjectile(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8, j * 16 + 8), new Vector2(0, -0.25f) + Main.rand.NextVector2Circular(1.5f, 1.5f), ModContent.ProjectileType<FrigidIceShard>(), 20, 3, Main.myPlayer); //40 damage normal mode, 80 expert
						noItem = true;
					}
				}
			}
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			SOTS.MergeWithFrame(i, j, Type, TileID.SnowBlock, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
	}
	public class FrigidIceTileSafe : ModTile
	{
        public override string Texture => "SOTS/Items/Permafrost/FrigidIceTile";
        public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBrick[Type] = true;
			MinPick = 45; //requires silver to mine
			MineResist = 0.5f;
			DustType = ModContent.DustType<ModIceDust>();
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<FrigidIce>();
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(96, 111, 215), name);
			HitSound = new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/FrigidOre1");
		}
		public override bool KillSound(int i, int j, bool fail)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			int type = Main.rand.Next(2) + 1;
			SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/FrigidOre" + type), (int)pos.X, (int)pos.Y, 2f, Main.rand.NextFloat(0.9f, 1.1f));
			return false;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			SOTS.MergeWithFrame(i, j, Type, TileID.SnowBlock, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
	}
	public class FrigidIce : ModItem
	{
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 3, 0);
			Item.createTile = ModContent.TileType<FrigidIceTileSafe>();
		}
	}
	public class FrigidBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			DustType = ModContent.DustType<ModIceDust>();
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<FrigidBrick>();
			AddMapEntry(new Color(96, 111, 215));
			HitSound = new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/FrigidOre1");
		}
		public override bool KillSound(int i, int j, bool fail)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			int type = Main.rand.Next(2) + 1;
			SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/FrigidOre" + type), (int)pos.X, (int)pos.Y, 2f, Main.rand.NextFloat(0.9f, 1.1f));
			return false;
		}
	}
	public class FrigidBrick : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(100);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<FrigidBrickTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ModContent.ItemType<FrigidIce>(), 1).AddIngredient(ItemID.IceBlock, 1).AddTile(TileID.IceMachine).Register();
		}
	}
	public class FrigidBrickWallTile : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = 122;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<FrigidBrickWall>();
			AddMapEntry(new Color(74, 85, 160));
		}
	}
	public class FrigidBrickWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<FrigidBrickWallTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<FrigidBrick>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<FrigidBrick>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
}