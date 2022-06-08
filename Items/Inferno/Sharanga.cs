using Microsoft.Xna.Framework;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Inferno;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Inferno
{
	public class Sharanga : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharanga");
			Tooltip.SetDefault("Fires supercharged hellfire arrows");
		}
		public override void SetDefaults()
		{
            Item.damage = 25; 
            Item.DamageType = DamageClass.Ranged;  
            Item.width = 36;   
            Item.height = 54; 
            Item.useTime = 25; 
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 2, 25, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SharangaBolt>(); 
            Item.shootSpeed = 21.5f;
			Item.useAmmo = ItemID.WoodenArrow;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<SharangaBolt>();
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HellwingBow, 1).AddIngredient(ItemID.MoltenFury, 1).AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 4).AddTile(TileID.Anvils).Register();
		}
	}
}
