using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;
using SOTS.Items.Fragments;
using Terraria.DataStructures;

namespace SOTS.Items.Evil
{
	public class TomeOfTheReaper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tome of the Reaper");
			Tooltip.SetDefault("Cast three scythes that move towards your cursor");
		}
		public override void SetDefaults()
		{
            Item.damage = 48; 
            Item.DamageType = DamageClass.Magic; 
            Item.width = 42;   
            Item.height = 44;   
            Item.useTime = 18;   
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;  
            Item.knockBack = 4.5f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ReaperScythe>(); 
            Item.shootSpeed = 9.5f;
			Item.mana = 14;
			Item.reuseDelay = 16;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.DemonScythe, 1).AddIngredient(ItemID.SoulofFright, 10).AddIngredient(ModContent.ItemType<DissolvingUmbra>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for(int i = -1; i<= 1; i++)
			{
				Vector2 perturbedSpeed = new Vector2(-velocity.X, -velocity.Y).RotatedBy(MathHelper.ToRadians(15) * i) * (i != 0 ? 0.9f : 1);
				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false; 
		}
	}
}
