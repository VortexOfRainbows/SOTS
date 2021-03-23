using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class NatureSpiritStaff : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Nature Spirit Staff");
			Tooltip.SetDefault("Summons an Nature Spirit to fight for you\nAttacks the closest enemy visible to you\nHeals health upon dealing damage");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}
		public override void SafeSetDefaults() 
		{
			item.damage = 9;
			item.knockBack = 4f;
			item.width = 42;
			item.height = 50;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = mod.BuffType("NatureSpiritAid");
			item.shoot = mod.ProjectileType("NatureSpirit");
			item.mana = 18;
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
			recipe.AddRecipeGroup("SOTS:SilverBar", 12);
			recipe.AddIngredient(ItemID.JungleSpores, 12);
			recipe.AddIngredient(ItemID.Stinger, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}