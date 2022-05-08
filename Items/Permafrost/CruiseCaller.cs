using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Minions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class CruiseCaller : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Cruise Caller");
			Tooltip.SetDefault("Summons a fleet of Penguin Copters to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults() 
		{
			Item.damage = 47;
			Item.knockBack = 4f;
			Item.mana = 20;
			Item.width = 34;
			Item.height = 42;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.HoldingUp;
			Item.value = Item.sellPrice(0, 8, 75, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.summon = true;
			Item.buffType = ModContent.BuffType<AerialAssistance>();
			Item.shoot = ModContent.ProjectileType<PenguinCopter>();
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
			recipe.AddIngredient(ModContent.ItemType<DissolvingAurora>(), 1);
			recipe.AddIngredient(ModContent.ItemType<AbsoluteBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<HelicopterParts>(), 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}