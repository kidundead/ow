using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum IAPType
{
    Cash = 0,
    Weapon,
    Avatar,
    Count
}

public enum IAPName
{
    Cash50W,
    Cash120W,
    Cash270W,
    Cash750W,
    Cash1650W,
    None
}


public class IAPItem
{
    public IAPType iType;
    public string ID { get; set; }
    public IAPName Name { get; set; }
    public string Desc { get; set; }
    public Rect textureRect { get; set; }

}

public class Shop
{
    protected List<IAPItem>[] itemList = new List<IAPItem>[(int)IAPType.Count];

    public void CreateIAPShopData()
    {
        for (int i = 0; i < (int)IAPType.Count; i++)
        {
            itemList[i] = new List<IAPItem>();
        }

        IAPItem cash50w = new IAPItem();
        cash50w.ID = "com.trinitigame.callofminizombies.099cents";
        cash50w.iType = IAPType.Cash;
        cash50w.Name = IAPName.Cash50W;
        cash50w.Desc = "$500,000\nMINI PACK";
        cash50w.textureRect = ShopTexturePosition.CashLogo;
        AddIAPItem(cash50w);

        
        IAPItem cash120w = new IAPItem();
        cash120w.ID = "com.trinitigame.callofminizombies.199cents";
        cash120w.iType = IAPType.Cash;
        cash120w.Name = IAPName.Cash120W;
        cash120w.Desc = "$1,200,000\nMEDIUM PACK";
        cash120w.textureRect = ShopTexturePosition.CashLogo;
        AddIAPItem(cash120w);

        IAPItem cash270w = new IAPItem();
        cash270w.ID = "com.trinitigame.callofminizombies.299cents";
        cash270w.iType = IAPType.Cash;
        cash270w.Name = IAPName.Cash270W;
        cash270w.Desc = "$2,700,000\nMEGA PACK";
        cash270w.textureRect = ShopTexturePosition.CashLogo;
        AddIAPItem(cash270w);


        IAPItem cash750w = new IAPItem();
        cash750w.ID = "com.trinitigame.callofminizombies.999cents";
        cash750w.iType = IAPType.Cash;
        cash750w.Name = IAPName.Cash750W;
        cash750w.Desc = "$7,500,000\n XL MEGA PACK";
        cash750w.textureRect = ShopTexturePosition.CashLogo;
        AddIAPItem(cash750w);

        IAPItem cash1650w = new IAPItem();
        cash1650w.ID = "com.trinitigame.callofminizombies.1999cents";
        cash1650w.iType = IAPType.Cash;
        cash1650w.Name = IAPName.Cash1650W;
        cash1650w.Desc = "$16,500,000\n XXL MEGA PACK";
        cash1650w.textureRect = ShopTexturePosition.CashLogo;
        AddIAPItem(cash1650w);

    }


    public void AddIAPItem(IAPItem item)
    {
        itemList[0].Add(item);
    }

    public List<IAPItem>[] GetIAPList()
    {
        return itemList;
    }

}
