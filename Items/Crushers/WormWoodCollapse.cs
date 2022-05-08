using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;
using SOTS.Items.Slime;

namespace SOTS.Items.Crushers
{
	public class WormWoodCollapse : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood Collapse");
			Tooltip.SetDefault("Charge to increase damage up to 500%\nTakes 3 seconds to reach max charge");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 34;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 42;
            Item.height = 42;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 7f;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<WormWoodCrusher>(); 
            Item.shootSpeed = 18f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CorrosiveGel>(), 20);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 32);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
