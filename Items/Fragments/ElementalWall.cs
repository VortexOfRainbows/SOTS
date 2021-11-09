using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Fragments
{
	public abstract class ElementalWall : ModItem
	{
		public sealed override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneWall);
			item.rare = ItemRarityID.Orange;
			item.createWall = mod.WallType("NatureWallWall");
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults() { }
	}
	public class NatureWall : ElementalWall
	{
		public override void SafeSetDefaults()
		{
			item.createWall = mod.WallType("NatureWallWall");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingNatureBlock", 1);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.SetResult(ModContent.ItemType<DissolvingNatureBlock>(), 1);
			recipe.AddRecipe();
		}
	}
	public class NatureWallWall : ModWall
	{
		public Color color = new Color(45, 130, 60);
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = mod.ItemType("NatureWall");
			AddMapEntry(color);
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 5;
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, 267);
			dust.color = color;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.8f;
			dust.velocity *= 2.4f;
			return false;
		}
	}
	public class EarthWall : ElementalWall
	{
		public override void SafeSetDefaults()
		{
			item.createWall = mod.WallType("EarthWallWall");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingEarthBlock", 1);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.SetResult(ModContent.ItemType<DissolvingEarthBlock>(), 1);
			recipe.AddRecipe();
		}
	}
	public class EarthWallWall : ModWall
	{
		public Color color = new Color(190, 145, 0);
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = mod.ItemType("EarthWall");
			AddMapEntry(color);
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 5;
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, 267);
			dust.color = color;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.8f;
			dust.velocity *= 2.4f;
			return false;
		}
	}
	public class DelugeWall : ElementalWall
	{
		public override void SafeSetDefaults()
		{
			item.createWall = mod.WallType("DelugeWallWall");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingDelugeBlock", 1);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.SetResult(ModContent.ItemType<DissolvingDelugeBlock>(), 1);
			recipe.AddRecipe();
		}
	}
	public class DelugeWallWall : ModWall
	{
		public Color color = new Color(50, 55, 135);
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = mod.ItemType("DelugeWall");
			AddMapEntry(color);
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 5;
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, 267);
			dust.color = color;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.8f;
			dust.velocity *= 2.4f;
			return false;
		}
	}
	public class AetherWall : ElementalWall
	{
		public override void SafeSetDefaults()
		{
			item.createWall = mod.WallType("AetherWallWall");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingAetherBlock", 1);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.SetResult(ModContent.ItemType<DissolvingAetherBlock>(), 1);
			recipe.AddRecipe();
		}
	}
	public class AetherWallWall : ModWall
	{
		public Color color = new Color(130, 25, 180);
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = mod.ItemType("AetherWall");
			AddMapEntry(color);
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 5;
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, 267);
			dust.color = color;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.8f;
			dust.velocity *= 2.4f;
			return false;
		}
	}
	public class AuroraWall : ElementalWall
	{
		public override void SafeSetDefaults()
		{
			item.createWall = mod.WallType("AuroraWallWall");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingAuroraBlock", 1);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.SetResult(ModContent.ItemType<DissolvingAuroraBlock>(), 1);
			recipe.AddRecipe();
		}
	}
	public class AuroraWallWall : ModWall
	{
		public Color color = new Color(35, 70, 85);
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = mod.ItemType("AuroraWall");
			AddMapEntry(color);
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 5;
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, 267);
			dust.color = color;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.8f;
			dust.velocity *= 2.4f;
			return false;
		}
	}
	public class UmbraWallWall : ModWall
	{
		public Color color = new Color(190, 25, 0);
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = ModContent.ItemType<UmbraWall>();
			AddMapEntry(color);
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 5;
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, 267);
			dust.color = color;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.8f;
			dust.velocity *= 2.4f;
			return false;
		}
	}
	public class UmbraWall : ElementalWall
	{
		public override void SafeSetDefaults()
		{
			item.createWall = ModContent.WallType<UmbraWallWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DissolvingUmbraBlock>(), 1);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.SetResult(ModContent.ItemType<DissolvingUmbraBlock>(), 1);
			recipe.AddRecipe();
		}
	}
}