using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;
using SOTS.Items.Fragments;

namespace SOTS.Items.Crushers
{
	public class Hellbreaker : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 48;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 46;
            Item.height = 46;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 8f;
            Item.value = Item.sellPrice(0, 1, 55, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HellbreakerCrusher>(); 
            Item.shootSpeed = 20f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
			Item.staff[Item.type] = true;
		}
		public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 6;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HellstoneBar, 12).AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 2).AddTile(TileID.Anvils).Register();
		}
	}
}