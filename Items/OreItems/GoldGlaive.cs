using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.OreItems
{
	public class GoldGlaive : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Glaive");
			Tooltip.SetDefault("Fires 3 bolts at your cursor, each dealing 75% damage");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 28;
			item.melee = true;
			item.width = 42;
			item.height = 40;
			item.useTime = 31;
			item.useAnimation = 31;
			item.useStyle = 5;
			item.knockBack = 5;
            item.value = Item.sellPrice(0, 0, 35, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;       
			item.shoot = mod.ProjectileType("GoldSpear"); 
            item.shootSpeed = 3.8f;
			item.noUseGraphic = true;
			item.noMelee = true;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GoldBar, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
              int numberProjectiles = 3;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(160 + (i * 20)));
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("GoldBolt"), (int)(damage * 0.75f) + 1, knockBack, player.whoAmI);
              }
              return true; 
		}
	}
}
	
