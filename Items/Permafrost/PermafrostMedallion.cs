using SOTS.Projectiles.Minions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class PermafrostMedallion : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 36;
			Item.DamageType = DamageClass.Summon;
            Item.width = 34;     
            Item.height = 38;   
            Item.value = Item.sellPrice(0, 5, 50, 0);
            Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}
		int[] Probes = { -1, -1, -1, -1, -1, -1, -1, -1 };
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<AbsoluteBar>(7).AddTile(TileID.MythrilAnvil).Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			int type = ModContent.ProjectileType<BlizzardProbe>();
			if(player.whoAmI == Main.myPlayer)
			{
				for (int i = 0; i < Probes.Length; i++)
				{
					if (Probes[i] == -1)
					{
						Probes[i] = Projectile.NewProjectile(player.GetSource_Misc("SOTS:Pets"), player.position.X, player.position.Y, 0, 0, type, SOTSPlayer.ApplyDamageClassModWithGeneric(player, Item.DamageType, Item.damage), 0, player.whoAmI, i, i * 15);
					}
					if (!Main.projectile[Probes[i]].active || Main.projectile[Probes[i]].type != type || Main.projectile[Probes[i]].ai[0] != i)
					{
						Probes[i] = Projectile.NewProjectile(player.GetSource_Misc("SOTS:Pets"), player.position.X, player.position.Y, 0, 0, type, SOTSPlayer.ApplyDamageClassModWithGeneric(player, Item.DamageType, Item.damage), 0, player.whoAmI, i, i * 15);
					}
					Main.projectile[Probes[i]].timeLeft = 6;
				}
			}
        }
        public override bool WeaponPrefix()
        {
            return false;
        }
    }
}