using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class AcornBag : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Acorn Bag");
			Tooltip.SetDefault("Summons a squirrel to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults() 
		{
			item.damage = 7;
			item.knockBack = 2f;
			item.mana = 9;
			item.width = 24;
			item.height = 32;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item44;

			item.noMelee = true;
			item.summon = true;
			item.buffType = mod.BuffType("SquirrelBuff");
			item.shoot = mod.ProjectileType("SquirrelMinion");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
			player.AddBuff(item.buffType, 2);

			position = Main.MouseWorld;
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 20);
			recipe.AddIngredient(null, "FragmentOfNature", 4);
			recipe.AddIngredient(ItemID.Acorn, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}