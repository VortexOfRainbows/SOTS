using Microsoft.Xna.Framework;
using SOTS;
using SOTS.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Projectiles.Otherworld;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using SOTS.Items.Fragments;
using SOTS.Void;

namespace SOTS.Items.Invidia
{
	public class VesperaFishingRod : VoidItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.CanFishInLava[Item.type] = false;
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodFishingPole);
			//Item.width = 46;
			//Item.height = 36;
			//Sets the poles fishing power
			Item.fishingPole = 10;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			//Sets the speed in which the bobbers are launched, Wooden Fishing Pole is 9f and Golden Fishing Rod is 17f
			Item.shootSpeed = 11f;
			//The Bobber projectile
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.EvostoneBobber>();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for (int i = -2; i <= 2; i++)
			{
				Vector2 rotation = velocity.RotatedBy(MathHelper.ToRadians(6f * i));
				Projectile.NewProjectile(source, position.X, position.Y, rotation.X, rotation.Y, type, 0, 0f, player.whoAmI);
			}
			return false;
		}
        public override bool BeforeDrainMana(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 1;
        }
        public override int GetVoid(Player player)
        {
            return 15;
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<Evostone>(20).AddIngredient(ItemID.StoneBlock, 40).AddIngredient<FragmentOfEarth>(1).AddTile(TileID.Furnaces).Register();
		}
	}
}