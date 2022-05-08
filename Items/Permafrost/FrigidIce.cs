using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class FrigidIceTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileSpelunker[Type] = true;
			Main.tileShine[Type] = 1000;
			Main.tileShine2[Type] = true;
			Main.tileValue[Type] = 420; //above gold
			minPick = 45; //requires silver to mine
			mineResist = 0.5f;
			dustType = ModContent.DustType<ModIceDust>();
			drop = ModContent.ItemType<FrigidIce>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Frigid Ore");
			AddMapEntry(new Color(96, 111, 215), name);
			soundType = SoundLoader.customSoundType;
			soundStyle = mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/FrigidOre1");
		}
		public override bool KillSound(int i, int j)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			int type = Main.rand.Next(2) + 1;
			SoundEngine.PlaySound(SoundLoader.customSoundType, (int)pos.X, (int)pos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/FrigidOre" + type), 2f, Main.rand.NextFloat(0.9f, 1.1f));
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
						Projectile.NewProjectile(new Vector2(i * 16 + 8, j * 16 + 8), new Vector2(0, -0.25f) + Main.rand.NextVector2Circular(1.5f, 1.5f), ModContent.ProjectileType<FrigidIceShard>(), 20, 3, Main.myPlayer); //40 damage normal mode, 80 expert
						noItem = true;
					}
				}
			}
        }
    }
	public class FrigidIceTileSafe : ModTile
	{
        public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SOTS/Items/Permafrost/FrigidIceTile";
			return base.Autoload(ref name, ref texture);
        }
        public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			minPick = 45; //requires silver to mine
			mineResist = 0.5f;
			dustType = ModContent.DustType<ModIceDust>();
			drop = ModContent.ItemType<FrigidIce>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Frigid Ore");
			AddMapEntry(new Color(96, 111, 215), name);
			soundType = SoundLoader.customSoundType;
			soundStyle = mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/FrigidOre");
		}
		public override bool KillSound(int i, int j)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			SoundEngine.PlaySound(SoundLoader.customSoundType, (int)pos.X, (int)pos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/FrigidOre"), 2f, Main.rand.NextFloat(0.9f, 1.1f));
			return false;
		}
	}
	public class FrigidIce : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Ore");
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
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			dustType = ModContent.DustType<ModIceDust>();
			drop = ModContent.ItemType<FrigidBrick>();
			AddMapEntry(new Color(96, 111, 215));
			soundType = SoundLoader.customSoundType;
			soundStyle = mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/FrigidOre");
		}
	}
	public class FrigidBrick : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<FrigidBrickTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FrigidIce>(), 1);
			recipe.AddIngredient(ItemID.IceBlock, 1);
			recipe.AddTile(TileID.IceMachine);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
	public class FrigidBrickWallTile : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = 122;
			drop = ModContent.ItemType<FrigidBrickWall>();
			AddMapEntry(new Color(74, 85, 160));
		}
	}
	public class FrigidBrickWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Brick Wall");
			Tooltip.SetDefault("");
		}
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FrigidBrick>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<FrigidBrick>(), 1);
			recipe.AddRecipe();
		}
	}
}