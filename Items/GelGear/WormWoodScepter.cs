using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
 
 
namespace SOTS.Items.GelGear      ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class WormWoodScepter : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worm Wood Scepter");
			
			Tooltip.SetDefault("Summons a worm wood crystal to fire upon your enemies");
		}
 
        public override void SetDefaults()
        {
            item.damage = 25;  //The damage stat for the Weapon.
            item.mana = 20;      //this defines how many mana this weapon use
            item.width = 44;    //The size of the width of the hitbox in pixels.
            item.height = 44;     //The size of the height of the hitbox in pixels.
            item.useTime = 45;   //How fast the Weapon is used.
            item.useAnimation = 45;    //How long the Weapon is used for.
            item.useStyle = 1;  //The way your Weapon will be used, 1 is the regular sword swing for exampleingame.
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;  //The knockback stat of your Weapon.
            item.value = 90000; // How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 10gold)
            item.rare = 4;   //The color the title of your Weapon when hovering over it ingame  
            item.UseSound = SoundID.Item44;   //The sound played when using your Weapon
            item.autoReuse = true;   //Weather your Weapon will be used again after use while holding down, if false you will need to click again after use to use it again.
            item.shoot = mod.ProjectileType("PinkyTurret");   //This defines what type of projectile this weapon will shot
            item.summon = true;    //This defines if it does Summon damage and if its effected by Summon increasing Armor/Accessories.
            item.sentry = true; //tells the game that this is a sentry
        } 
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WormWoodCore", 1);
			recipe.AddIngredient(null, "SlimeyFeather", 32);
			recipe.AddIngredient(null, "GelBar", 12);
			recipe.AddIngredient(ItemID.Wood, 8);
			recipe.AddIngredient(ItemID.PinkGel, 32);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 SPos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			position = SPos;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
				{
					proj.active = false;
				}
			}
			return player.altFunctionUse != 2;
		}

    }
}