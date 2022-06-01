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
			CreateRecipe(1).AddIngredient(ItemID.ChlorophyteBar, 16).AddIngredient(ItemID.SoulofNight, 6).AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 6).AddTile(TileID.MythrilAnvil).Register();
		}
		public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 4;
		}
	}
}
