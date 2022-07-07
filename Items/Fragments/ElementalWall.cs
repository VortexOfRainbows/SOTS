using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Fragments
{
	public abstract class ElementalWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
		public sealed override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.rare = ItemRarityID.Orange;
			Item.createWall = ModContent.WallType<NatureWallWall>();
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults() { }
	}
	public class NatureWall : ElementalWall
	{
		public override void SafeSetDefaults()
		{
			Item.createWall = ModContent.WallType<NatureWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DissolvingNatureBlock>(), 1).Register();
			Recipe.Create(ModContent.ItemType<DissolvingNatureBlock>()).AddIngredient(this, 4).Register();
		}
	}
	public class NatureWallWall : ModWall
	{
		public Color color = new Color(45, 130, 60);
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<NatureWall>();
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
			Item.createWall = ModContent.WallType<EarthWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DissolvingEarthBlock>(), 1).Register();
			Recipe.Create(ModContent.ItemType<DissolvingEarthBlock>()).AddIngredient(this, 4).Register();
		}
	}
	public class EarthWallWall : ModWall
	{
		public Color color = new Color(190, 145, 0);
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<EarthWall>();
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
			Item.createWall = ModContent.WallType<DelugeWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DissolvingDelugeBlock>(), 1).Register();
			Recipe.Create(ModContent.ItemType<DissolvingDelugeBlock>()).AddIngredient(this, 4).Register();
		}
	}
	public class DelugeWallWall : ModWall
	{
		public Color color = new Color(50, 55, 135);
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<DelugeWall>();
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
			Item.createWall = ModContent.WallType<AetherWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DissolvingAetherBlock>(), 1).Register();
			Recipe.Create(ModContent.ItemType<DissolvingAetherBlock>()).AddIngredient(this, 4).Register();
		}
	}
	public class AetherWallWall : ModWall
	{
		public Color color = new Color(130, 25, 180);
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<AetherWall>();
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
			Item.createWall = ModContent.WallType<AuroraWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DissolvingAuroraBlock>(), 1).Register();
			Recipe.Create(ModContent.ItemType<DissolvingAuroraBlock>()).AddIngredient(this, 4).Register();
		}
	}
	public class AuroraWallWall : ModWall
	{
		public Color color = new Color(35, 70, 85);
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<AuroraWall>();
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
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<UmbraWall>();
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
			Item.createWall = ModContent.WallType<UmbraWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DissolvingUmbraBlock>(), 1).Register();
			Recipe.Create(ModContent.ItemType<DissolvingUmbraBlock>()).AddIngredient(this, 4).Register();
		}
	}
	public class NetherWallWall : ModWall
	{
		public Color color = new Color(155, 50, 9);
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<UmbraWall>();
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
	public class NetherWall : ElementalWall
	{
		public override void SafeSetDefaults()
		{
			Item.createWall = ModContent.WallType<NetherWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DissolvingNetherBlock>(), 1).Register();
			Recipe.Create(ModContent.ItemType<DissolvingNetherBlock>()).AddIngredient(this, 4).Register();
		}
	}
	public class BrillianceWall : ElementalWall
	{
		public override void SafeSetDefaults()
		{
			Item.createWall = ModContent.WallType<BrillianceWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DissolvingBrillianceBlock>(), 1).Register();
			Recipe.Create(ModContent.ItemType<DissolvingBrillianceBlock>()).AddIngredient(this, 4).Register();
		}
	}
	public class BrillianceWallWall : ModWall
	{
		public Color color = new Color(231, 95, 203);
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<BrillianceWall>();
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
}