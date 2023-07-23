using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Pyramid
{  
    public class CursedBlade : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
        }
        public override void SafeSetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Summon;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 2, 25, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.CursedBlade>();
			Item.buffType = ModContent.BuffType<Buffs.MinionBuffs.CursedBlade>();
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CursedMatter>(), 8).AddIngredient<RoyalRubyShard>(10).AddTile(TileID.Anvils).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			player.AddBuff(Item.buffType, 2);
            player.SpawnMinionOnCursor(source, player.whoAmI, type, damage, knockback);
			return false;
		}
	}
}