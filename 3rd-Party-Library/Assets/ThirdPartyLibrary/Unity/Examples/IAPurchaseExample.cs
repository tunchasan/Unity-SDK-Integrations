using Library.Purchasing;
using UnityEngine;

public class IAPurchaseExample : MonoBehaviour
{
    private IAPurchase purchase;

    private void Awake()
    {
        purchase = new IAPurchase();
    }

    // Initialize IAP Services
    public void InitializeIAPServices()
    {
        purchase.InitializeServices();
    }
    
    // Buy NO AD Product
    public void BuyNoADProduct()
    {
        purchase.BuyNoAddProduct();
    }

}
