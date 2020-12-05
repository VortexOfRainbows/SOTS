using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class TerminatorAcorns : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Terminator Acorns");
			Tooltip.SetDefault("Summons a Mechanically Modified Squirrel to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}
		public override void SetDefaults() 
		{
			item.damage = 20;
			item.knockBack = 2f;
			item.mana = 12;
			item.width = 26;
			item.height = 32;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 1;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = mod.BuffType("TerminatorSquirrelBuff");
			item.shoot = mod.ProjectileType("TerminatorSquirrel");
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
			recipe.AddIngredient(null, "DissolvingNature", 1);
			recipe.AddIngredient(null, "FragmentOfInferno", 3);
			recipe.AddIngredient(null, "AcornBag", 1);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 10);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}