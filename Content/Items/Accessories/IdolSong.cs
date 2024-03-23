using BestFriend.Common;
using BestFriend.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend.Content.Items.Accessories;

public class IdolSong : ModItem {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Items/Accessories/IdolSong";
  public override void SetStaticDefaults() {
    CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
  }

  public override void SetDefaults() {
    Item.width = 22;
    Item.height = 28;
    Item.accessory = true;
    Item.rare = ItemRarityID.Yellow;
    Item.value = Item.sellPrice(0, 5, 25);
  }

  public override void UpdateAccessory(Player player, bool hide_visual) {
    var abilities = player.GetModPlayer<BestFriendAbilitiesPlayer>();
    abilities.hit_scan = true;
    player.maxMinions += 2;
    player.GetArmorPenetration(DamageClass.Ranged) += 10;
    player.GetArmorPenetration(DamageClass.Summon) += 10;
    player.GetDamage(DamageClass.Summon) += 0.10f;
    player.GetDamage(DamageClass.Ranged) += 0.10f;
    player.GetCritChance(DamageClass.Summon) += 10;
    player.GetCritChance(DamageClass.Ranged) += 10;
  }

  public override void AddRecipes() {
    // CreateRecipe()
    //   .AddIngredient<PerfectSync>()
    //   .AddIngredient<StarlightAccelerator>()
    //   .Register();
  }
}