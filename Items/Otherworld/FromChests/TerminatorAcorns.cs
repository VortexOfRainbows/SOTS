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
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}
		public override void SetDefaults() 
		{
			Item.damage = 20;
			Item.knockBack = 2f;
			Item.mana = 12;
			Item.width = 26;
			Item.height = 32;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.summon = true;
			Item.buffType = ModContent.BuffType<TerminatorSquirrelBuff>();
			Item.shoot = ModContent.ProjectileType<TerminatorSquirrel>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
			player.AddBuff(Item.buffType, 2);
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