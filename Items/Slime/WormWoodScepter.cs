using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
 
namespace SOTS.Items.Slime    
{
    public class WormWoodScepter : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood Scepter");
			Tooltip.SetDefault("Summons a Wormwood Turret to fire upon up to 4 enemies at a time");
		}
        public override void SetDefaults()
        {
            Item.damage = 21;  
            Item.mana = 20;     
            Item.width = 44;  
            Item.height = 44;    
            Item.useTime = 45; 
            Item.useAnimation = 45;   
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.noMelee = true;
            Item.knockBack = 0;  
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = 4;  
            Item.UseSound = SoundID.Item44; 
            Item.autoReuse = true;   
            Item.shoot = Mod.Find<ModProjectile>("PinkyTurret").Type;  
            Item.DamageType = DamageClass.Summon; 
            Item.sentry = true;
        } 
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CorrosiveGel>(), 32).AddIngredient(null, "Wormwood", 28).AddTile(TileID.Anvils).Register();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 cursorPos = Main.MouseWorld;
			position = cursorPos;
			return player.altFunctionUse != 2;
		}
    }
}