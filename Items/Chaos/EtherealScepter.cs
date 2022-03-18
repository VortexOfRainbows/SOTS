using System;
using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Projectiles.Minions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class EtherealScepter : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Ethereal Scepter");
			Tooltip.SetDefault("Summons an Ethereal Flame to fight for you\nEthereal Flames attack enemies by rapidly dashing through them");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults() 
		{
			item.damage = 41;
			item.knockBack = 4.5f;
			item.width = 66;
			item.height = 74;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 12, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<Ethereal>();
			item.shoot = ModContent.ProjectileType<EtherealFlame>();
			item.mana = 16;
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
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}