using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Projectiles.Minions;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpiritStaves
{
	public class TidalSpiritStaff : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Summons an Tidal Spirit to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}
		public override void SafeSetDefaults() 
		{
			item.damage = 33;
			item.knockBack = 4f;
			item.width = 40;
			item.height = 42;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<TidalSpiritAid>();
			item.shoot = ModContent.ProjectileType<TidalSpirit>();
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
			recipe.AddIngredient(ModContent.ItemType<Fragments.DissolvingDeluge>(), 1);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
			recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddTile(ItemID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}