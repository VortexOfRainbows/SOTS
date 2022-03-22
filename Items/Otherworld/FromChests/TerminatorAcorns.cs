using System;
using Microsoft.Xna.Framework;
using SOTS.Buffs.MinionBuffs;
using SOTS.Items.Fragments;
using SOTS.Items.Nature;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Projectiles.Minions;
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
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<TerminatorSquirrelBuff>();
			item.shoot = ModContent.ProjectileType<TerminatorSquirrel>();
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
			recipe.AddIngredient(ModContent.ItemType<DissolvingNature>(), 1);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 3);
			recipe.AddIngredient(ModContent.ItemType<AcornBag>(), 1);
			recipe.AddIngredient(ModContent.ItemType<OtherworldlyAlloy>(), 10);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}