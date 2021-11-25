using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Items.Fragments;
using SOTS.Projectiles;

namespace SOTS.Items.Nature
{
	public class Scatterseed : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scatterseed");
			Tooltip.SetDefault("Scatters seeds that latch onto enemies\nWhen the seeds bloom, they do 125% damage");
		}
		public override void SetDefaults()
		{
            item.damage = 10; 
            item.magic = true; 
            item.width = 30;   
            item.height = 36;   
            item.useTime = 39;   
            item.useAnimation = 39;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true;  
            item.knockBack = 2.25f;
			item.value = Item.sellPrice(0, 1, 25, 0);
            item.rare = ItemRarityID.Orange;
			item.autoReuse = true;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<FlowerSeed>(); 
            item.shootSpeed = 15f;
			item.mana = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NatureSpell>(), 1);
			recipe.AddIngredient(ItemID.CrimtaneBar, 8);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfNature>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NatureSpell>(), 1);
			recipe.AddIngredient(ItemID.DemoniteBar, 8);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfNature>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int amt = 3;
			for (int i = 0; i < amt; i++)
			{
				Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(3.5f * i, 3.5f * i))) + Main.rand.NextVector2Circular(1, 1), type, damage, knockBack, player.whoAmI, 0, 1.25f);
			}
			return false; 
		}
	}
}
