using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Fragments
{
	public class DissolvingNature : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolving Nature");
			Tooltip.SetDefault("'Your limbs ache'\nReduces damage dealt by 10% while in the inventory");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 6));
		}
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 42;
            item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 3;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true; 
		}
		public override void UpdateInventory(Player player)
		{
			for(int i = 0; i < item.stack; i++)
			{
				if(player.allDamage > 0f)
				{
					player.allDamage -= 0.1f;
				}
				else
				{
					player.allDamage = 0;
				}
			}
			
			Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, mod.DustType("BigNatureDust"));
		}
		public override void PostUpdate()
		{
			Dust.NewDust(new Vector2(item.position.X, item.position.Y), item.width, item.height, mod.DustType("BigNatureDust"));
		}
	}
}