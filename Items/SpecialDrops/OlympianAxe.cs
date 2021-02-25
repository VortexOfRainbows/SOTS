using Microsoft.Xna.Framework;
using SOTS.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class OlympianAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Olympian Waraxe");
            Tooltip.SetDefault("Enter a 3 second Frenzy after killing an enemy, massively increasing melee attack speed\nRight click to toss the axe for 60% damage\nCan toss two axes when under Frenzy");
        }
		public override void SetDefaults()
		{
            item.damage = 21; 
            item.melee = true;  
            item.width = 38;   
            item.height = 38;
            item.useTime = 24; 
            item.useAnimation = 24;
            item.useStyle = 1;    
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
            item.axe = 16;
            item.channel = true;
            item.shoot = ModContent.ProjectileType<Projectiles.OlympianAxe>();
            item.shootSpeed = 8.5f;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (target.life <= 0)
            {
                Main.PlaySound(SoundID.MaxMana, player.Center);
                player.AddBuff(ModContent.BuffType<Frenzy>(), 190);
            }
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            damage = (int)(damage * 0.6f);
            speedX *= (1 + SOTSPlayer.ModPlayer(player).attackSpeedMod);
            speedY *= (1 + SOTSPlayer.ModPlayer(player).attackSpeedMod);
            return player.ownedProjectileCounts[item.shoot] <= (player.HasBuff(ModContent.BuffType<Frenzy>()) ? 1 : 0) && player.altFunctionUse == 2;
        }
        public override float UseTimeMultiplier(Player player)
        {
            return player.altFunctionUse == 2 ? 1.1f : 1;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.noMelee = true;
                item.noUseGraphic = true;
                item.UseSound = SoundID.Item19;
                item.axe = 0;
            }
            else
            {
                item.noMelee = false;
                item.noUseGraphic = false;
                item.UseSound = SoundID.Item1;
                item.axe = 16;
            }
            return player.ownedProjectileCounts[item.shoot] <= (player.HasBuff(ModContent.BuffType<Frenzy>()) ? 1 : 0);
        }
    }
}
