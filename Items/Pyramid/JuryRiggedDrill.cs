using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SOTS.Items.Pyramid
{
	public class JuryRiggedDrill : ModItem
	{
		int counter = 0;
		int index = -1;
		bool inInventory = false;
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(200);
		}
		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Melee;
			Item.width = 42;
			Item.height = 22;
			Item.useTime = 6;
			Item.useAnimation = 30;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.pick = 110;
			Item.tileBoost++;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(0, 0, 1, 50);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item23;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pyramid.JuryRiggedDrill>();
			Item.shootSpeed = 20f;
			Item.consumable = true;
			Item.maxStack = 9999;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            index = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
			return false;
		}
        public override void UpdateInventory(Player player)
		{
			SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(player);
			if (sPlayer.oldHeldProj >= 0 && Main.projectile[sPlayer.oldHeldProj].type == ModContent.ProjectileType<Projectiles.Pyramid.JuryRiggedDrill>() && player.whoAmI == Main.myPlayer)
			{
				//Main.NewText("facts");
				counter++;
				if (counter > 60)
				{
					counter = 0;
					Main.projectile[index].Kill();
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Main.projectile[index].Center);
					for (int i = 0; i < 15; i++)
					{
						int num1 = Dust.NewDust(new Vector2(Main.projectile[index].position.X, Main.projectile[index].position.Y), 20, 34, DustID.Sand);
						Main.dust[num1].noGravity = false;
						Main.dust[num1].velocity *= 1.5f;
						Main.dust[num1].scale *= 1.3f;
					}
					for (int i = 0; i < 2; i++)
					{
						int goreIndex = Gore.NewGore(player.GetSource_ItemUse(Item), new Vector2(Main.projectile[index].position.X, Main.projectile[index].position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
						Main.gore[goreIndex].scale = 0.65f;
					}
					player.ConsumeItem(Item.type);
				}
			}
		}
        public override bool? UseItem(Player player)
        {
			counter += 4;
			return base.UseItem(player);
        }
        public override bool CanUseItem(Player player)
		{
			inInventory = true;
			if (player.selectedItem == 58)
			{
				inInventory = false;
				//Main.NewText("not selected");
			}
			return inInventory;
        }
        public override bool ConsumeItem(Player player)
		{
			if (index != -1 && !Main.projectile[index].active)
			{
				index = -1;
				return true;
			}
			return false;
		}
	}
}