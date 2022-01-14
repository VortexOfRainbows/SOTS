using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Otherworld.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public abstract class PlatingTile : ModTile
	{
		public virtual Texture2D glowTexture => mod.GetTexture("Items/Fragments/PermafrostPlatingTileGlow");
		public sealed override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<PermafrostPlating>();
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
		{
			AddMapEntry(new Color(165, 179, 198));
			mineResist = 1.5f;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = DustID.Silver;
		}
		public virtual bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.frameX / 18;
			int frameY = tile.frameY / 18;
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
		public void DrawLights(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].color()) * (100f / 255f);
			color.A = 0;
			float uniquenessCounter = Main.GlobalTime * -100 + (i + j) * 5;
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			for (int k = 0; k < 3; k++)
			{
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.25f * k;
				SOTSTile.DrawSlopedGlowMask(i, j, tile.type, glowTexture, color * alphaMult * 0.6f, offset);
			}
		}
	}
	public class NaturePlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Plating");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<NaturePlatingTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfNature>(), 1);
			recipe.AddRecipeGroup("SOTS:PHMOre", 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
	public class NaturePlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => mod.GetTexture("Items/Fragments/NaturePlatingTileGlow");
		public override void SafeSetDefaults()
		{
			drop = ModContent.ItemType<NaturePlating>();
			AddMapEntry(new Color(158, 177, 171));
			mineResist = 1.5f;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = DustID.Tungsten;
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.frameX / 18;
			int frameY = tile.frameY / 18;
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
			DisplayName.SetDefault("Earthen Plating");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<EarthenPlatingTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEarth>(), 1);
			recipe.AddRecipeGroup("SOTS:PHMOre", 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
	public class EarthenPlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => mod.GetTexture("Items/Fragments/EarthenPlatingTileGlow");
		public override void SafeSetDefaults()
		{
			drop = ModContent.ItemType<EarthenPlating>();
			AddMapEntry(new Color(112, 90, 86));
			mineResist = 1.5f;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = DustID.Iron;
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.frameX / 18;
			int frameY = tile.frameY / 18;
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
			DisplayName.SetDefault("Permafrost Plating");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<PermafrostPlatingTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfPermafrost>(), 1);
			recipe.AddRecipeGroup("SOTS:PHMOre", 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
	public class PermafrostPlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => mod.GetTexture("Items/Fragments/PermafrostPlatingTileGlow");
		public override void SafeSetDefaults()
		{
			drop = ModContent.ItemType<PermafrostPlating>();
			AddMapEntry(new Color(165, 179, 198));
			mineResist = 1.5f;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = DustID.Silver;
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.frameX / 18;
			int frameY = tile.frameY / 18;
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
				r = 0.225f;
				g = 0.3f;
				b = 0.3f;
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
			DisplayName.SetDefault("Tide Plating");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<TidePlatingTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfTide>(), 1);
			recipe.AddRecipeGroup("SOTS:PHMOre", 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
	public class TidePlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => mod.GetTexture("Items/Fragments/TidePlatingTileGlow");
		public override void SafeSetDefaults()
		{
			drop = ModContent.ItemType<TidePlating>();
			AddMapEntry(new Color(35, 37, 52));
			mineResist = 1.5f;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = DustID.Lead; //demonite
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.frameX / 18;
			int frameY = tile.frameY / 18;
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
			DisplayName.SetDefault("Evil Plating");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<EvilPlatingTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 1);
			recipe.AddRecipeGroup("SOTS:PHMOre", 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
	public class EvilPlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => mod.GetTexture("Items/Fragments/EvilPlatingTileGlow");
		public override void SafeSetDefaults()
		{
			drop = ModContent.ItemType<EvilPlating>();
			AddMapEntry(new Color(98, 47, 126));
			mineResist = 1.5f;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = 14; //demonite
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.frameX / 18;
			int frameY = tile.frameY / 18;
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
			DisplayName.SetDefault("Chaos Plating");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<ChaosPlatingTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfChaos>(), 1);
			recipe.AddRecipeGroup("SOTS:PHMOre", 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
	public class ChaosPlatingTile : PlatingTile
	{
		public override Texture2D glowTexture => mod.GetTexture("Items/Fragments/ChaosPlatingTileGlow");
		public override void SafeSetDefaults()
		{
			drop = ModContent.ItemType<ChaosPlating>();
			AddMapEntry(new Color(82, 85, 123));
			mineResist = 1.5f;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = DustID.Platinum; //demonite
		}
		public override bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.frameX / 18;
			int frameY = tile.frameY / 18;
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
}