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
			Tooltip.SetDefault("A heavy dart that sticks to enemies");
		}
		public override void SafeSetDefaults()
		{
			
			item.CloneDefaults(279);
			item.damage = 20;
			item.useTime = 22;
			item.useAnimation = 22;
			item.ranged = true;
			item.thrown = false;
			item.value = Item.sellPrice(0, 0, 35, 0);
			item.rare = 2;
			item.width = 26;
			item.height = 26;
			item.maxStack = 1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("PlatinumDart"); 
            item.shootSpeed = 12.5f;
			item.consumable = false;
			
		}
		public override void GetVoid(Player player)
		{
			voidMana = 3;
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