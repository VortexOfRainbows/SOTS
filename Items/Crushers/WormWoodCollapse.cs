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
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 34;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 44;
            Item.height = 44;  
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
		public override bool CanShoot(Player player)
		{
            return true; // player.ownedProjectileCounts[Item.shoot] <= 0;
        }
		public override int GetVoid(Player player)
		{
			return 4;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CorrosiveGel>(), 20).AddIngredient(ModContent.ItemType<Wormwood>(), 32).AddTile(TileID.Anvils).Register();
		}
	}
}
