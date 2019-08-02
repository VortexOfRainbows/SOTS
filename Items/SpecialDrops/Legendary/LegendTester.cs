using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops.Legendary
{
	public class LegendTester : ModItem
	{	int epic = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Legend Tester");
		}
		public override void SetDefaults()
		{
            item.magic = true;   //its a gun so set this to true
            item.width = 22;     //gun image width
            item.height = 24;   //gun image  height
            item.useTime = 15;  //how fast 
            item.useAnimation = 15;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 30000;
            item.rare = 6;
            item.UseSound = SoundID.Item8;
            item.autoReuse = false;
            item.shoot =  mod.ProjectileType("PikeProj"); 
            item.shootSpeed = 24;

		}
		public override void UpdateInventory(Player player)
		{
			SOTSWorld.legendLevel = epic;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			  epic++;
			  
			  if(epic > 25)
				  epic = 0;
			  
				int chargeInt = (int)epic;
				string text = chargeInt.ToString();
				Main.NewText("Epic of " + text, 195, 145, 225);
			  
					return false;
					
          
		}
	}
}
