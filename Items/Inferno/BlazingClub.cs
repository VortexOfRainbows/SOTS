using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;
using Terraria.DataStructures;
using SOTS.Items.ChestItems;
using SOTS.Items.Nature;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Inferno;

namespace SOTS.Items.Inferno
{
	public class BlazingClub : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 19;
			Item.DamageType = DamageClass.Melee;
			Item.width = 42;
			Item.height = 46;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2.2f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<BlazingMine>(); 
            Item.shootSpeed = 6.5f;
		}
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for(int i = 0; i <= player.spikedBoots; i++)
			{
				float speedMult = 0.5f + i * 0.5f + (player.altFunctionUse == 2 ? 0.75f : 0);
				int totalDamage = damage - i * 2; //slight damage fall of for farther mines (bigger quantities)
				if (totalDamage < damage / 2)
					totalDamage = damage / 2;
				Projectile.NewProjectile(source, position, velocity * speedMult, type, totalDamage, knockback, player.whoAmI, player.altFunctionUse == 2 ? -1 : 0);
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<SpikedClub>(1).AddIngredient<SporeClub>(1).AddIngredient<DissolvingNature>(1).AddIngredient<FragmentOfInferno>(6).AddRecipeGroup("SOTS:EvilMaterial", 10).AddTile(TileID.Anvils).Register();
		}
	}
}