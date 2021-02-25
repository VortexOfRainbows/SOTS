using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class PinkJellyfishStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pink Jellyfish Staff");
			Tooltip.SetDefault("Fires pink lightning which chains off enemies for 60% damage");
		}
		public override void SetDefaults()
		{
            item.damage = 14;
            item.magic = true; 
            item.width = 32;    
            item.height = 34; 
            item.useTime = 20; 
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 3;
			item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item93;
            item.noMelee = true; 
            item.autoReuse = false;
            item.shootSpeed = 5.5f; 
			item.shoot = mod.ProjectileType("PinkLightning");
			Item.staff[item.type] = true; 
			item.mana = 13;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Amethyst, 15);
			recipe.AddIngredient(null, "FragmentOfTide", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX * .66f, speedY * .66f).RotatedByRandom(MathHelper.ToRadians(25));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, 2.5f);
			}
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 2.5f);
            return false;
		}
	}
}
