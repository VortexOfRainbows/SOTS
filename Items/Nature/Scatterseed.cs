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
            Item.damage = 10; 
            Item.magic = true; 
            Item.width = 30;   
            Item.height = 36;   
            Item.useTime = 39;   
            Item.useAnimation = 39;
            Item.useStyle = ItemUseStyleID.HoldingOut;    
            Item.noMelee = true;  
            Item.knockBack = 2.25f;
			Item.value = Item.sellPrice(0, 1, 25, 0);
            Item.rare = ItemRarityID.Orange;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<FlowerSeed>(); 
            Item.shootSpeed = 15f;
			Item.mana = 10;
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
