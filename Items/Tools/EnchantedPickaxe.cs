using Microsoft.Xna.Framework;
using SOTS.Items.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class EnchantedPickaxe : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Pickaxe");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 14;
            item.melee = true;  
            item.width = 30;   
            item.height = 30;   
            item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
            item.useTime = 18;
            item.useAnimation = 18;
			item.pick = 100;
			item.knockBack = 2.5f;
			item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item1;
			item.tileBoost = 3;
			item.autoReuse = true;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(5) == 0)
			{
				int num = Main.rand.Next(3);
				int type;
				if (num == 0)
				{
					type = 15;
				}
				else
				{
					if (num == 1)
					{
						type = 57;
					}
					else
					{
						type = 58;
					}
				}
				int num1 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, type, player.direction * 2, 0f, 150, default(Color), 1.3f);
				Main.dust[num1].velocity *= 0.2f;
			}
			base.MeleeEffects(player, hitbox);
		}
	}
}
