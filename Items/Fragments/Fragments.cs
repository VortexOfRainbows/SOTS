using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class FragmentOfNature : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Nature");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = 1;
			item.maxStack = 999;
		}
	}
	public class FragmentOfEarth : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Earth");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}
	}
	public class FragmentOfPermafrost : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Permafrost");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}
	}
	public class FragmentOfTide : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Tide");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}
	}
	public class FragmentOfOtherworld : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Otherworld");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 26;
			item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}
	}
	public class FragmentOfEvil : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Evil");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 26;
			item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = WorldGen.crimson ? mod.GetTexture("Items/Fragments/FragmentOfEvil") : mod.GetTexture("Items/Fragments/FragmentOfEvilAlt");
			spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = WorldGen.crimson ? mod.GetTexture("Items/Fragments/FragmentOfEvil") : mod.GetTexture("Items/Fragments/FragmentOfEvilAlt");
			spriteBatch.Draw(texture, item.Center - Main.screenPosition, null, lightColor, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
			return false;
		}
	}
	public class FragmentOfChaos : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Chaos");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 36;
			item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}
	}
	public class FragmentOfInferno : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Inferno");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 36;
			item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = 1;
			item.maxStack = 999;
		}
	}
}