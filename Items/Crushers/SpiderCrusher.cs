using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Fragments;

namespace SOTS.Items.Crushers
{
	public class SpiderCrusher : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fly Catcher");
			Tooltip.SetDefault("Charge to increase damage up to 180%\nReleases spider webs that slow hit enemies\nReleases more when charged\nTakes 2.5 seconds to reach max charge\n'That's really clever actually'");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 42;
            item.melee = true;  
            item.width = 44;
            item.height = 44;  
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.knockBack = 8f;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Projectiles.Crushers.SpiderCrusher>(); 
            item.shootSpeed = 18f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return player.ownedProjectileCounts[type] <= 0; 
		}
		public override int GetVoid(Player player)
		{
			return 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SpiderFang, 18);
			recipe.AddIngredient(ModContent.ItemType<DissolvingEarth>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
