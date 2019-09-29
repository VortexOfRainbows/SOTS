using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets
{	[AutoloadEquip(EquipType.HandsOn)]
	public class LibrasBackupBow : ModItem
	{	int Probe = -1;
	int timer = -1;
	int mr = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Libra's Backup Bow");
			Tooltip.SetDefault("Secret Item\nRange Locked");
		}
		public override void SetDefaults()
		{
      
            item.width = 56;     
            item.height = 50;   
            item.value = 0;
            item.rare = -12;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.thrownDamage -= player.thrownDamage;
			player.rangedDamage += 0.25f;
			player.meleeDamage -= player.meleeDamage;
			player.magicDamage -= player.magicDamage;
			player.minionDamage -= player.minionDamage;
			player.AddBuff(mod.BuffType("BackupBow"), 300);
		}
	}
		
}

