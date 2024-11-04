using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SOTS.Projectiles.Minions;
using Terraria.DataStructures;

namespace SOTS.Items.Celestial    
{
    public class VoidspaceAuraStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.damage = 54;  
            Item.mana = 12;   
            Item.width = 44;    
            Item.height = 40;    
            Item.useTime = 45;  
            Item.useAnimation = 45;   
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.noMelee = true; 
            Item.knockBack = 3f; 
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item44; 
            Item.autoReuse = true;  
            Item.shoot = ModContent.ProjectileType<VoidspaceCell>();
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true; 
        } 
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<SanguiteBar>(15).AddIngredient<Items.AbandonedVillage.AncientSteelLantern>(1).AddTile(TileID.MythrilAnvil).Register();
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