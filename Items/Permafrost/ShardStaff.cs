using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Permafrost
{  
    public class ShardStaff : ModItem
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shard Staff");
			Tooltip.SetDefault("Summons shards around the player");
			this.SetResearchCost(1);
		}
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Magic;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 20;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
			Item.shootSpeed = 9;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item43;
            Item.mana = 12;
			Item.shoot = ModContent.ProjectileType<IceShard>();
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<FrigidBar>(12).AddTile(TileID.Anvils).Register();
		}
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if(modPlayer.shardSpellExtra != 0)
			{
				Item.useTime = 12;
			}
			else
			{
				Item.useTime = 20;
			}
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 toPos = Main.MouseWorld;
			for(int i = 0; i < 1; i++)
			{
				Vector2 newPos = position + new Vector2(Main.rand.Next(-48, 49), Main.rand.Next(-48, 19));
				float speed = velocity.Length();
				Vector2 speed2 = new Vector2(speed, 0).RotatedBy(Math.Atan2(toPos.Y - newPos.Y, toPos.X - newPos.X));
				if(Item.crit + player.GetCritChance(DamageClass.Magic) + 4 >= Main.rand.Next(100) + 1)
				{
					Projectile.NewProjectile(source, newPos.X, newPos.Y, speed2.X, speed2.Y, type, damage, knockback, player.whoAmI, 5, 2);
				}
				else
				{
					Projectile.NewProjectile(source, newPos.X, newPos.Y, speed2.X, speed2.Y, type, damage, knockback, player.whoAmI, Main.rand.Next(5), 2);
				}
			}
			return false;
		}
    }
}