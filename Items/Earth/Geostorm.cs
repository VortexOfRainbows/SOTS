using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Earth
{
	public class Geostorm : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geostorm");
			Tooltip.SetDefault("Bombards your cursor with crystals");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 14;
			item.magic = true;
			item.width = 26;
			item.height = 38;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = 10;
            item.shootSpeed = 5.5f; //arbitrary
			item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			return true; 
		}
		public override void GetVoid(Player player)
		{
			voidMana = 15;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}