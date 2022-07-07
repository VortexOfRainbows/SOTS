using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tide
{
	public class Riptide : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Riptide");
			Tooltip.SetDefault("Right click while in water or rain to launch yourself forward, doing 120% damage\nImmunity to fall damage while held");
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Melee;
			Item.width = 58;
			Item.height = 58;
			Item.useTime = 29;
			Item.useAnimation = 29;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 7f;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<Projectiles.Tide.Riptide>();
			Item.shootSpeed = 3.0f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
        public override void HoldItem(Player player)
		{ 
			player.noFallDmg = true;
			base.HoldItem(player);
        }
        public override bool AltFunctionUse(Player player)
		{
			return true;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			float mult = 1f;
			if(player.altFunctionUse == 2)
			{
				mult = 1.2f;
				velocity *= 0.55f;
			}
			Projectile.NewProjectile(source, position, velocity, type, (int)(damage * mult), (int)(knockback * mult), player.whoAmI, player.altFunctionUse == 2 ? 1 : 0);
			return false;
		}
        public override float UseAnimationMultiplier(Player player)
        {
            return UseTimeMultiplier(player);
        }
        public override float UseTimeMultiplier(Player player)
		{
			return 1f / (player.altFunctionUse == 2 ? 0.45f : 1);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Trident, 1).AddIngredient(ModContent.ItemType<Fragments.FragmentOfTide>(), 10).AddTile(TileID.Anvils).Register();
			Recipe.Create(ItemID.Trident).AddIngredient(ItemID.Spear, 1).AddIngredient(ItemID.GoldBar, 8).AddTile(TileID.Anvils).Register();
			Recipe.Create(ItemID.Trident).AddIngredient(ItemID.Spear, 1).AddIngredient(ItemID.PlatinumBar, 8).AddTile(TileID.Anvils).Register();
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}
}