using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using System.Linq;
 

namespace SOTS.Items.BiomeItems
{
	[AutoloadEquip(EquipType.Body)]
	
	public class DevilChestplate : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 24;

			item.value = 125000;
			item.rare = 6;
			item.defense = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Devil Chestplate");
			Tooltip.SetDefault("5% increased melee speed\n5% increased melee damage\n34% decrease to all other damage types");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("DevilHelmet") && legs.type == mod.ItemType("DevilLeggings");
        }

		public override void UpdateEquip(Player player)
		{
		
			player.meleeSpeed += 0.05f;
			player.meleeDamage += 0.05f;
			player.magicDamage -= 0.34f;
			player.rangedDamage -= 0.34f;
			player.thrownDamage -= 0.34f;
			player.minionDamage -= 0.34f;
			
		}


	}
}