using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class OtherworldlySpiritStaff : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Otherworldly Spirit Staff");
			Tooltip.SetDefault("Summons an Otherworldly Spirit to fight for you\nAccumulates up to 4 thunder charges that are launched at enemies in quick succession");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}
		public override void SafeSetDefaults() 
		{
			item.damage = 30;
			item.knockBack = 4f;
			item.width = 40;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = mod.BuffType("OtherworldlySpiritAid");
			item.shoot = mod.ProjectileType("OtherworldlySpirit");
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
			recipe.AddIngredient(null, "DissolvingAether", 1);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 30);
			recipe.AddIngredient(ModContent.ItemType<HardlightAlloy>(), 10);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}