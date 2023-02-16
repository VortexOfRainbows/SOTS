using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Items.Fragments;
using SOTS.Projectiles;
using Terraria.DataStructures;

namespace SOTS.Items.Nature
{
	public class Scatterseed : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 10; 
            Item.DamageType = DamageClass.Magic; 
            Item.width = 30;   
            Item.height = 36;   
            Item.useTime = 39;   
            Item.useAnimation = 39;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;  
            Item.knockBack = 2.25f;
			Item.value = Item.sellPrice(0, 1, 25, 0);
            Item.rare = ItemRarityID.Orange;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<FlowerSeed>(); 
            Item.shootSpeed = 15f;
			Item.mana = 10;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<NatureSpell>(), 1).AddIngredient(ItemID.CrimtaneBar, 8).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 6).AddTile(TileID.Anvils).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<NatureSpell>(), 1).AddIngredient(ItemID.DemoniteBar, 8).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 6).AddTile(TileID.Anvils).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int amt = 3;
			for (int i = 0; i < amt; i++)
			{
				Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(3.5f * i, 3.5f * i))) + Main.rand.NextVector2Circular(1, 1), type, damage, knockback, player.whoAmI, 0, 1.25f);
			}
			return false; 
		}
	}
}
