using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;


namespace SOTS.Items.ChestItems
{	[AutoloadEquip(EquipType.Shield)]
	public class ShieldofDesecar : ModItem
	{
		float shield = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shield of Desecar");
			Tooltip.SetDefault("'Less is More'\nGrants 1 defense for every 5 empty inventory slots");
		}
		public override void SetDefaults()
		{
            
            item.width = 34;     
            item.height = 32;     
            item.value = 50000;
            item.rare = 4;
			item.accessory = true;
			
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			item.defense = 0;
			shield = 0;
			for(int i = 0; i < 50; i++)
			{
			Item inventoryItem = player.inventory[i];
				if(inventoryItem.type == 0)
				{
					shield += 0.2f;
				}
			
			}
			item.defense += (int)shield;
		}
	}
}
