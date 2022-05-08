using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
namespace SOTS.Items.OreItems
{
	public class PlatinumDart : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Dart");
			Tooltip.SetDefault("A heavy dart that sticks to enemies, slowing them down drastically");
		}
		public override void SafeSetDefaults()
		{
			Item.CloneDefaults(279);
			Item.damage = 15;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.ranged = true;
			Item.thrown = false;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = 1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<Projectiles.Ores.PlatinumDart>(); 
            Item.shootSpeed = 14f;
			Item.consumable = false;
		}
		public override int GetVoid(Player player)
		{
			return 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PlatinumBar, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}