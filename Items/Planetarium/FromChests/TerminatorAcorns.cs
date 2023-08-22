using System;
using Microsoft.Xna.Framework;
using SOTS.Buffs.MinionBuffs;
using SOTS.Items.Fragments;
using SOTS.Items.Nature;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Projectiles.Minions;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium.FromChests
{
	public class TerminatorAcorns : ModItem
	{
		public override void SetStaticDefaults() 
		{
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			this.SetResearchCost(1);
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
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<TerminatorSquirrelBuff>();
			Item.shoot = ModContent.ProjectileType<TerminatorSquirrel>();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			player.AddBuff(Item.buffType, 2);
			player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DissolvingNature>(), 1).AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 3).AddIngredient(ModContent.ItemType<AcornBag>(), 1).AddIngredient(ModContent.ItemType<OtherworldlyAlloy>(), 10).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}