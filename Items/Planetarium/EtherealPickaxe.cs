using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class EtherealPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Pickaxe");
			Tooltip.SetDefault("Consumes 200 mana per swing\nCan mine the planetarium orb");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(990);
			item.damage = 42;
            item.magic = true;   //its a gun so set this to true
            item.width = 38;     //gun image width
            item.height = 38;   //gun image  height
            item.useStyle = 1;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 120000;
            item.rare = 9;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
			item.expert = true;

		}
		public override bool CanUseItem(Player player)
        {
			if(player.statMana > 199)
			{
				player.statMana -= 200;
				return true;
			}
			return false;
		}
					
          }  
}
