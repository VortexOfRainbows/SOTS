using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class JuryRiggedDrill : ModItem
	{
		int counter = 0;
		int index = -1;
		bool inInventory = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jury Rigged Drill");
			Tooltip.SetDefault("Can break the walls of the pyramid\n'Might only withstand a few hits'");
		}
		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Melee;
			Item.width = 42;
			Item.height = 22;
			Item.useTime = 5;
			Item.useAnimation = 25;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.pick = 110;
			Item.tileBoost++;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(0, 0, 1, 50);
			Item.rare = 5;
			Item.UseSound = SoundID.Item23;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("JuryRiggedDrill").Type;
			Item.shootSpeed = 20f;
			Item.consumable = true;
			Item.maxStack = 999;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            index = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			inInventory = true;
		}
		public override bool CanUseItem(Player player)
		{
			if(inInventory)
			return true;
		
			return false;
		}
		public override bool ConsumeItem(Player player) 
		{
			inInventory = false;
			if(counter > 8 && index != -1 && Main.projectile[index].active && Main.player[Main.projectile[index].owner] == player && Main.projectile[index].type == Item.shoot)
			{
				counter = 0;
				Main.projectile[index].Kill();
				SoundEngine.PlaySound(SoundID.Item14, (int)(Main.projectile[index].Center.X), (int)(Main.projectile[index].Center.Y));
				for(int i = 0; i < 15; i ++)
				{
					int num1 = Dust.NewDust(new Vector2(Main.projectile[index].position.X, Main.projectile[index].position.Y), 20, 34, 32);

					
					Main.dust[num1].noGravity = false;
					Main.dust[num1].velocity *= 1.5f;
					Main.dust[num1].scale *= 1.3f;
				}
				for(int i = 0; i < 2; i++)
				{
					int goreIndex = Gore.NewGore(new Vector2(Main.projectile[index].position.X, Main.projectile[index].position.Y), default(Vector2), Main.rand.Next(61,64), 1f);	
					Main.gore[goreIndex].scale = 0.65f;
				}
				index = -1;
				return true;
			}
			counter++;
			return false;
		}
	}
}