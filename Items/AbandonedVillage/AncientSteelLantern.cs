using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SOTS.Items.AbandonedVillage    
{
    public class AncientSteelLantern : ModItem
    {
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.damage = 8;
            Item.mana = 10;   
            Item.width = 30;    
            Item.height = 52;    
            Item.useTime = 45;  
            Item.useAnimation = 45;   
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.noMelee = Item.autoReuse = Item.sentry = true; 
            Item.knockBack = 2.5f;
            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item44; 
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.AncientSteelLantern>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<AncientSteelBar>(), 12).AddIngredient<CharredWood>(12).AddIngredient(ItemID.Campfire).AddTile(TileID.Anvils).Register();
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
		    player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
    }
}