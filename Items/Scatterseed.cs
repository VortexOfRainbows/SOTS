using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Items.Fragments;

namespace SOTS.Items
{
	public class Scatterseed : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scatterseed");
			Tooltip.SetDefault("Rapidly launches seeds that latch onto enemies\nWhen the seeds bloom, they do 125% damage");
		}
		public override void SetDefaults()
		{
            item.damage = 10; 
            item.magic = true; 
            item.width = 30;   
            item.height = 36;   
            item.useTime = 14;   
            item.useAnimation = 14;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true;  
            item.knockBack = 2.25f;
			item.value = Item.sellPrice(0, 1, 25, 0);
            item.rare = ItemRarityID.Orange;
			item.autoReuse = true;
            item.UseSound = SoundID.Item8;
            item.shoot = mod.ProjectileType("FlowerSeed"); 
            item.shootSpeed = 11.5f;
			item.mana = 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "NatureSpell", 1);
			recipe.AddIngredient(ItemID.CrimtaneBar, 8);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfNature>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "NatureSpell", 1);
			recipe.AddIngredient(ItemID.DemoniteBar, 8);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfNature>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectile(position.X, position.Y, speedX * (Main.rand.Next(90,111) * 0.007f), speedY * (Main.rand.Next(90,111) * 0.007f), type, damage, knockBack, player.whoAmI, 0, 1.25f);
			return false; 
		}
	}
}
