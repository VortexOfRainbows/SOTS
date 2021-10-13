using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;
using SOTS.Items.Fragments;

namespace SOTS.Items.Crushers
{
	public class CrabClaw : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crab Claw");
			Tooltip.SetDefault("Charge to increase damage up to 800%\nTakes 4 seconds to reach max charge");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 18;
            item.melee = true;  
            item.width = 32;
            item.height = 26;  
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 10f;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<CrabCrusher>(); 
            item.shootSpeed = 18f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Topaz, 15);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfTide>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 3;
		}
	}
}