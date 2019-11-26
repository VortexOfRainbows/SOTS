using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
 
namespace SOTS.Items.Celestial
{
    public class StrangeFruit : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strange Fruit");
			Tooltip.SetDefault("'Must be a commodity somewhere'");
		}
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Carrot);
            item.shoot = mod.ProjectileType("NormalPetTurtle");
            item.buffType = mod.BuffType("EpicGun");
            item.value = Item.sellPrice(0, 7, 50, 0);
            item.rare = 8;
			//item.width = 48;
			//item.height = 38;
        }
        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return player.ownedProjectileCounts[mod.ProjectileType("NormalPetTurtle")] <= 0;
		}
    }
}