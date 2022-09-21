using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using SOTS.Projectiles.Blades;
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
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(2, 15));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 62;
            Item.height = 66;  
            Item.useTime = 20; 
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;		
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			i++;
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, i % 2 * 2 -1, Main.rand.NextFloat(0.875f, 1.125f));
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Arkhalis, 1).AddIngredient<HardlightAlloy>(30).AddIngredient<PrecariousCluster>(1).AddIngredient<TaintedKeystone>(1).AddTile<HardlightFabricatorTile>().Register();
		}
		/*public override int GetVoid(Player player)
		{
			return  3;
		}*/
	}
}
