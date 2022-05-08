using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using SOTS.Projectiles.Otherworld;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Fragments;
using SOTS.Items.Pyramid;

namespace SOTS.Items
{
	public class DigitalDaito : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Digital Daito");
			Tooltip.SetDefault("");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(2, 15));
		}
		public override void SetDefaults()
		{
            Item.damage = 50;
            Item.melee = true;  
            Item.width = 62;
            Item.height = 64;  
            Item.useTime = 20; 
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldingOut;		
            Item.knockBack = 8f;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DigitalSlash>(); 
            Item.shootSpeed = 18f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
		int i = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			i++;
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, i % 2 * 2 -1, Main.rand.NextFloat(0.875f, 1.125f));
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Arkhalis, 1);
			recipe.AddIngredient(ModContent.ItemType<HardlightAlloy>(), 30);
			recipe.AddIngredient(ModContent.ItemType<PrecariousCluster>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TaintedKeystone>(), 1);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		/*public override int GetVoid(Player player)
		{
			return  3;
		}*/
	}
}
