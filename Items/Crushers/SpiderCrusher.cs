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
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 42;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 44;
            Item.height = 44;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 8f;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Crushers.SpiderCrusher>(); 
            Item.shootSpeed = 18f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
		public override bool CanShoot(Player player)
		{
            return true; // player.ownedProjectileCounts[Item.shoot] <= 0;
        }
		public override int GetVoid(Player player)
		{
			return 5;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.SpiderFang, 18).AddIngredient(ModContent.ItemType<DissolvingEarth>(), 1).AddTile(TileID.Anvils).Register();
		}
	}
}
