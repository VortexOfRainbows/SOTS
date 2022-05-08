using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Crushers;

namespace SOTS.Items.Crushers
{
	public class IrradiatedChainReactor : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Irradiated Chain-Reactor");
			Tooltip.SetDefault("Charge to increase damage up to 500%\nLaunches out spores that deal 50% damage\nTakes 1.66 seconds to reach max charge");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 54;
            Item.height = 54;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(0, 5, 40, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IrradiatedCrusher>(); 
            Item.shootSpeed = 12f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 16);
			recipe.AddIngredient(ItemID.SoulofNight, 6);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 4;
		}
	}
}
