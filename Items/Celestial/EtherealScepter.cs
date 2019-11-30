using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
	public class EtherealScepter : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Ethereal Scepter");
			Tooltip.SetDefault("Summons an Ethereal Flame to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults() 
		{
			item.damage = 52;
			item.knockBack = 4f;
			item.mana = 24;
			item.width = 40;
			item.height = 42;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.value = Item.sellPrice(0, 8, 75, 0);
			item.rare = 8;
			item.UseSound = SoundID.Item44;

			item.noMelee = true;
			item.summon = true;
			item.buffType = mod.BuffType("Ethereal");
			item.shoot = mod.ProjectileType("EtherealFlame");
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
			recipe.AddIngredient(null, "StarShard", 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}