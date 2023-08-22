using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Planetarium.Blocks;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Planetarium.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public abstract class PlatingTile : ModTile
	{
		public virtual Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Fragments/PermafrostPlatingTileGlow").Value;
		public sealed override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true; 
			Main.tileMerge[Type][TileID.Dirt] = true;
			Main.tileMerge[TileID.Dirt][Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<PermafrostPlating>();
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
		{
			AddMapEntry(new Color(165, 179, 198));
			MineResist = 1.5f;
			HitSound = SoundID.Tink;
			DustType = DustID.Silver;
		}
		public virtual bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if (frameX == 5 && (frameY >= 0 && frameY <= 2))
			{
				return false;
			}
			if (frameX >= 6 && frameX <= 8 && (frameY == 0 || frameY == 3))
				return false;
			return true;
		}
		public sealed override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if(canGlow(i, j))
				DrawLights(i, j, spriteBatch);
		}
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public void DrawLights(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			for (int k = 0; k < 3; k++)
			{
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.25f * k;
				SOTSTile.DrawSlopedGlowMask(i, j, tile.TileType, glowTexture, color * alphaMult * 0.6f, offset);
			}
		}
	}
	public class NaturePlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<NaturePlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(5).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 1).AddRecipeGroup("SOTS:PHMOre", 1).AddTile(TileID.HeavyWorkBench).Register();
		}
	}
	public class NaturePlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Fragments/NaturePlatingTileGlow").Value;
		public override void SafeSetDefaults()
		{
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<NaturePlating>();
			AddMapEntry(SOTSTile.NaturePlatingColor);
			MineResist = 1.5f;
			HitSound = SoundID.Tink;
			DustType = DustID.Tungsten;
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if (frameX >= 1 && frameX <= 3 && (frameY == 0 || frameY == 2))
			{
				return true;
			}
			if (frameX >= 0 && frameX <= 5 && (frameY == 3 || frameY == 4))
				return true;
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (canGlow(i, j))
			{
				r = 0.275f;
				g = 0.4f;
				b = 0.215f;
			}
			else
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}
	}
	public class EarthenPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<EarthenPlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(5).AddIngredient(ModContent.ItemType<FragmentOfEarth>(), 1).AddRecipeGroup("SOTS:PHMOre", 1).AddTile(TileID.HeavyWorkBench).Register();
		}
	}
	public class EarthenPlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Fragments/EarthenPlatingTileGlow").Value;
		public override void SafeSetDefaults()
		{
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<EarthenPlating>();
			AddMapEntry(SOTSTile.EarthenPlatingColor);
			MineResist = 1.5f;
			HitSound = SoundID.Tink;
			DustType = DustID.Iron;
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if (frameX == 0 || frameX == 4 || frameX == 5)
			{
				return true;
			}
			if (frameX >= 0 && frameX <= 5 && (frameY == 3 || frameY == 4))
				return true;
			if (frameX >= 6 && frameX <= 8 && (frameY == 0 || frameY == 3))
				return true;
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (canGlow(i, j))
			{
				r = 0.36f;
				g = 0.32f;
				b = 0.11f;
			}
			else
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}
	}
	public class PermafrostPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<PermafrostPlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(5).AddIngredient(ModContent.ItemType<FragmentOfPermafrost>(), 1).AddRecipeGroup("SOTS:PHMOre", 1).AddTile(TileID.HeavyWorkBench).Register();
		}
	}
	public class PermafrostPlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Fragments/PermafrostPlatingTileGlow").Value;
		public override void SafeSetDefaults()
		{
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<PermafrostPlating>();
			AddMapEntry(SOTSTile.PermafrostPlatingColor);
			MineResist = 1.5f;
			HitSound = SoundID.Tink;
			DustType = DustID.Silver;
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if (frameY == 0 && ((frameX >= 0 && frameX <= 4) || (frameX == 10 || frameX == 11)))
			{
				return true;
			}
			if ((frameY == 1 || frameY == 2) && ((frameX >= 0 && frameX <= 4) || (frameX >= 6 && frameX <= 8) || (frameX == 10 || frameX == 11)))
			{
				return true;
			}
			if ((frameY == 3 || frameY == 4) && (frameX >= 0 && frameX <= 5))
			{
				return true;
			}
			return false;
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			if(canGlow(i, j))
			{
				r = SOTSTile.PermafrostPlatingLight.X;
				g = SOTSTile.PermafrostPlatingLight.Y;
				b = SOTSTile.PermafrostPlatingLight.Z;
			}
			else
            {
				r = 0;
				g = 0;
				b = 0;
            }
        }
	}
	public class OtherworldPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<OtherworldPlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(5).AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 1).AddRecipeGroup("SOTS:PHMOre", 1).AddTile(TileID.HeavyWorkBench).Register();
		}
	}
	public class OtherworldPlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Fragments/OtherworldPlatingTileGlow").Value;
		public override void SafeSetDefaults()
		{
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<OtherworldPlating>();
			AddMapEntry(new Color(48, 57, 70));
			MineResist = 1.5f;
			HitSound = SoundID.Tink;
			DustType = DustID.Lead; //demonite
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if (frameX >= 1 && frameX <= 3 && frameY == 2)
				return false;
			if ((frameX == 9 || frameX == 12) && frameY >= 0 && frameY <= 2)
				return false;
			if (frameX >= 6 && frameY >= 3)
				return false;
			return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (canGlow(i, j))
			{
				r = 0.19f;
				g = 0.095f;
				b = 0.21f;
			}
			else
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}
	}
	public class TidePlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<TidePlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(5).AddIngredient(ModContent.ItemType<FragmentOfTide>(), 1).AddRecipeGroup("SOTS:PHMOre", 1).AddTile(TileID.HeavyWorkBench).Register();
		}
	}
	public class TidePlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Fragments/TidePlatingTileGlow").Value;
		public override void SafeSetDefaults()
		{
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<TidePlating>();
			AddMapEntry(new Color(35, 37, 52));
			MineResist = 1.5f;
			HitSound = SoundID.Tink;
			DustType = DustID.Lead; //demonite
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if (frameX >= 6 && frameX <= 8 && (frameY == 3 || frameY == 4))
				return false;
			if ((frameX == 9 || frameX == 12) && frameY >= 0 && frameY <= 2)
				return false;
			if (frameX >= 9 && frameX <= 11 && frameY == 3)
				return false;
			if (frameY == 4)
				return false;
			if (frameY == 2 && frameX >= 1 && frameX <= 3)
				return false;
			return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (canGlow(i, j))
			{
				r = 0.1f;
				g = 0.12f;
				b = 0.30f;
			}
			else
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}
	}
	public class EvilPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<EvilPlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(5).AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 1).AddRecipeGroup("SOTS:PHMOre", 1).AddTile(TileID.HeavyWorkBench).Register();
		}
	}
	public class EvilPlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Fragments/EvilPlatingTileGlow").Value;
		public override void SafeSetDefaults()
		{
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<EvilPlating>();
			AddMapEntry(new Color(98, 47, 126));
			MineResist = 1.5f;
			HitSound = SoundID.Tink;
			DustType = 14; //demonite
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if (frameY == 0 && ((frameX >= 1 && frameX <= 3) || frameX == 10 || frameX == 11))
			{
				return true;
			}
			if (frameY == 1 && ((frameX >= 1 && frameX <= 3) || (frameX >= 6 && frameX <= 8) || frameX == 10 || frameX == 11))
				return true;
			if (frameY == 2 && ((frameX >= 6 && frameX <= 8) || frameX == 10 || frameX == 11))
				return true;
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (canGlow(i, j))
			{
				r = 0.4f;
				g = 0.04f;
				b = 0.04f;
			}
			else
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}
	}
	public class ChaosPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<ChaosPlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(5).AddIngredient(ModContent.ItemType<FragmentOfChaos>(), 1).AddRecipeGroup("SOTS:PHMOre", 1).AddTile(TileID.HeavyWorkBench).Register();
		}
	}
	public class ChaosPlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Fragments/ChaosPlatingTileGlow").Value;
		public override void SafeSetDefaults()
		{
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<ChaosPlating>();
			AddMapEntry(new Color(82, 85, 123));
			MineResist = 1.5f;
			HitSound = SoundID.Tink;
			DustType = DustID.Platinum;
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if (frameX >= 1 && frameX <= 3 && frameY == 1)
			{
				return false;
			}
			if (frameX >= 6 && frameX <= 8 && (frameY == 1 || frameY == 2 || frameY == 4))
			{
				return false;
			}
			if ((frameX == 10 || frameX == 11) && frameY >= 0 && frameY <= 2)
			{
				return false;
			}
			return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (canGlow(i, j))
			{
				r = 0.23f;
				g = 0.09f;
				b = 0.20f;
			}
			else
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}
	}
	public class InfernoPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.height = 22;
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<InfernoPlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(5).AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 1).AddRecipeGroup("SOTS:PHMOre", 1).AddTile(TileID.HeavyWorkBench).Register();
		}
	}
	public class InfernoPlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Fragments/InfernoPlatingTileGlow").Value;
		public override void SafeSetDefaults()
		{
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<InfernoPlating>();
			AddMapEntry(new Color(73, 35, 59));
			MineResist = 1.5f;
			HitSound = SoundID.Tink;
			DustType = DustID.Iron;
		}
        public override bool CreateDust(int i, int j, ref int type)
		{
			type = DustType;
			if (Main.rand.NextBool(3))
				type = DustID.Torch;
			return true;
        }
        public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if ((frameX == 5 || frameX == 12) && frameY >= 0 && frameY <= 2)
			{
				return false;
			}
			if (frameX >= 6 && frameX <= 8 && frameY == 0)
			{
				return false;
			}
			if (frameX >= 9 && frameX <= 11 && frameY == 3)
			{
				return false;
			}
			return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (canGlow(i, j))
			{
				r = 0.213f;
				g = 0.068f;
				b = 0.013f;
			}
			else
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}
	}
	public class UltimatePlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<UltimatePlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(15).AddIngredient<NaturePlating>(1)
				.AddIngredient<EarthenPlating>(1)
				.AddIngredient<PermafrostPlating>(1)
				.AddIngredient<OtherworldPlating>(1)
				.AddIngredient<TidePlating>(1)
				.AddIngredient<EvilPlating>(1)
				.AddIngredient<ChaosPlating>(1)
				.AddIngredient<InfernoPlating>(1)
				.AddTile(TileID.HeavyWorkBench).Register();
		}
	}
	public class UltimatePlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Fragments/UltimatePlatingTileGlow").Value;
		public override void SafeSetDefaults()
		{
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<UltimatePlating>();
			AddMapEntry(new Color(82, 85, 123));
			MineResist = 1.5f;
			HitSound = SoundID.Tink;
			DustType = 146; //Titanium
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			type = DustType;
			if (Main.rand.NextBool(3))
				type = 63; //white torch
			return true;
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if (frameX >= 1 && frameX <= 3 && frameY == 2)
			{
				return false;
			}
			if (frameX >= 6 && frameX <= 8 && frameY == 4)
			{
				return false;
			}
			if ((frameY >= 0 && frameY <= 2) && (frameX == 9 || frameX == 12))
			{
				return false;
			}
			return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (canGlow(i, j))
			{
				r = 0.2f;
				g = 0.2f;
				b = 0.2f;
			}
			else
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}
	}
}