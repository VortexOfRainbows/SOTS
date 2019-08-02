using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
namespace SOTS.Items.SpecialDrops
{
	public class DetachedEye : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Detached Eye");
		}
		public override void SafeSetDefaults()
		{
			item.magic = true;
			item.damage = 13;
			item.width = 36;
			item.height = 22;
			item.useTime = 27;
			item.useAnimation = 27;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 27500;
			item.rare = 3;
			item.UseSound = SoundID.Item12;
			item.autoReuse = false;            
			item.shoot = 95; 
            item.shootSpeed = 16;
		}
		public override void GetVoid(Player player)
		{
				voidMana = 6;
		}
	}
}