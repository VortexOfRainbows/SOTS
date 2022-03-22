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
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults() 
		{
			item.damage = 47;
			item.knockBack = 4f;
			item.mana = 20;
			item.width = 34;
			item.height = 42;
			item.useTime = 32;
			item.useAnimation = 32;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.value = Item.sellPrice(0, 8, 75, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<AerialAssistance>();
			item.shoot = ModContent.ProjectileType<PenguinCopter>();
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
			recipe.AddIngredient(ModContent.ItemType<DissolvingAurora>(), 1);
			recipe.AddIngredient(ModContent.ItemType<AbsoluteBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<HelicopterParts>(), 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}