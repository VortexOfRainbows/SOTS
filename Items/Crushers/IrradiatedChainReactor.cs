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
            item.damage = 40;
            item.melee = true;  
            item.width = 54;
            item.height = 54;  
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 6f;
            item.value = Item.sellPrice(0, 5, 40, 0);
            item.rare = ItemRarityID.Lime;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<IrradiatedCrusher>(); 
            item.shootSpeed = 12f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
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
		public override void GetVoid(Player player)
		{
			voidMana = 4;
		}
	}
}
